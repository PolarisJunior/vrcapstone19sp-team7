﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using System.IO;

public class PathBeat : MonoBehaviour
{
    public UnityEvent onReachedEnd;
    public UnityEvent onBegan;

    // reached end but failed
    public UnityEvent onReachedEndBad;

    // invoked when completed and successful
    public UnityEvent onSuccessful;
    // invoked when failed
    public UnityEvent onFailed;

    public Material goodMat;

    public Material failedMat;


    public enum PathMode
    {
        SPEED_CONSTANT,
        TIMED
    }

    public LineRenderer lineRenderer;

    // time spent at each part of the line
    public List<float> timeMap = new List<float>();

    public GameObject obj;

    public PathMode pathMode = PathMode.SPEED_CONSTANT;

    // units of movement per sec
    public float speed = 5;

    public bool isRenderingLine;

    float timeElapsed;
    int index;

    bool isMoving = false;


    bool hasFailed = false;

    bool hasSuccessfullyCompleted = false;

    // was marked this frame
    bool wasMarked = false;

    const string directory = "Assets/Paths/";

    void Start()
    {
        lineVisible = isRenderingLine;
    }

    void Update()
    {

        if (!moving)
        {
            return;
        }
        if (canMoveForward)
        {
            Vector3 v;
            float t = timeMap[index];

            switch (pathMode)
            {
                case PathMode.TIMED:
                    t = timeMap[index];
                    break;
                case PathMode.SPEED_CONSTANT:
                    t = CalcTimeTraverse(lineRenderer.GetPosition(index), lineRenderer.GetPosition(index + 1));
                    break;

            }

            float completion = timeElapsed / t;

            v = Vector3.Lerp(lineRenderer.GetPosition(index),
                             lineRenderer.GetPosition(index + 1),
                             completion);

            obj.transform.localPosition = v;

            if (timeElapsed > t)
            {
                timeElapsed -= t;
                index++;

                if (!canMoveForward)
                {
                    onReachedEnd.Invoke();
                    if (!hasSuccessfullyCompleted)
                    {
                        if (!hasFailed)
                        {
                            // succeeded for the first time
                            OnSuccess();
                        }
                        else
                        {
                            OnReachedEndBad();
                        }
                    }

                    // return to the start
                    ResetPosition();
                }
            }

            timeElapsed += Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        if (!hasSuccessfullyCompleted && canMoveForward && isMoving &&
            !hasFailed && !wasMarked)
        {
            // was not marked in the most recent update frame
            OnFailed();
        }

        wasMarked = false;
    }

    /* 
        Marks the object as having been hit this frame
     */
    public void markAsHit()
    {
        wasMarked = true;
    }

    // resets only the position
    public void ResetPosition()
    {
        index = 0;
        obj.transform.localPosition = lineRenderer.GetPosition(index);
    }

    // resets to a state as if it was never started
    public void Reset()
    {
        index = 0;
        obj.transform.localPosition = lineRenderer.GetPosition(index);
        moving = false;
        hasFailed = false;

        Renderer renderer = obj.GetComponent<Renderer>();
        renderer.material = goodMat;
    }

    public void Begin()
    {
        moving = true;
        onBegan.Invoke();
    }

    public void Stop()
    {
        moving = false;
    }

    public void AddVertex(Vector3 pos, float t)
    {
        // no line is drawn if only one vertex exists
        if (vertexCount > 0)
        {
            timeMap.Add(t);
        }
        vertexCount += 1;
        lineRenderer.SetPosition(vertexCount - 1, pos);
    }

    public void SetCompletionTime(float t)
    {
        switch (pathMode)
        {
            case PathMode.SPEED_CONSTANT:
                // Debug.Log(pathLength + " " + t);
                this.speed = pathLength / t;
                break;
            case PathMode.TIMED:
                Debug.Log("set completion time not supported for PathMode.TIMED");
                break;
        }
    }

    public void LoadFromFile(string path)
    {
        path = directory + path + ".txt";
        StreamReader reader = new StreamReader(path);
        string line;
        while (!reader.EndOfStream)
        {
            line = reader.ReadLine();

            var tokens = line.Split(',');
            var v = new Vector3(float.Parse(tokens[0]), float.Parse(tokens[1]), float.Parse(tokens[2]));
            AddVertex(v, 1f);
        }


        obj.transform.parent = transform;
        obj.transform.localPosition = lineRenderer.GetPosition(0);
        obj.layer = 1 << 2;

        Hittable hittable = obj.GetComponent<Hittable>();
        if (hittable != null)
        {
            hittable.onPinched.AddListener(delegate ()
            {
                if (!isMoving)
                {
                    Begin();
                }
            });

            hittable.onTracked.AddListener(delegate ()
            {
                markAsHit();
            });
        }
    }

    public void Hello()
    {
        Debug.Log("Hello " + gameObject.name);
    }

    private float CalcTimeTraverse(Vector3 start, Vector3 end)
    {
        var dist = Vector3.Distance(start, end);
        return dist / speed;
    }

    public float pathLength
    {
        get
        {
            float totalDist = 0f;
            Vector3[] ps = positions;
            for (int i = 0; i < vertexCount - 1; i++)
            {
                totalDist += Vector3.Distance(ps[i], ps[i + 1]);
            }

            return totalDist;
        }
    }

    // seconds
    public float pathTime
    {
        get
        {
            switch (pathMode)
            {
                case PathMode.SPEED_CONSTANT:
                    return pathLength / speed;
                case PathMode.TIMED:
                    float s = 0f;
                    foreach (var v in timeMap)
                    {
                        s += v;
                    }
                    return s;
                default:
                    return 0;
            }
        }
    }

    public bool lineVisible
    {
        get
        {
            return isRenderingLine;
        }

        set
        {
            lineRenderer.enabled = value;
            isRenderingLine = value;
        }
    }

    public Vector3[] positions
    {
        get
        {
            int count = lineRenderer.positionCount;
            Vector3[] p = new Vector3[count];

            lineRenderer.GetPositions(p);
            return p;
        }
    }

    public bool canMoveForward
    {
        get
        {
            return index < vertexCount - 1;
        }
    }

    private bool moving {
        get {
            return isMoving;
        }

        set {
            isMoving = value;
        }
    }

    private int vertexCount
    {
        get
        {
            return lineRenderer.positionCount;
        }

        set
        {
            lineRenderer.positionCount = value;
        }
    }

    private void OnFailed()
    {
        hasFailed = true;
        onFailed.Invoke();

        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = failedMat;
        }
    }

    private void OnSuccess()
    {
        onSuccessful.Invoke();
        Renderer renderer = obj.GetComponent<Renderer>();
        var color = renderer.material.color;
        renderer.material.color = new Color(color.r, color.g, color.b, 1);

        hasSuccessfullyCompleted = true;
    }

    private void OnReachedEndBad()
    {
        onReachedEndBad.Invoke();
        Reset();
        // mark so that we don't immediately fail
        wasMarked = true;
        moving = true;
    }
}
