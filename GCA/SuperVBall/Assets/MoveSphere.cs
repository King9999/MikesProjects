using UnityEngine;
using System.Collections;

public class MoveSphere : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	transform.position = new Vector3(0,160,0);
	}
	
	// Update is called once per frame
	void Update () {
	transform.position = new Vector3(Volleyball.pos.x,160,Volleyball.pos.z);
	float mod = 400/(Volleyball.pos.y - 160);
	transform.localScale = new Vector3(mod,0.09617879f, mod);
	}
}
