using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private int tempScaleFactor = 1;
    // Start is called before the first frame update
	public void Move(int scaleFactor) {
		StartCoroutine(MoveDownSteps());
        tempScaleFactor = scaleFactor;
	}

    public IEnumerator MoveDownSteps(){
		for (int i = 0; i < 100; i++) {
	    	yield return new WaitForSeconds(.025f);
	    	transform.position += new Vector3(0, tempScaleFactor * -.004f, 0);
    	}
    }

    
    
}
