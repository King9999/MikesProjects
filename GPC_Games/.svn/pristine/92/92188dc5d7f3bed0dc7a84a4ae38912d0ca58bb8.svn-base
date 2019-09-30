using UnityEngine;
using System.Collections;

/* This is the player's standard weapon, the Herbivore. It is a subclass of Weapon. 
 * For purposes of the prototype challenge, this weapon has limited ammo so the player will eventually
 * have to reload.
 */

public class Herbivore : Weapon {

    float bulletVelocity;
    float bulletSpeed;
    
	// Use this for initialization
	void Start ()
    {
        name = "Herbivore";
        description = "Your standard weapon. Fires tiny bullets filled with herbicide, and shatter on impact.";
        //ammoCapacity = 100;
        //ammoCount = 100;
        power = 1;      //1 damage per shot
        level = 1;
        bulletVelocity = 0.4f;
        bulletSpeed = 100;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //this function will be constantly checking for bullets on screen and whether they collided with anything.
        float bulletMove = bulletSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, 1) * bulletMove);

        //check if bullet is off the field, and destroy it
        if (transform.position.z > 940) //940 is the z coordinate of the field's edge
        {
            //destroy bullet
            Destroy(this.gameObject);
        }


	
	}

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "RedFlower")
        {
            //we hit a flower; reduce its health. If it has no more health, then kill it.
            RedFlower enemy = target.gameObject.GetComponent<RedFlower>();  //this is used to grab the target's health
            enemy.hitPoints -= this.power;  //reduce target's health by bullet's power.
            if (enemy.hitPoints <= 0)
            {
                Destroy(target.gameObject);
            }
            //destroy the bullet no matter what happens
            Destroy(this.gameObject);

        }

        if (target.tag == "House")
        {
            //nothing happens to the house, but the bullet is destroyed
            Destroy(this.gameObject);
        }
    }

    void OnGUI()
    {
        //draws bullets on screen
    }


}
