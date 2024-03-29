/* Hero.cs
 * This class is the character that the player controls.  The player can move and use their weapons. */



using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    //player variables
    string name;
    public int health = 100;         //player's current health
    int maxHealth;      //maximum health.  Can be increased over the course of the game.
    float moveSpeed;    //player's movement speed. Can be increased over the course of the game.
    int moveLevel;      //move speed level. max level is 3.
    const int MAX_MOVE_LEVEL = 3;
    int cash;           //cash on hand.  Used to upgrade Herbinator suit.
    const int MAX_CASH = 1000000;
    float velocity;        //player movement
    float vertVelocity;
    Weapon weapon;            //player's weapons. 0 = default, 1 = jet stream, 2 = spray, 3 = gas bomb
    int cooldown;    //number of frames that must pass before player can fire a shot.
    float ammoCount;
    float ammoCapacity;
    int reloadTime;

    //prefabs
    public GameObject HerbBulletPrefab;


	// Use this for initialization
	void Start () 
    {
        name = "King";
        maxHealth = 100;
        health = maxHealth;
        moveSpeed = 0.4f;
        moveLevel = 1;
        cash = 0;
        cooldown = 0;
        ammoCapacity = 50;
        ammoCount = ammoCapacity;
        //weapon = new Weapon();
        reloadTime = 0;
	}

    void FireWeapon()
    {
        //When the player clicks the left mouse button, the weapon is fired in the direction the mouse cursor is located.

    }

    void ChangeWeapon(int id)
    {
        /*Use a key to switch weapons */
    }

	// Update is called once per frame
	void Update () 
    {
        //At any time the player's health is 0, respawn the player at starting position.
        if (health <= 0)
        {
           
            Vector3 startPos = new Vector3(-331, 2, 883);
            transform.position = startPos;
            health = maxHealth;
            ammoCount = ammoCapacity;
        }
       
        //player position should always be updating.  Only the x and z coordinates will update because the player can move in 8 directions

        //update movement
        velocity = Input.GetAxis("Horizontal") * moveSpeed;
        vertVelocity = Input.GetAxis("Vertical") * moveSpeed;



        //boundary check
        Vector3 pos = transform.position;
        if (transform.position.x < -367)
        {
            pos.x = -367;
            transform.position = pos;
        }

        if (transform.position.x > -299)
        {
            pos.x = -299;
            transform.position = pos;
        }

        if (transform.position.z < 872)
        {
            pos.z = 872;
            transform.position = pos;
        }

        if (transform.position.z > 915)
        {
            pos.z = 915;
            transform.position = pos;
        }

        //translate using 3D vectors
        transform.Translate(Vector3.right * velocity);
        transform.Translate(new Vector3(0,0,1) * vertVelocity);
       

        //check for left mouse button click and fire bullet
        if (Input.GetMouseButton(0))
        {
            //fire equipped weapon.  Because this will allow continuous fire, there should be a little cooldown between shots.
            //bullets will fire every few frames.
            if (cooldown <= 0 && ammoCount > 0)
            {
                Instantiate(HerbBulletPrefab, transform.position, transform.rotation);
                cooldown = 12;  //set cooldown
                ammoCount--;
            }
            else
            {
                //keep counting down until cooldown ends
                cooldown--;
                if (cooldown < 0)
                    cooldown = 0;
             
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //any time the mouse button is not pressed, there should be no cooldown.
            cooldown = 0;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("Pressing down");
        }
	
	}

    
    void OnTriggerEnter(Collider target)
    {
               

    }

    void OnTriggerStay(Collider target)
    {
        if (target.tag == "Truck")
        {
            //reload player's weapon
            Debug.Log("Collided with truck");   //test message
            if (reloadTime <= 0)
            {
                SetReloadTime();
            }
            else
            {
                reloadTime--;
                if (reloadTime <= 0)
                {
                    //reload weapon and get some health back
                    ammoCount = ammoCapacity;
                    health += (int)(maxHealth * 0.3);
                    Debug.Log((int)maxHealth * 0.3);

                    if (health > maxHealth)
                        health = maxHealth;

                    reloadTime = 0;
                }
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, Screen.height / 2, 200, 50), "Ammo: " + ammoCount.ToString() + "/" + ammoCapacity.ToString());
        GUI.Label(new Rect(0, Screen.height / 2 + 50, 200, 50), "Health: " + health.ToString() + "/" + maxHealth.ToString());
        if (reloadTime > 0)
            GUI.Label(new Rect(0, Screen.height / 2 + 80, 200, 50), "Reload Time: " + reloadTime.ToString());

    }

    void SetReloadTime()
    {
        //reloads weapon and sets a time based on remaining ammo across all weapons.  This is not instantaneous, so the player must use with caution!
        float ammoPercentage = ammoCount / ammoCapacity;
        //Debug.Log("Ammo% " + ammoPercentage);

        //check the percentage of ammo remaining. There are 60 frames in a second so the time to reload should be represented in frames.
        if (ammoPercentage <= 0.1f)
            reloadTime = 300;
        else if (ammoPercentage <= 0.2f)
            reloadTime = 255;
        else if (ammoPercentage <= 0.3f)
            reloadTime = 220;
        else if (ammoPercentage <= 0.4f)
            reloadTime = 200;
        else if (ammoPercentage <= 0.5f)
            reloadTime = 180;
        else if (ammoPercentage <= 0.6f)
            reloadTime = 150;
        else if (ammoPercentage <= 0.7f)
            reloadTime = 120;
        else if (ammoPercentage <= 0.8f)
            reloadTime = 80;
        else if (ammoPercentage <= 0.9f)
            reloadTime = 30;
        else
            reloadTime = 0;

      
    }

}
