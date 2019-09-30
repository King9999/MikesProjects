using UnityEngine;
using System.Collections;

/*This code is used to manipulate the ball when it collides with a player */
public class Ballhandling : MonoBehaviour {
	
	GameObject GameVolleyball;
	//Volleyball volleyballScript;
	
	
    //void OnTriggerStay(Collider target)
    //{
    //    if(target.tag == "Volleyball")
    //    {
    //        volleyballScript.ballpos(new Vector3(
    //                  transform.position.x + Mathf.Cos(transform.rotation.y) * 25,
    //                  transform.position.y + 50,
    //                  transform.position.z + Mathf.Sin(transform.rotation.y) * 25));
			
    //        volleyballScript.ballvel(new Vector3(0,0,0));
    //    }
    //}
	
	// Use this for initialization
	void Start () 
	{
		GameVolleyball = GameObject.FindGameObjectWithTag("Player2");
		//volleyballScript = GameVolleyball.GetComponent<Volleyball>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Debug.Log(GameVolleyball.transform.position);
	
	}
}
