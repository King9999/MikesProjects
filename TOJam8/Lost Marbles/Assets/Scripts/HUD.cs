//Displays information for the player.  There is a ticker at the bottom of the screen that updates with
//various information.  Pressing Select or Space should restart the level.

using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour 
{
    public static int pesos { get; set; }         //points received for destruction.
    public static int crushCount { get; set; }    //numbr of objects destroyed
    public GUISkin FontSkin;
    public GUISkin tickerSkin;
    public AudioClip music;
    AudioSource source;
    public static int timer;          //game clock in milliseconds.
    public static int targetPesos;       //amount of pesos needed before level can be completed.

    public static string dogPoints = "+200 Pesos";
    public static string housePoints = "+1000 Pesos";
    public static string goatPoints = "+2000 Pesos";
    public static string timerBonusStr;
    public static string ptsDisplayString;
    public static bool pointsOnDisplay;                //provides onscreen feedback to player
    public static Rect ptsDisplayRect;
    public static Rect timerBonusRect;
    int displayTimer;
    bool gamePaused;
    bool gameOver;

    //ticker
    public static string tickerMsg;
    public static bool tickerOnDisplay;
    Rect tickerRect;        //used to scroll message across the screen.
    public static int tickerXPos;

	// Use this for initialization
	void Start () 
    {
        gameOver = false;
        gamePaused = false;
        //get audio source
        source = GetComponent<AudioSource>();
        source.clip = music;
        source.Play();

        pesos = PlayerPrefs.GetInt("Peso");
        timer = PlayerPrefs.GetInt("Timer");
        targetPesos = 10000;
        crushCount = 0;
        pointsOnDisplay = false;
        displayTimer = 0;
        tickerXPos = Screen.width;
        tickerRect = new Rect(tickerXPos, 700, 10000, 80);  //starts off screen.
        
        tickerOnDisplay = false;
        Time.timeScale = 1;

        //try
        //{
        //    timer = PlayerPrefs.GetInt("Timer");
        //    PlayerPrefs.DeleteKey("Timer");
        //}
        //catch
        //{
        //    Debug.Log("Timer key not found");
        //    timer = 600;
        //}
	}

    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!gameOver)
        {
            if (!gamePaused)
            {
                //timer countdown
                timer--;
                if (timer <= 0)
                {
                    //gamePaused = true;
                    gameOver = true;
                }

                if (pointsOnDisplay)
                {
                    displayTimer++;
                    ptsDisplayRect.y--;
                    timerBonusRect.y--;
                    if (displayTimer > 120)
                    {
                        pointsOnDisplay = false;
                        displayTimer = 0;
                    }
                }

                if (tickerOnDisplay)
                {
                    tickerXPos -= 4;
                    tickerRect = new Rect(tickerXPos, 700, 10000, 80);
                    Debug.Log("Ticker X: " + tickerXPos);
                    if (tickerXPos < -tickerRect.width)
                    {
                        tickerXPos = Screen.width;
                        tickerOnDisplay = false;
                    }
                }
            }
            else    //game is paused
            {
                Time.timeScale = 0;
                if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Back"))
                {
                    Time.timeScale = 1;
                    Application.LoadLevel("TitleScene");
                }
            }

            if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Start"))
            {
                gamePaused = !gamePaused;
                if (gamePaused)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            }
        }
        else    //game is over
        {
            Time.timeScale = 0;
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetButtonUp("Back"))
            {
                Time.timeScale = 1;
                Application.LoadLevel("TitleScene");
            }
        }
	}

    void OnGUI()
    {
        GUI.skin = FontSkin;
        GUI.Label(new Rect(100, 50, 200, 80), "Pesos: " + pesos);
        GUI.Label(new Rect(100, 80, 200, 80), "Time: " + timer);

        if (pointsOnDisplay)
        {
            GUI.Label(ptsDisplayRect, ptsDisplayString);
            GUI.Label(timerBonusRect, "Bonus Time +" + timerBonusStr);
        }
        GUI.skin = tickerSkin;
        GUI.Label(tickerRect, tickerMsg);

        if (gamePaused)
        {
            GUI.Label(new Rect(Screen.width / 2.5f, Screen.height / 2, 300, 80), "- P A U S E -");
            GUI.Label(new Rect(Screen.width / 2.5f, Screen.height / 1.5f, 500, 120), "Press ESC or Back button to quit game");
        }

        if (gameOver)
        {
            GUI.skin = FontSkin;
            GUI.Label(new Rect(Screen.width / 2.5f, Screen.height / 2, 400, 80), "- G A M E  O V E R -");
            GUI.skin = tickerSkin;
            GUI.Label(new Rect(Screen.width / 2.5f, Screen.height / 1.5f, 500, 120), "Press ESC or Back button to quit game");
        }
        
    }

}
