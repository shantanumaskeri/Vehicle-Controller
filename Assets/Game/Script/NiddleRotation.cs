using UnityEngine;
using System.Collections;

public class NiddleRotation : MonoBehaviour {

	// Use this for initialization
	RectTransform rectTransForm;
	void Start () {
		rectTransForm = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		rectTransForm.RotateAround (rectTransForm.position, rectTransForm.forward, 1.0f);
	}
}
