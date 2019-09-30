using UnityEngine;
using System.Collections;

/* Red Flowers are basic enemies.  They shoot pollen in one direction. They will spawn in random locations on the map.*/
public class RedFlower : Enemy 
{
    //public GameObject PollenBulletPrefab;
	// Use this for initialization
	void Start () 
    {
        name = "Red Flower";
        maxHitPoints = 3;
        hitPoints = maxHitPoints;
        cashReward = 1;

        emptyBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, 200, 10);
        greenBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, greenBarWidth, 10);
        redBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, redBarWidth, 10);

        //fire a shot upon spawning
        Instantiate(PollenBulletPrefab, transform.position, Quaternion.identity);
        timer = Random.Range(180, 300);
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer--;
        if (timer <= 0)
        {
            //fire a shot
            Instantiate(PollenBulletPrefab, transform.position, Quaternion.identity);
            timer = Random.Range(180, 300);
        }

        /* For some reason the lifebar code doesn't work as intended even though the code is correct. */

        float i = (hitPoints / maxHitPoints) * greenBarWidth;

        //the following line ensures that the bar has the same width, no matter how much health the enemy has. 
        greenBar.width = i;
        
        //if (greenBar.width < 0)
        //    greenBar.width = 0;

        //decrease red health
        redBar.width--;
        if (redBar.width < greenBar.width)
            redBar.width = greenBar.width;
	
	}

    void OnGUI()
    {
        //Show enemy HP
        //GUI.Label(new Rect(Screen.width / 1.5f, Screen.height / 1.2f, 50, 50), "Enemy HP: " + hitPoints.ToString() + "/" + maxHitPoints.ToString());

        //GUI.Label(new Rect(Screen.width / 1.5f, Screen.height / 1.1f, 50, 50), "i: " + greenBar.width.ToString());
        //GUI.DrawTexture(emptyBar, blackBarTexture);
        //GUI.DrawTexture(redBar, redBarTexture);
        //GUI.DrawTexture(greenBar, greenBarTexture);
    }
}
