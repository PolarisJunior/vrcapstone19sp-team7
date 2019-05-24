﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PercussionObject : MonoBehaviour
{

    public AudioClip hitClip;
    public AudioClip loopClip;
    public Material unlockMaterial;
    public uint hitsToUnlock = 4;
    public bool unlocked = false;
    public bool isPiano = false;
    //public static rhythmComplete = false; //true 

    private Renderer hitRenderer;
    private BeatBlinkController beatBlinkController;
    private AudioSource hitSource;
    private AudioSource loopSource;
    private Hittable hittable;
    private BeatInfo beatInfo;

    private ParticleSystem ps;

    // We can remove this and set values in the prefab
    void Awake()
    {
        beatBlinkController = GetComponent<BeatBlinkController>();
        hitRenderer = transform.Find("HitAnimation").GetComponent<Renderer>();
        

        if (isPiano)
        {
            hitSource = GetComponent<AudioSource>();
            beatInfo = GetComponent<BeatInfo>();
        } else
        {
            hitSource = transform.Find("HitSource").GetComponent<AudioSource>();
            hitSource.spatialBlend = 1.0f;
            hitSource.clip = hitClip;
            loopSource = transform.Find("LoopSource").GetComponent<AudioSource>();
            loopSource.pitch = loopClip.length / MasterLoop.loopTime;
            loopSource.spatialBlend = 1.0f;
            loopSource.clip = loopClip;
            beatInfo = transform.Find("BeatInfo").GetComponent<BeatInfo>(); 
        }

        hittable = GetComponent<Hittable>();
        hittable.hitsToUnlock = hitsToUnlock;
        ps = transform.Find("ParticleSystem").gameObject.GetComponent<ParticleSystem>();

        Renderer objRenderer = GetComponent<Renderer>();
        objRenderer.material.SetFloat("_Completion", 0.0f);
        hittable.onHitOnce.AddListener(delegate() {
            float completion = ((float)hittable.hitCount + 1.0f) / hittable.hitsToUnlock;
            // Debug.Log(completion + " completion");
            objRenderer.material.SetFloat("_Completion", completion);
            // ps.Emit(5);
        });

        hittable.onHitCountReset.AddListener(delegate() {
            objRenderer.material.SetFloat("_Completion", 0.0f);
        });

        beatBlinkController.beatInfo = beatInfo;
    }

    public void NewLoop()
    {
        if (gameObject.activeInHierarchy)
        {
            beatBlinkController.NewLoop();
            if (!isPiano) // && !rhythmComplete)
            {
                loopSource.Play();
            }
        }
    }

    public void Unlock()
    {
        unlocked = true;
        Invoke("LoopSourceOn", hitClip.length + .1f);
        hitRenderer.material = unlockMaterial;
        GetComponent<Renderer>().material = unlockMaterial;
        Color color = this.GetComponent<MeshRenderer>().material.color;
        color.a = .2f;
        this.GetComponent<MeshRenderer>().material.color = color;
    }

    public void HitOnce()
    {
        if (!unlocked || !isPiano)
        {
            hitSource.Play();
        }
    }

    float GetCompletion() {
        // hitsToUnlock / 
        return 0.0f;
    }
        
    void LoopSourceOn()
    {
        if (loopSource != null)
        {
            loopSource.volume = 1.0f;
        }
    }

    
}
