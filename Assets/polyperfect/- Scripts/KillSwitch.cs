using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSwitch : MonoBehaviour {

	Animator anim;

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha1))
		anim.SetBool ("isDead", true);
		
	}
}
