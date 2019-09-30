using UnityEngine;
using System.Collections;

/* Adjusts the score on the 3D scoreboard */

public class Score : MonoBehaviour
{
	public AudioSource audioSource;
	public AudioClip applause;
	public int playerScore;
    public TextMesh scoreBoard;
	
	public void AddPoint()
	{
        playerScore++;
		audioSource.Play();
        Debug.Log("Add point");
        GetComponent<TextMesh>().text = playerScore.ToString();
	}
	
	public void PlayerScoreReset()
	{
        playerScore = 0;
        GetComponent<TextMesh>().text = playerScore.ToString();
	}
	
	// Use this for initialization
	void Start () 
    {
        playerScore = 0;
        scoreBoard = new TextMesh();
		audioSource = (AudioSource) gameObject.AddComponent("AudioSource");
		applause = (AudioClip)Resources.Load("applause");
		audioSource.clip = applause;
	}
	
	// Update is called once per frame
	void Update () 
    {
       
	
	}
}
