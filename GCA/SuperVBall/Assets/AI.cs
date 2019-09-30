using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
	
		GameObject GameVolleyball;
		Volleyball volleyballScript;
	
		Vector3 target;
		Vector3 ballloc;
		float speed;
		float speedv;
		float positiony;
		int framecount;
		float sumtime;
	
	Vector3 myside(float x,float y,float z){
		positiony = Volleyball.pos.y;
		sumtime = 0;
		while(positiony > 180)
		{
			positiony += y * 0.033333f;
			sumtime += 0.033333f;
			y = y - 100 * 0.033333f;
			Debug.Log("lol");
		}
		Debug.Log(sumtime + "    " + x * sumtime);
		return new Vector3(x * sumtime + Volleyball.pos.x, 180,z * sumtime  + Volleyball.pos.z);
	}
	
	// Use this for initialization
	void Start () {
		GameVolleyball = GameObject.FindGameObjectWithTag("Volleyball");
		volleyballScript = GameVolleyball.GetComponent<Volleyball>();
		target = new Vector3(453,182,254);
		ballloc = new Vector3(0,0,0);
		speed = 3.0f;    //controls the player's movement speed
		framecount = 0;
		speedv = 0;
		positiony = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		framecount++;
		if(framecount > 60)
		{
			Debug.Log(volleyballScript.xvel + "  " + volleyballScript.yvel + "  " + volleyballScript.zvel);
			ballloc = myside(volleyballScript.xvel, volleyballScript.yvel, volleyballScript.zvel);
			Debug.Log(ballloc);
			framecount = 0;
		
		
			if(volleyballScript.xvel <= 0)
				target = new Vector3(453,182,254);
			else if(volleyballScript.xvel > 0 && 
		    		ballloc.x > 160 && ballloc.x < 550 && ballloc.z < 442 && ballloc.z > 85)
			{
				target = ballloc;
			}
			else
				target = new Vector3(275,182,254);
		}
		
		if (target.x < transform.position.x)
            transform.Translate(new Vector3(-1, 0, 0) * speed);
        if (target.x > transform.position.x)
            transform.Translate(new Vector3(1, 0, 0) * speed);
        if (target.z < transform.position.z)
            transform.Translate(new Vector3(0, 0, -1) * speed);
        if (target.z > transform.position.z)
            transform.Translate(new Vector3(0, 0, 1) * speed);	
	}
}