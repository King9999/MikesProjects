/* The HUD for the title screen. The player can press the start button to start the game. */

using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour 
{
    public GUISkin titleSkin;
    public Texture2D playIcon;
    public Texture2D instructIcon;
    public Texture2D creditsIcon;
    public Texture2D cursor;

    bool stickPressed;

    Rect cursorRect;
    float cursorPos;
    int menuSelect;     //0 = main game, 1 = instruction page, 2 = credits
    
	// Use this for initialization
	void Start () 
    {
        menuSelect = 0;
        cursorPos = Screen.width / 4;
        cursorRect = new Rect(cursorPos, 620, cursor.width, cursor.height);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //if either the enter key or the start button on the Xbox controller is pressed, the screen changes.
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Start"))
        {
            if (menuSelect == 0)
                Application.LoadLevel("LevelScene1");
            else if (menuSelect == 1)
                //load instruction screen
                 Application.LoadLevel("InstructionScene");
            else if (menuSelect == 2)
                //load credits screen
                Application.LoadLevel("CreditsScene");
        }

        if (Input.GetKeyUp(KeyCode.A) || (Input.GetAxis("Horizontal") == -1 && !stickPressed))
        {
            stickPressed = true;
            if (menuSelect == 0)
                menuSelect = 2;
            else
                menuSelect--;

            switch (menuSelect)
            {
                case 0:
                    cursorPos = Screen.width / 4;
                    break;
                case 1:
                    cursorPos = Screen.width / 2.5f;
                    break;
                case 2:
                    cursorPos = Screen.width / 1.4f;
                    break;
            }

            cursorRect = new Rect(cursorPos, 620, cursor.width, cursor.height);
        }

        if (Input.GetKeyUp(KeyCode.D) || (Input.GetAxis("Horizontal") == 1 && !stickPressed))
        {
            stickPressed = true;
            if (menuSelect == 2)
                menuSelect = 0;
            else
                menuSelect++;

            switch (menuSelect)
            {
                case 0:
                    cursorPos = Screen.width / 4;
                    break;
                case 1:
                    cursorPos = Screen.width / 2.5f;
                    break;
                case 2:
                    cursorPos = Screen.width / 1.4f;
                    break;
            }

            cursorRect = new Rect(cursorPos, 620, cursor.width, cursor.height);

        }

        if (Input.GetAxis("Horizontal") == 0)
        {
            stickPressed = false;
        }
	}

    void OnGUI()
    {
        GUI.skin = titleSkin;
        GUI.Label(cursorRect, cursor);
        GUI.Label(new Rect(Screen.width / 4, 650, playIcon.width / 2, playIcon.height / 2), playIcon);
        GUI.Label(new Rect(Screen.width / 2.5f, 650, instructIcon.width, instructIcon.height), instructIcon);
        GUI.Label(new Rect(Screen.width / 1.4f, 650, creditsIcon.width / 1.5f, creditsIcon.height), creditsIcon);
        //GUI.Label(new Rect(Screen.width * 0.45f, Screen.height * 0.83f, 400, 80), "Press Start or [Space] to Begin");
    }
}
