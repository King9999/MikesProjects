using UnityEngine;
using System.Collections;

/* Enemy bullets.  slow-moving but deadly! */

public class Pollen : MonoBehaviour 
{
    int power;
    float moveSpeed;

	// Use this for initialization
	void Start () 
    {
        power = 10;
        moveSpeed = 10;
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        float velocity = moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, -1) * velocity);

        if (transform.position.z < 870) //870 is the min z coordinate of the field's edge
        {
            //destroy bullet
            Destroy(this.gameObject);
        }
	
	}

    void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            //pollen struck the player
            Player player = target.gameObject.GetComponent<Player>();  //this is used to grab the target's health
            player.health -= this.power;  //reduce target's health by bullet's power.
            //if (player.health <= 0)
            //{
            //    //Kill player and respawn at starting position.
            //    //Destroy(target.gameObject);
            //    Vector3 pos = new Vector3(-331, 2, 883);
            //    //target.gameObject.
            //    Instantiate(target.gameObject, new Vector3(-331, 2, 883), Quaternion.identity);
            //    Destroy(player.gameObject);
            //}

            //destroy the bullet no matter what happens
            Destroy(this.gameObject);

        }

    //    if (target.tag == "House")
    //    {
    //        //nothing happens to the house, but the bullet is destroyed
    //        //Destroy(this.gameObject);
    //    }

    }

    public int GetPower()
    {
        return power;
    }  
}
