﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodyObject : MonoBehaviour
{

    public AudioClip loopClip;

    public Material windowOnMat;

    public Material windowOffMat;


    // mat to use while successfully following
    public Material trackingMat;

    // mat to use after failed
    public Material failMat;

    public PathBeat pathBeat;

    // public float startTime;
    // public float endTime;
    public float windowLength = 1f;
    public bool unlocked = false;
    public Valve.VR.InteractionSystem.PuzzleSequence puzzleSequence;

    private AudioSource loopSource;
    private Collider coll;
    private MeshRenderer rend;

    private Hittable hittable;

    private bool windowStatus = false;

    private bool isMoving;


    void Awake()
    {
        coll = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
        loopSource = GetComponent<AudioSource>();

        isMoving = false;

        // loopSource.clip = loopClip;
        // loopSource.pitch = loopClip.length / MasterLoop.loopTime;
        // loopSource.spatialBlend = 1.0f;
        // loopSource.clip = loopClip;

        hittable = GetComponent<Hittable>();


    }

    void Start()
    {
        hittable.canInteract = false;
        if (gameObject.activeInHierarchy)
        {
            rend.enabled = true;
        }
    }

    public void NewLoop()
    {
        if (gameObject.activeInHierarchy)
        {
            // just keep looping if unlocked
            if (unlocked)
            {
                pathBeat.ResetPosition();
            }

            // if still locked and not moving then handle the window indicator

            if (!pathBeat.moving)
            {
                WindowOn();
                Invoke("WindowOff", windowLength);
            }
        }


        // if (gameObject.activeInHierarchy) {
        //     Debug.Log("new melody object loop");
        //     rend.enabled = !rend.enabled;
        //     windowStatus = !windowStatus;

        //     if (windowStatus) {
        //         rend.material = windowOnMat;
        //     } else {
        //         rend.material = windowOffMat;
        //     }
        // }

        // loopSource.Play();
        // Invoke("StartWindow", MasterLoop.loopTime * startTime);
        // Invoke("StartPlay", MasterLoop.loopTime * (startTime + windowLength));
        // Invoke("EndPlay", MasterLoop.loopTime * endTime);
    }

    public void WindowOn()
    {
        windowStatus = true;
        rend.material = windowOnMat;
        hittable.canInteract = true;
    }

    public void WindowOff()
    {
        windowStatus = false;
        // only if  not moving
        if (!pathBeat.moving)
        {
            rend.material = windowOffMat;
            hittable.canInteract = false;
        }
    }

    public Material GetWindowMaterial() {
        return windowStatus ? windowOnMat : windowOffMat;
    }

    public void UnlockObject()
    {
        unlocked = true;
    }

    public void ObjectFailed() {
        rend.material = GetWindowMaterial();
        pathBeat.Reset();
    }

}