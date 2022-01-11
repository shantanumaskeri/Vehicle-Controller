using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerCarHandler : MonoBehaviour {
	public List<AxleInfo> axleInfos; // the information about each individual axle

	public Transform centerOfMass;

	public RectTransform Needle;
	public int Score = 0;

	public Text scoreText;
	//Car Properties
	public int Min_Turn = 6;
	public int Max_Turn = 8;
	public float CurrentSpeed = 0;
	public float SpeedMax = 200;
	public float SpeedMax_Reverse = 30;
	public float BoostSpeed = 300;

	public float Engine_Torque = 100;
	public float MaxEngineRPM  = 3000.0f;
	public float MinEngineRPM  = 1000.0f;
	
	public float []GearRatio;
	public int CurrentGear = 0;
	
	public int BrakingTorque = 1000;
	public float ConstantForceValue = 0.0f;

	private WheelFrictionCurve wfc;
	public float resetTime = 5.0f;
	private float resetTimer = 0.0f;
	private float EngineRPM = 0.0f;

	private float V_Aceleration = 0.0f;
	private float H_Aceleration = 0.0f;

	private bool BrakeFlag;

	private Rigidbody rigidBody;

	private TweenPos tweenPos;
	private Vector3 needleTargetRotation = Vector3.zero;



	// Use this for initialization
	void Start () {
		PlayerPrefs.SetInt ("Score",Score);
		GetComponent<Rigidbody> ().centerOfMass = centerOfMass.localPosition;

		tweenPos = GetComponentInChildren<TweenPos> ();

		rigidBody = GetComponent<Rigidbody> ();

		if (Needle != null)
			needleTargetRotation = Needle.localEulerAngles;

		wfc = new WheelFrictionCurve();
		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				wfc.asymptoteSlip = axleInfo.leftWheel.forwardFriction.asymptoteSlip;
				wfc.asymptoteValue = axleInfo.leftWheel.forwardFriction.asymptoteValue;
				wfc.extremumSlip = axleInfo.leftWheel.forwardFriction.extremumSlip;
				wfc.extremumValue = axleInfo.leftWheel.forwardFriction.extremumValue;
			}
		}

	}


	public void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) 
		{
			PlayerPrefs.SetInt ("Result",0);
			Application.LoadLevel ("ResultScene");
		}

		if (Needle != null) {
//			Needle.localEulerAngles = new Vector3(Needle.localEulerAngles.x,Needle.localEulerAngles.y,Mathf.Clamp(-90 + 180/SpeedMax * Mathf.Abs(CurrentSpeed),-90,-90+180));

			needleTargetRotation.z =360 - ((262) * Mathf.Clamp(Mathf.Abs(CurrentSpeed),0,SpeedMax)/BoostSpeed);
			Needle.localEulerAngles = Vector3.Lerp(Needle.localEulerAngles,needleTargetRotation,Time.deltaTime*2.0f);
		}

		Check_If_Car_Is_Flipped();
	}


	public void UpdateWheelFriction(AxleInfo axleInfo)
	{
		return;

		if(BrakeFlag)
		{
			wfc.stiffness =  0.01f;
			axleInfo.leftWheel.forwardFriction = wfc;
			axleInfo.rightWheel.forwardFriction = wfc;

			wfc.stiffness =  0.01f;
			axleInfo.leftWheel.sidewaysFriction = wfc;
			axleInfo.rightWheel.sidewaysFriction = wfc;

		}
		else
		{
//			wfc.stiffness = SpeedMax/(SpeedMax+CurrentSpeed) * 2.0f;
			wfc.stiffness =  2.0f;
			axleInfo.leftWheel.forwardFriction = wfc;
			axleInfo.rightWheel.forwardFriction = wfc;

//			wfc.stiffness = SpeedMax/(SpeedMax+CurrentSpeed) * 6.0f;
			wfc.stiffness =  2.0f;
			axleInfo.leftWheel.sidewaysFriction = wfc;
			axleInfo.rightWheel.sidewaysFriction = wfc;
		}
	}


	// Update is called once per frame
	public void FixedUpdate(){
		HandleKeyEvents ();

		foreach (AxleInfo axleInfo in axleInfos) {

			UpdateWheelFriction(axleInfo);

			if (axleInfo.steering) {
				ApplySteer(axleInfo);
			}
			if (axleInfo.motor) {
				ApplyTorque(axleInfo);
				CalculateSpeed(axleInfo);
				CalculateEngineRpm(axleInfo);
//				ShiftGears(axleInfo);
			}

			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
		}
	
	}

	public void ApplySteer(AxleInfo axleinfo){

			if (CurrentSpeed < 20.0f) {
				axleinfo.leftWheel.steerAngle = Max_Turn * H_Aceleration;
//				axleinfo.leftWheel.steerAngle = SpeedMax/(SpeedMax+CurrentSpeed) * (Max_Turn * H_Aceleration);
				axleinfo.rightWheel.steerAngle = Max_Turn * H_Aceleration;	
			}
			else 
			{
				axleinfo.leftWheel.steerAngle = Min_Turn * H_Aceleration;
				axleinfo.rightWheel.steerAngle = Min_Turn * H_Aceleration;	
			}
	}
	public bool breakSoundPlay = false;
	public void ApplyTorque(AxleInfo axleinfo){
	
		rigidBody.AddForce(Vector3.up*-1*ConstantForceValue);
//		if(Input.GetButton("joystick button 0")){
//			rigidBody.drag = 2.0f;
//			return;
//		}

		PlaySound();
		if (CurrentSpeed < SpeedMax) {

//			if(V_Aceleration < 0){
//				InGameAudioManager.instance.PlayReverseSound();
////				rigidBody.drag = 1.0f;
//				breakSoundPlay = false;
//			}
//
////			Debug.Log("V_AccelVAlue:"+V_Aceleration);
//
//			if(V_Aceleration == 0){
////				Debug.Log("break:"+V_Aceleration +"pre_value:"+preV_value);
//
////				rigidBody.drag = 5.0f;
//				if(!breakSoundPlay){
//					InGameAudioManager.instance.PlayBreakSound();
//					breakSoundPlay = true;
//				}
//			}
//			else if(V_Aceleration > 0){
////				rigidBody.drag = 0.05f;
//				InGameAudioManager.instance.PlayAccelerationSound();
//				breakSoundPlay = false;
//			}



			rigidBody.drag = 1.0f * (1 - V_Aceleration);
			if(V_Aceleration != 0){
				axleinfo.leftWheel.motorTorque = V_Aceleration * Engine_Torque;
				axleinfo.rightWheel.motorTorque = V_Aceleration * Engine_Torque;
				axleinfo.leftWheel.brakeTorque = 0;
				axleinfo.rightWheel.brakeTorque = 0;

			}
			else{
				axleinfo.leftWheel.motorTorque = 0;
				axleinfo.rightWheel.motorTorque = 0;

				axleinfo.leftWheel.brakeTorque = BrakingTorque;
				axleinfo.rightWheel.brakeTorque = BrakingTorque;
			}
		} else {

			axleinfo.leftWheel.motorTorque = 0;
			axleinfo.rightWheel.motorTorque = 0;
			rigidBody.drag = 1.0f;
//			rigidBody.drag = 5.0f;
		}

		
	}

	void Check_If_Car_Is_Flipped()
	{
		if(transform.localEulerAngles.z > 60 && transform.localEulerAngles.z < 300)
			resetTimer += Time.deltaTime;
		else
			resetTimer = 0;
		
		if(resetTimer > resetTime)
			FlipCar();
	}
	
	void FlipCar()
	{	
		
		transform.rotation = Quaternion.LookRotation(transform.forward);
		transform.position += Vector3.up * 0.5f;
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		
		
		resetTimer = 0;
		//currentEnginePower = 0;
	}

	public void CalculateSpeed(AxleInfo axleInfo){
		CurrentSpeed = (float)(2*(22/7)*axleInfo.leftWheel.radius*axleInfo.leftWheel.rpm/1.4 * 60/1000);
//		CurrentSpeed = (int)Mathf.Abs((180.0f*rigidBody.velocity.z)/50.0f);
//		CurrentSpeed = (int)(rigidBody.velocity.magnitude * 60.0f/1000.0f);
//		CurrentSpeed = (float)((double)rigidBody.velocity.magnitude * 2.237);
	}

	public void CalculateEngineRpm(AxleInfo axleInfo){
		EngineRPM = (axleInfo.leftWheel.rpm + axleInfo.rightWheel.rpm)/2 * GearRatio[CurrentGear];
	}

	
	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}
		
		Transform visualWheel = collider.transform.GetChild(0);
		
		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);
		
		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation;
	}

	public void PlaySound(){
		if(V_Aceleration < 0){
			InGameAudioManager.instance.PlayReverseSound();
			//				rigidBody.drag = 1.0f;
			breakSoundPlay = false;
		}
		
		//			Debug.Log("V_AccelVAlue:"+V_Aceleration);
		
		if(V_Aceleration == 0){
			//				Debug.Log("break:"+V_Aceleration +"pre_value:"+preV_value);
			
			//				rigidBody.drag = 5.0f;
			if(!breakSoundPlay){
				InGameAudioManager.instance.PlayBreakSound();
				breakSoundPlay = true;
			}
		}
		else if(V_Aceleration > 0){
			//				rigidBody.drag = 0.05f;
			InGameAudioManager.instance.PlayAccelerationSound();
			breakSoundPlay = false;
		}
	}

	public void HandleKeyEvents()
	{
		V_Aceleration = 0.0f;
		H_Aceleration = 0.0f;
	
		#if UNITY_EDITOR
		H_Aceleration = Input.GetAxis ("Horizontal");
		V_Aceleration = Input.GetAxis ("Vertical");
		#endif

		BrakeFlag = false;
		if (false) {
			BrakeFlag = true;
		}


//		if(Input.GetKey(KeyCode.Space)){
//		
//		}
	}

	void ShiftGears(AxleInfo axleInfo) {
		if (EngineRPM >= MaxEngineRPM) {
			int AppropriateGear  = CurrentGear;
			
			for (int i = 0; i < GearRatio.Length; i ++ ) {
				if ( axleInfo.leftWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			CurrentGear = AppropriateGear;
		}
		
		if (EngineRPM <= MinEngineRPM) {
			int AppropriateGear = CurrentGear;
			
			for (int j = GearRatio.Length-1; j >= 0; j -- ) {
				if ( axleInfo.leftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			CurrentGear = AppropriateGear;
		}
	}

	[System.Serializable]
	public class AxleInfo {
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor; // is this wheel attached to motor?
		public bool steering; // does this wheel apply steer angle?

	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "GameEndTrigger") {
			PlayerPrefs.SetInt ("Result", 1);
			Application.LoadLevel ("ResultScene");
		}
		else if(other.tag == "obstacle")
		{
			Score +=20;
			PlayerPrefs.SetInt("Score",Score);
			other.gameObject.GetComponent<Renderer>().enabled = false;
			other.gameObject.GetComponent<Collider>().enabled = false;
			other.gameObject.GetComponent<ObstacleHandler>().EmmitParticle();
			tweenPos.ResetToOriginalValues();
			tweenPos.Play();
			InGameAudioManager.instance.PlayCarHitSound();
			Debug.Log("Collided"+other.name+"Your score:--"+Score);
		}
		else if(other.tag == "boost")
		{
			Score +=10;
			if(scoreText != null)
				scoreText.text = Score.ToString();
			PlayerPrefs.SetInt("Score",Score);
			InGameAudioManager.instance.PlayCarHitSound();
			SpeedMax = BoostSpeed;
			CancelInvoke("ResetMaxSpeed");
			Invoke("ResetMaxSpeed",5);
			other.gameObject.SetActive(false);
			Debug.Log("Collided"+other.name+"Your score:--"+Score);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (other.tag == "water") {
			InGameAudioManager.instance.PlayWaterSplahSound();
		}
	}

	public void ResetMaxSpeed(){
		SpeedMax = 100;
	}



}
