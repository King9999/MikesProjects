//this is used by the marble to check for collision between crushable objects.  Any crushable objects are destroyed.

using UnityEngine;
using System.Collections;

public class CollisionCheck : MonoBehaviour 
{

    public AudioClip[] crushSound;
    public AudioClip explodeSound;
    public AudioClip goatSound;
    public AudioClip teleportSound;
   
    public Transform smokePrefab;
    public Transform houseDebrisPrefab;
    

    //randomized messages
    string[] dogCrushText;
    string[] houseCrushText;
    string[] goatCrushText;
    const int MAX_MSGS = 3;

    //HUD hud = new HUD();
	// Use this for initialization
	void Start () 
    {
       

        dogCrushText = new string[MAX_MSGS];
        houseCrushText = new string[MAX_MSGS];
        goatCrushText = new string[MAX_MSGS];

        dogCrushText[0] = "That was a dog!  How could you??";
        dogCrushText[1] = "That dog had a family...!";
        dogCrushText[2] = "Did you not read the rules? DON'T crush anything!";

        houseCrushText[0] = "That was someone's home!";
        houseCrushText[1] = "Who's going to pay for the damages, huh?";
        houseCrushText[2] = "Wow, aren't you...uncooperative.";

        goatCrushText[0] = "There is no goat level.";
        goatCrushText[1] = "Thanks for playing!";
        goatCrushText[2] = "This game was brought to you by the Space Frogs team for TOJam: Haters Gonna Eight. Copyright 2013. All rights reserved. Made in Canada. If you're still reading this, you must be bored. There is nothing more to say to you. We mean it. Seriously.  Go home.                                             All right, since you're still reading, here's a question: do you know the meaning behind the goat on a pole?...                                                      Sorry, were you looking for an answer? :)";

	}

    void Awake()
    {
       
        DontDestroyOnLoad(teleportSound);
    }
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider target)
    {
        int num = Random.Range(0, 3);       //used to play random sound effects.
        //if the marble collides with a dog, the dog is destroyed.
        if (target.GetComponent<Collider>().tag == "Dog")
        {
            AudioSource.PlayClipAtPoint(crushSound[num], target.gameObject.transform.position);
            Instantiate(smokePrefab, target.gameObject.transform.position, target.gameObject.transform.rotation);
            Destroy(target.gameObject);
            HUD.pesos += 200;
            HUD.timer += 100;
            HUD.timerBonusStr = "100";
            HUD.crushCount++;
            HUD.pointsOnDisplay = true;
            HUD.ptsDisplayString = HUD.dogPoints;

            //set up ticker
            HUD.tickerXPos = Screen.width;
            HUD.tickerMsg = dogCrushText[num];
            HUD.tickerOnDisplay = true;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            if (screenPos.y < 0 || screenPos.y > Screen.height)
                screenPos.y = Screen.height / 2;
            if (screenPos.x > Screen.width || screenPos.x < 0)
                screenPos.x = Screen.width / 2;

            //Debug.Log(screenPos.x + ", " + screenPos.y);
            HUD.ptsDisplayRect = new Rect(screenPos.x, screenPos.y, 300, 80);
            HUD.timerBonusRect = new Rect(screenPos.x, screenPos.y + 50, 300, 80);
            //Debug.Log("hit dog");
        }

        if (target.GetComponent<Collider>().tag == "Hut")
        {
            AudioSource.PlayClipAtPoint(explodeSound, target.gameObject.transform.position);

             //replace house with smoke particles + wooden planks. This is done by instantiating the smoke prefab.
            Instantiate(smokePrefab, target.gameObject.transform.position, target.gameObject.transform.rotation);
            //Instantiate(houseDebrisPrefab, target.gameObject.transform.position, target.gameObject.transform.rotation);
            Destroy(target.gameObject);
           


            //add to player's money & display how much the player earned
            HUD.pesos += 1000;
            HUD.timer += 300;
            HUD.timerBonusStr = "300";
            HUD.crushCount++;
            HUD.pointsOnDisplay = true;
            HUD.ptsDisplayString = HUD.housePoints;

            //set up ticker
            HUD.tickerXPos = Screen.width;
            HUD.tickerMsg = houseCrushText[num];
            HUD.tickerOnDisplay = true;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            //adjust the screen coordinates in case the camera's coordinates are not within screen range.
            if (screenPos.y < 0 || screenPos.y > Screen.height)
                screenPos.y = Screen.height / 2;
            if (screenPos.x > Screen.width || screenPos.x < 0)
                screenPos.x = Screen.width / 2;

            HUD.ptsDisplayRect = new Rect(screenPos.x, screenPos.y, 300, 80);
            HUD.timerBonusRect = new Rect(screenPos.x, screenPos.y + 50, 300, 80);
            //Debug.Log("hit hut");
        }

        if (target.GetComponent<Collider>().tag == "Goat")
        {
            AudioSource.PlayClipAtPoint(goatSound, target.gameObject.transform.position);

            //replace house with smoke particles + wooden planks. This is done by instantiating the smoke prefab.
            Instantiate(smokePrefab, target.gameObject.transform.position, target.gameObject.transform.rotation);
            //Instantiate(houseDebrisPrefab, target.gameObject.transform.position, target.gameObject.transform.rotation);
            Destroy(target.gameObject);




            //add to player's money & display how much the player earned
            HUD.pesos += 2000;
            HUD.timer += 500;
            HUD.timerBonusStr = "500";
            HUD.crushCount++;
            HUD.pointsOnDisplay = true;
            HUD.ptsDisplayString = HUD.goatPoints;

            //set up ticker
            HUD.tickerXPos = Screen.width;
            HUD.tickerMsg = goatCrushText[num];
            HUD.tickerOnDisplay = true;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

            //adjust the screen coordinates in case the camera's coordinates are not within screen range.
            if (screenPos.y < 0 || screenPos.y > Screen.height)
                screenPos.y = Screen.height / 2;
            if (screenPos.x > Screen.width || screenPos.x < 0)
                screenPos.x = Screen.width / 2;

            HUD.ptsDisplayRect = new Rect(screenPos.x, screenPos.y, 300, 80);
            HUD.timerBonusRect = new Rect(screenPos.x, screenPos.y + 50, 300, 80);
            //Debug.Log("hit hut");
        }

        /******Level Warps******/
        if (target.GetComponent<Collider>().tag == "WarpToHalfPipe")
        {
            //warp to new level
            AudioSource.PlayClipAtPoint(teleportSound, target.gameObject.transform.position);

            //save player data for next scene
            PlayerPrefs.SetInt("Timer", HUD.timer);
            PlayerPrefs.SetInt("Peso", HUD.pesos);

            Application.LoadLevel("halfpipeLevel");
            

        }

        if (target.GetComponent<Collider>().tag == "WarpToMaze")
        {
            //warp to new level
            AudioSource.PlayClipAtPoint(teleportSound, target.gameObject.transform.position);

            //save player data for next scene
            PlayerPrefs.SetInt("Timer", HUD.timer);
            PlayerPrefs.SetInt("Peso", HUD.pesos);

            Application.LoadLevel("mazeLevel");


        }

        if (target.GetComponent<Collider>().tag == "WarpToGoatLevel")
        {
            //warp to new level
            AudioSource.PlayClipAtPoint(teleportSound, target.gameObject.transform.position);

            //save player data for next scene
            PlayerPrefs.SetInt("Timer", HUD.timer);
            PlayerPrefs.SetInt("Peso", HUD.pesos);

            Application.LoadLevel("goatLevel");

       
        }
    }

}
