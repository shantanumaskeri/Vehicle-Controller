using UnityEngine;
using System.Collections;

public class RotateArroundCustom : MonoBehaviour {
	[SerializeField]
	Transform targetTransform;

	[SerializeField]
	float speed = 1.0f;
	// Use this for initialization
	void Start () {
//		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("Hiiii in Rotate Arround Function");
		transform.RotateAround (targetTransform.position, transform.up, speed);
	}
}
