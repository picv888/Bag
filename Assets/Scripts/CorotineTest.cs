using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorotineTest : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
        Debug.Log(123213);
        yield return new WaitForSeconds(5);
        Debug.Log(123213);
        yield return new WaitForSeconds(5);
        Debug.Log(123213);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
