﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class HandController : MonoBehaviour
    {
        public bool debugMode = true;

        public Hand leftHand;
        public Hand rightHand;
        
        // [EnumFlags]
        // public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.
        // Start is called before the first frame update
        void Start()
        {
            

        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(leftHand.transform.position);
            
            RaycastHit hit;
            if (Physics.Raycast(leftHand.transform.position, leftHand.transform.rotation * transform.forward, Mathf.Infinity, ~(1 << 2))) {
                // Debug.Log("we have hit");
            }
            // Debug.Log(leftHand.transform.eulerAngles);
            if (debugMode) {
                Debug.DrawRay(leftHand.transform.position, leftHand.transform.rotation * transform.forward * 1000, Color.green);
                Debug.DrawRay(rightHand.transform.position, rightHand.transform.rotation * transform.forward * 1000, Color.blue);
            }

            
            
        }

    }

}

