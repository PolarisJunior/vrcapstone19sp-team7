﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HarmonyDestinationActions : MonoBehaviour
{
    public float delayDepart = 4f;

    public UnityEvent moveToCenter;

    public void Arrive()
    {
        gameObject.SetActive(true);
    }

    public void Depart()
    {
        StartCoroutine(DelayDepart());
    }

    private IEnumerator DelayDepart() {
        yield return new WaitForSeconds(delayDepart);

        moveToCenter.Invoke();
    }
}
