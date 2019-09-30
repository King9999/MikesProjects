/* Player lifebar.  Uses multiple rectangles to make an attractive-looking lifebar. */

using UnityEngine;
using System.Collections;

public class Lifebar : MonoBehaviour {

    float redBarWidth = 200;
    float greenBarWidth = 200;
    Rect emptyBar;           //shows amount of life missing
    Rect greenBar;          //shows amount of life remaining
    Rect lifebarBorder;       //border, purely cosmetic
    Rect redBar;             //indicates amount of damage taken. gradually decreases each frame when damaged
    Rect lifebarLabel;

    public Texture2D greenBarTexture;      
    public Texture2D blackBarTexture;      
    public Texture2D redBarTexture;

	// Use this for initialization
	void Start ()
    {
        emptyBar = new Rect(50, 50, 200, 20);
        greenBar = new Rect(50, 50, greenBarWidth, 20);
        lifebarBorder = new Rect(50, 50, 200, 40);
        redBar = new Rect(50,50, redBarWidth, 20);

        lifebarLabel = new Rect(40, 20, 30, 30);

	}

    void DecreaseHealth()
    {
        //health goes down whenever a key is pressed.  If another key is pressed, health should increase.
        //if (KeyCode
    }
	
	// Update is called once per frame
	void Update ()
    {

        //greenBar.width -= 2;
        //if (greenBar.width < 0)
        //    greenBar.width = 0;

        //redBar.width--;
        //if (redBar.width < greenBar.width)
        //    redBar.width = greenBar.width;
	
	}

    void OnGUI()
    {
        //display the bars in this order: black, red, green
        GUI.Label(lifebarLabel, "LIFE");
        GUI.DrawTexture(emptyBar, blackBarTexture);
        GUI.DrawTexture(redBar, redBarTexture);
        GUI.DrawTexture(greenBar, greenBarTexture);
    }
}
