using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Text scoreValue;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		scoreValue.text = PlayerPrefs.GetInt ("Score").ToString();
	}
}
