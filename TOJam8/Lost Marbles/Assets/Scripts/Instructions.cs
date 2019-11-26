//Displays the tutorial screen.

using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour 
{
    public Texture2D tutorialScreen;
    public GUISkin fontSkin;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	     if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Back"))
         {
            
            Application.LoadLevel("TitleScene");
           
         }
	}

    void OnGUI()
    {
        GUI.skin = fontSkin;
        GUI.Label(new Rect(0, 0, tutorialScreen.width, tutorialScreen.height), tutorialScreen);
        GUI.Label(new Rect(Screen.width / 2.5f, 730, 400, 60), "EXIT - [Space] or Back");
    }
}
