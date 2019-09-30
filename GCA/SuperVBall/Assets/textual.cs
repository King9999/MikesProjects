using UnityEngine;
using System.Collections;

public class textual : MonoBehaviour {
	public int score = 0;
	
	public void scoreup()
	{
		score++;
		GetComponent<TextMesh>().text = score.ToString();
	}
	
	public void scorereset()
	{
		score = 0;
		GetComponent<TextMesh>().text = score.ToString();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
