using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int timeLeft = 5;
    public Canvas darkCanvas;
    public Text countdown;
    public GameController controller;
    public AgnosticHand leftHand;
    public AgnosticHand righttHand;

    public bool trig = false;
    public int cSwitch = 0;

    void OnEnable()
    {
        Invoke("Dim", 0);
        countdown.GetComponent<Text>().enabled = true;
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    void Update()
    {
        countdown.text = ("Keep holding the trigger to skip puzzle in " + timeLeft);
        if (!(leftHand.IsTriggerDown() || righttHand.IsTriggerDown()))
        {
            StopCoroutine("LoseTime");
            darkCanvas.enabled = false;
            timeLeft = 5;
            this.enabled = false;
        }

        else if (timeLeft == 0)
        {
            countdown.GetComponent<Text>().enabled = false;
            
            //case 1 = rhythm, case 2 = bass, case 3 = melody, case 4 = harmony

            switch (cSwitch)
            {
                case 1:
                    trig = true;
                    Debug.Log("rhythm skip " + cSwitch);
                    break;

                case 2:
                    trig = true;
                    Debug.Log("bass skip " + cSwitch);
                    break;

                case 3:
                    trig = true;
                    Debug.Log("melody skip " + cSwitch);
                    break;

                case 4:
                    trig = true;
                    Debug.Log("harmony skip " + cSwitch);
                    break;

            }

            //controller.SetNextActive();
            darkCanvas.enabled = false;
            timeLeft = 5;
            this.enabled = false;

        }
    }
    
    IEnumerator LoseTime()
    {
        while (this.isActiveAndEnabled)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }

    }

    void Dim()
    {
        darkCanvas.enabled = true;
    }
}
