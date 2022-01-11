using UnityEngine;
using System.Collections;

public class SteeringSimulation : MonoBehaviour {

	// Use this for initialization
	Vector3 eulerAngle = Vector3.zero;
	float offSetZ;
	void Start () {
		eulerAngle = transform.localEulerAngles;
		offSetZ = eulerAngle.z;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxisRaw("Vertical")> 0.01|| Input.GetAxisRaw("Vertical") < -0.01)
		{
			eulerAngle.x = transform.localEulerAngles.x;
			eulerAngle.z = (offSetZ + 360.0f * Input.GetAxisRaw("Vertical"))*-1.0f;
			eulerAngle.y = transform.localEulerAngles.y;
		}
		transform.localEulerAngles = eulerAngle;
	}
}
