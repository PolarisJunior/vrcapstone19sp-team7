﻿using System.Collections;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class DestinationActions : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject puzzlePrefab;
        public GameObject startingPosition;
        public int returnDelay = 1;
        public GameObject[] nextActive;
        public Material[] colorMaterials;

        private GameObject instruments;
        private GameObject teleportIndicator;
        private float puzzleHeight = 1.5f;

        void Awake() {
            instruments = transform.Find("Instruments").gameObject;
            teleportIndicator = transform.Find("TeleportInd").gameObject;
        }

        public void onArrive() {
            Debug.Log("called onArrive");
            if (puzzlePrefab != null) 
            {
                int childCount = instruments.transform.childCount;
                for (int i = 0; i < childCount; i++) {
                    // should probably animate this 
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                if (teleportIndicator != null)
                {
                    teleportIndicator.SetActive(false);
                }

                Debug.Log("setting puzzle active");
                puzzlePrefab.SetActive(true);
            }
        }

        public void onPrepareToLeave() {
            if (puzzlePrefab != null)
            { 
                StartCoroutine(glowIndicator());
            }
        }

        private IEnumerator glowIndicator() {
            startingPosition.GetComponent<Glow>().GlowOn();

            yield return new WaitForSeconds(returnDelay);

            // puzzlePrefab.SetActive(false);
            Vector3 newPos = puzzlePrefab.transform.position;
            newPos.y += puzzleHeight;

            puzzlePrefab.transform.position = newPos;

            Renderer[] renderers = instruments.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = colorMaterials[i];
            }

            for (int i = 0; i < renderers.Length; i++)
            {
                Transform child = transform.GetChild(i);

                child.gameObject.SetActive(true); // should probably animate
            }
            Player.instance.GetComponent<PerspectiveShift>().TeleportTo(startingPosition);
            startingPosition.GetComponent<Glow>().GlowOff();

            foreach (GameObject next in nextActive)
            {
                next.SetActive(true);
            }
        }
    }
}