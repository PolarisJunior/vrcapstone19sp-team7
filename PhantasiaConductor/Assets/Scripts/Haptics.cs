﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class Haptics : MonoBehaviour
{
    public SteamVR_Action_Vibration hapticAction;
    void Start()
    {
        Debug.Log("Hello Windows Haptics");
    }

    public void PulseLeft()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.LeftHand);

    }

    public void PulseRight()
    {
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.RightHand);

    }

    public void sweepHandRight()
    {
        //hapticAction.Execute(0, 1, 75, 75, SteamVR_Input_Sources.RightHand);
        //hapticAction.Execute(0, 0.5f, 100, 75, SteamVR_Input_Sources.RightHand);
        hapticAction.Execute(0, 1, 125, 60, SteamVR_Input_Sources.RightHand);
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.RightHand);


    }
    public void sweepHandLeft()
    {
        //hapticAction.Execute(0, 1, 75, 20, SteamVR_Input_Sources.LeftHand);
        //hapticAction.Execute(0, 0.5f, 100, 40, SteamVR_Input_Sources.LeftHand);
        hapticAction.Execute(0, 1, 125, 60, SteamVR_Input_Sources.LeftHand);
        hapticAction.Execute(0, 1, 150, 75, SteamVR_Input_Sources.LeftHand);
    }

    // void Start()
    // {
    //     Debug.Log("Hello Android");
    // }

    // public IEnumerator PulseLeft()
    // {
    //     OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
    //     yield return new WaitForSeconds(1.0f);
    //     OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    // }

    // public IEnumerator PulseRight()
    // {
    //     OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
    //     yield return new WaitForSeconds(1.0f);
    //     OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    // }

    // public IEnumerator sweepHandRight()
    // {
    //     OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.RTouch);
    //     yield return new WaitForSeconds(1.0f);
    //     OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    // }

    // public IEnumerator sweepHandLeft()
    // {
    //     OVRInput.SetControllerVibration(0.5f, 0.5f, OVRInput.Controller.LTouch);
    //     yield return new WaitForSeconds(1.0f);
    //     OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    // }


}
