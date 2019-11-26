//initializes data for use by other scripts.  This must be run on the TitleScene so that
//the player starts the game fresh.

using UnityEngine;
using System.Collections;

public class InitPlayerData : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
    {
        PlayerPrefs.SetInt("Timer", 600);
        PlayerPrefs.SetInt("Peso", 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
