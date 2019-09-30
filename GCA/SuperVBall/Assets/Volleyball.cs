using UnityEngine;
using System.Collections;

public class Volleyball : MonoBehaviour {
	
	public float xvel = 0, yvel = 0, zvel = 0, gravity = 100;
    public bool impacty = false, impactz = false;
	public static Vector3 pos;

   
    //Objects needed to manipulate the scoreboard
    GameObject P1GameScore, P2GameScore;
    Score p1ScoreScript, p2ScoreScript;
    bool pointCheck;
    
	public void ballvel(Vector3 vel)
	{
		xvel = vel.x; 
		yvel = vel.y;
		zvel = vel.z;
	}
	
	public float ballvelup(char dimension)
	{ 
		switch(dimension)
		{
		case 'x': return xvel;
		case 'y': return yvel;
		case 'z': return zvel;
		default: return 0;
		}	
	}
	
	public void ballvelmod(char dimension,float x)
	{
		switch(dimension)
		{
		case 'x': 
		xvel = x;
		break;
		case 'y': 
		yvel = x;
		break;
		case 'z': 
		zvel = x;
		break;
		default: break;
		}
	}
	
	public void ballpos(Vector3 pos)
	{
		transform.position = pos;
	}
	
	public void ballposup(Vector3 pos)
	{
	float x = transform.position.x, y = transform.position.y,z = transform.position.z;
	transform.position = new Vector3(x + pos.x, y + pos.y, z + pos.z);
	}
	
	public void updateballpos()
	{
		ballposup(new Vector3(xvel * Time.deltaTime, yvel * Time.deltaTime, zvel * Time.deltaTime));

        if (transform.position.y > 175)
        {
            yvel -= gravity * Time.deltaTime;
            impacty = false;
        }
        else if (transform.position.y <= 185 && impacty == false)
        {
            yvel = -yvel;
            impacty = true;
        }

        if (transform.position.z < 50 || transform.position.z > 470)
        {
            zvel = -zvel;
            impactz = true;
        }
        else {
            impactz = false;
		}
		
		pos.x = transform.position.x;
		pos.y = transform.position.y;
		pos.z = transform.position.z;
	}
	
	
	// Use this for initialization
	void Start () {
		//ballpos(new Vector3(0,0,0));
		ballvel(new Vector3(0,0,0));
        P1GameScore = GameObject.FindGameObjectWithTag("Score");
        P2GameScore = GameObject.FindGameObjectWithTag("Score2");
        p1ScoreScript = P1GameScore.GetComponent<Score>();
        p2ScoreScript = P2GameScore.GetComponent<Score>();
        pointCheck = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		
		updateballpos();
        if (transform.position.y > 187)
            pointCheck = false;

        //the ball is the on the ground.  Check which side it's on
        if (transform.position.y <= 187 && transform.position.x <= 164 && !pointCheck)
        {
            //add a point on player 2's side
            p2ScoreScript.AddPoint();
            pointCheck = true;			
            Debug.Log("Check1");
            
        }
        else if (transform.position.y <= 187 && transform.position.x > 164 && !pointCheck)
        {
            //add a point on player 1's side
            p1ScoreScript.AddPoint();
            pointCheck = true;
            Debug.Log("Check2");
        }
	}
}