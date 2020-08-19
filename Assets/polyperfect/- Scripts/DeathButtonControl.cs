using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathButtonControl : MonoBehaviour {
	public int countOfAnimations;
	public Button button;
	// Use this for initialization
	void Start () {

		for (int i = 0; i<countOfAnimations; i++) {
			float height = button.GetComponent<RectTransform> ().sizeDelta.y;
			Vector2 newPosition = new Vector2 (0, 115 + ( height * i));
				
			Button newButton = Object.Instantiate(button, newPosition, Quaternion.identity) as Button;
			RectTransform newButtonTransform = newButton.GetComponent<RectTransform> ();
			newButton.transform.SetParent(this.transform);
			newButtonTransform.anchoredPosition = newPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
