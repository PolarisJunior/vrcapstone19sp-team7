﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class RadialObject : MonoBehaviour
{
    public UnityEvent onSuccess;

    public UnityEvent onFailed;


    public float lifetime = 4f;


    [HideInInspector]
    public bool isLastObject;

    private RadialSequence ownerSequence;

    private int groupId_;


    void Start() {
        Invoke("EndOfLifetime", lifetime);
    }

    void EndOfLifetime()
    {
        onFailed.Invoke();
        Destroy(gameObject);
    }

    // sequence this reports to for finishing
    public void BindSequence(RadialSequence seq)
    {
        ownerSequence = seq;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // we caught it
        onSuccess.Invoke();
        ownerSequence.ObjectCaught(groupId);
        Destroy(gameObject);
    }

    void Fadeout()
    {

    }

    void OnDestroy()
    {
        if (isLastObject)
        {
            ownerSequence.LastObjectDestroyed(groupId);
        }
    }

    public int groupId
    {
        get
        {
            return groupId_;
        }

        set
        {
            groupId_ = value;
        }
    }

}