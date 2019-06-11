using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgnosticHand : MonoBehaviour
{
    public GameObject steamHand;

    public GameObject oculusHand;


    public bool useOculus = false;

    public bool isRight;

    private Baton baton;

    static Baton rightBaton;
    static Baton leftBaton;


    void Awake()
    {
        GameObject src = useOculus ? oculusHand : steamHand;

        transform.parent = src.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        baton = transform.Find("baton").gameObject.GetComponent<Baton>();
        if (baton == null)
        {
            Debug.Log("Could not find baton");
        }
        else
        {
            if (isRight)
            {
                rightBaton = baton;
            }
            else
            {
                leftBaton = baton;
            }
        }
    }


    public Baton GetBaton()
    {
        return baton;
    }

    // private Valve.VR.SteamVR_Action_Boolean pinchAction = Valve.VR.SteamVR_Input.GetAction<Valve.VR.SteamVR_Action_Boolean>("GrabPinch");
    // public bool TriggerDown()
    // {
    //     Valve.VR.SteamVR_Input_Sources handType = steamHand.GetComponent<Valve.VR.InteractionSystem.Hand>().handType;
    //     return pinchAction.GetStateDown(handType);
    // }

    // public bool TriggerUp()
    // {
    //     Valve.VR.SteamVR_Input_Sources handType = steamHand.GetComponent<Valve.VR.InteractionSystem.Hand>().handType;
    //     return pinchAction.GetStateUp(handType);
    // }

    // public bool IsTriggerDown()
    // {
    //     Valve.VR.SteamVR_Input_Sources handType = steamHand.GetComponent<Valve.VR.InteractionSystem.Hand>().handType;
    //     return pinchAction.GetState(handType);
    // }

    public bool TriggerDown() {
        OVRGrabber grabber = oculusHand.GetComponent<OVRGrabber>();
        OVRInput.Controller controller = isRight ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
        return OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller);
    }

    public bool IsTriggerDown()
    {
        OVRGrabber grabber = oculusHand.GetComponent<OVRGrabber>();
        OVRInput.Controller controller = isRight ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
        return OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller);
    }

    public bool TriggerUp() {
        OVRGrabber grabber = oculusHand.GetComponent<OVRGrabber>();
        OVRInput.Controller controller = isRight ? OVRInput.Controller.RTouch : OVRInput.Controller.LTouch;
        return OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controller);
    }

    public static Baton GetRightBaton()
    {
        return rightBaton;
    }

    public static Baton GetLeftBaton()
    {
        return leftBaton;
    }
}
