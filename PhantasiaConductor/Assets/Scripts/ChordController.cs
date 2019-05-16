﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Valve.VR.InteractionSystem
{
    [RequireComponent (typeof(CustomSpawnAndAttachToHand))]
    public class ChordController : MonoBehaviour
    {
        public GameObject[] targets;

        public Material targetOn;
        public Material targetOff;

        public UnityEvent onCompleteChord;

        private AudioSource loopSource;
        private BeatInfo beatInfo;
        private CustomSpawnAndAttachToHand spawning;
        private int beatIndex = -1;
        private int targetIndex = 0; 
        private GameObject leftThrowable;
        private GameObject rightThrowable;
        private GameObject leftSphere;
        private GameObject rightSphere;



        private bool[] completed;
        private bool waitForLoop = false;
        private bool notePlaying = false;
        private bool complete = false;
        private bool invokeComplete = false;

        private void Awake()
        {
            loopSource = loopSource = transform.Find("LoopSource").GetComponent<AudioSource>();
            beatInfo = transform.Find("BeatInfo").GetComponent<BeatInfo>();
            spawning = GetComponent<CustomSpawnAndAttachToHand>();
            completed = new bool[targets.Length];
        }

        private void OnEnable()
        {
            waitForLoop = true;
            beatIndex = -1;
            targetIndex = 0;
            RunTarget();
        }

        private void OnDisable()
        {
            CancelInvoke();
        }

        public void NewLoop()
        {
            if (gameObject.activeInHierarchy)
            {
                targetIndex = 0;
                beatIndex = -1;
                loopSource.Play();

                if (!complete)
                {
                    waitForLoop = false;
                    notePlaying = false;

                    int finishCount = 0;
                    foreach (bool finish in completed)
                    {
                        if (finish)
                        {
                            finishCount++;
                        }
                    }
                    if (finishCount == completed.Length)
                    {
                        complete = true;
                        invokeComplete = true;
                    }
                }

                if (invokeComplete)
                {
                    invokeComplete = false;
                    onCompleteChord.Invoke();
                    foreach (GameObject target in targets) {
                        target.GetComponent<Target_hit>().Complete = true;
                        target.GetComponent<Target_hit>().CanHit = false;
                    }
                    PlayAnimation();
                } 
            }

        }

        private void RunTarget()
        {
            Debug.Log("RT");
            if (!waitForLoop)
            {
                bool nextBeat = beatInfo.beats[(beatIndex + 1) % beatInfo.beats.Length];
                beatIndex++;

                if (beatIndex  != 0 && beatInfo.beats[(beatIndex - 1) % beatInfo.beats.Length] 
                        && completed[targetIndex]) {
                    targets[targetIndex].GetComponent<Target_hit>().playTarget();
                    targetIndex++;
                }
                if (nextBeat && !completed[targetIndex])
                {
                    targets[targetIndex].GetComponent<Renderer>().material = targetOn;
                    targets[targetIndex].GetComponent<Target_hit>().CanHit = true;   
                    notePlaying = true;
                }
                else if (!nextBeat && notePlaying)
                {
                    if (targetIndex < targets.Length)
                    {
                        StartCoroutine(HitWindowOff(targetIndex));
                    }
                }
            }
            if (targetIndex < targets.Length)
            {
                Invoke("RunTarget", beatInfo.beatTime);
            } else {
                complete = true;
                invokeComplete = true;
            }
        }

        private void PlayAnimation()
        {
            bool nextBeat = beatInfo.beats[(beatIndex + 1) % beatInfo.beats.Length];
            beatIndex++;

            if (beatIndex != 0 && beatInfo.beats[(beatIndex - 1) % beatInfo.beats.Length])
            {
                targets[targetIndex].GetComponent<Target_hit>().playTarget();
                targetIndex++;
            }

            Invoke("PlayAnimation", beatInfo.beatTime);
        }

        private IEnumerator HitWindowOff(int index) {
            yield return new WaitForSeconds(beatInfo.hittableAfter * beatInfo.beatTime);

            targets[index].GetComponent<Renderer>().material = targetOff;
            targets[index].GetComponent<Target_hit>().CanHit = false;
            if (!completed[index]) {
                waitForLoop = true;
            } else if (index == targetIndex)
            {
                    targetIndex++;
            }
            notePlaying = false;

            
        }

        public void ResetTargets()
        {
            NewLoop();
            waitForLoop = true;
        }

        public void HitNote() {
            if (targetIndex == 0 || completed[targetIndex - 1]) {
                targets[targetIndex].GetComponent<Target_hit>().CanHit = false;
                completed[targetIndex] = true;
            }
        }
    }
}
