using UnityEngine;
using System.Collections;

/* This is the superclass for all enemies in the game.*/

public class Enemy : MonoBehaviour {

    protected string name;
    public int hitPoints;   //enemy life
    protected int maxHitPoints;
    public int cashReward;  //amount of money given to player when enemy is killed
    protected int timer;      //controls attacks.

    public GameObject PollenBulletPrefab;

    //lifebar
    protected float redBarWidth = 200;
    protected float greenBarWidth = 200;
    protected Rect emptyBar;           //shows amount of life missing
    protected Rect greenBar;          //shows amount of life remaining
    protected Rect lifebarBorder;       //border, purely cosmetic
    protected Rect redBar;             //indicates amount of damage taken. gradually decreases each frame when damaged
    protected Rect lifebarLabel;

    public Texture2D greenBarTexture;
    public Texture2D blackBarTexture;
    public Texture2D redBarTexture;

	// Use this for initialization
	void Start () 
    {
        //emptyBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, 200, 10);
        //greenBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, greenBarWidth, 10);
        //redBar = new Rect(Screen.width / 1.5f, Screen.height / 1.2f, redBarWidth, 10);

	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //float i;

        ////the following line ensures that the bar has the same width, no matter how long the time is. The fill rate will increase
        ////or decrease depending on the reload time.
        //i = (1 - (hitPoints / maxHitPoints)) * greenBarWidth;
        //greenBar.width -= i;

        ////decrease red health
        //redBar.width--;
        //if (redBar.width < greenBar.width)
        //    redBar.width = greenBar.width;

        //if (greenBar.width < 0)
        //    greenBar.width = 0;
	
	}

    void OnGUI()
    {
        //GUI.Label(new Rect(Screen.width / 1.5f, Screen.height / 1.2f, 50, 50), "Enemy HP: " + hitPoints.ToString() + "/" + maxHitPoints.ToString());
        //GUI.DrawTexture(emptyBar, blackBarTexture);
        //GUI.DrawTexture(redBar, redBarTexture);
        //GUI.DrawTexture(greenBar, greenBarTexture);
    }

    virtual public void Attack()
    {
        /* This function has different effects depending on the enemy */
    }

    void Spawn()
    {
        //if (spawnCount < MAX_SPAWNS)
        //{
        //    //creates an enemy and places it in a random location on the field.
        //    float xSpawnPoint = Random.Range(-367, -297);
        //    float zSpawnPoint = Random.Range(889, 940);

        //    //Instantiate(EnemySpawnPrefab, new Vector3(xSpawnPoint, 2, zSpawnPoint), Quaternion.identity);
        //    spawnCount++;
        //}
    }

    void Die()
    {
        //remove instance of this enemy
    }

}
