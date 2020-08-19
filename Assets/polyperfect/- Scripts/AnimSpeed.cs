using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSpeed : MonoBehaviour {

	Animator anim;

	float Speed;

	// Use this for initialization
	void Start () {
		Speed = Random.Range (0.5f, 1.5f);
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		anim.speed = Speed;	
	}
}
