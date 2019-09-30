using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour {
	
	bool isPaused = false;

	// Use this for initialization
	void Start () {
		
		//bool isPaused = false;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButton(0))
		{
			isPaused = true;
			Time.timeScale = 0f;
		}
		
		if (Input.GetMouseButton(0) && isPaused == true)
		{
			isPaused = false;
			Time.timeScale = 1;
		}
	
	}
}
