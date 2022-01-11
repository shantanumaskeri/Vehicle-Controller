using UnityEngine;
using System.Collections;

public class TweenPos : MonoBehaviour {
	// Use this for initialization

	public Vector3 FromPosition;
	public Vector3 ToPosition;

	public enum ANIM_TYPE{ONCE = 0,TWICE = 1,LOOP = 2,PING_PONG = 3};
	public ANIM_TYPE AnimatonType;

	public float StartDelay = 0.0f;
	public float Duration = 1.0f;
	public AnimationCurve AnimationData;

	public enum CONSTRAINT_ONLY{
		NONE,
		X_ONLY,
		Y_ONLY,
		Z_ONLY
	};
	
	public CONSTRAINT_ONLY tweenConstraint = CONSTRAINT_ONLY.NONE;

	public bool PlayAutomatically = true;

	public delegate void EventUpdate();
	public EventUpdate OnAnimationFinished;

	//Private data
	private float TimerCnt = 0.0f;
	private float StartDelayCnt = 0.0f;


	private bool isReverse = false;
	private bool play = false;



	void Awake(){


	}

	void OnEnable(){
		if (PlayAutomatically) {
			ResetToOriginalValues ();
			Play ();
		}
	}

	void Start () {
		if (PlayAutomatically)
			Play ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!play)
			return;

		if (StartDelayCnt < StartDelay) {
			StartDelayCnt += Time.smoothDeltaTime;
			return;
		}

		//Play Tweening according animation curve;


		if(!isReverse)
			TimerCnt += (Time.smoothDeltaTime/Duration);
		else
			TimerCnt -= (Time.smoothDeltaTime/Duration);


		switch (AnimatonType) {
		case ANIM_TYPE.ONCE:
			if((!isReverse && TimerCnt >= 1.0f) || (isReverse && TimerCnt <= 0.0f)){
				if(OnAnimationFinished != null)
					OnAnimationFinished();
				this.enabled = false;
			}
			break;
		case ANIM_TYPE.LOOP:
			if(TimerCnt >= 1.0f)
				TimerCnt = Time.smoothDeltaTime;
			break;
		case ANIM_TYPE.PING_PONG:
			if((!isReverse && TimerCnt >= 1.0f) || (isReverse && TimerCnt <= 0.0f)){
//				TimerCnt = Time.smoothDeltaTime;
				isReverse = !isReverse;

//				Vector3 temp = Vector3.zero;
//				temp = FromPosition;
//				FromPosition = ToPosition;
//				ToPosition = temp;
				
			}

//			Debug.Log("TimerCnt"+TimerCnt);
			break;
		}

		Vector3 targetPos = Vector3.zero;
//		if(!isReverse)
//			targetPos = FromPosition + (ToPosition - FromPosition) * AnimationData.Evaluate (TimerCnt);
//		else
//			targetPos = FromPosition + ((ToPosition - FromPosition) - (ToPosition - FromPosition) * AnimationData.Evaluate (TimerCnt));

		if(!isReverse)
			targetPos = FromPosition + (ToPosition - FromPosition) * AnimationData.Evaluate (TimerCnt);
		else
			targetPos = ToPosition + ((FromPosition - ToPosition) - (FromPosition - ToPosition) * AnimationData.Evaluate (TimerCnt));

//		if(!isReverse)
//			targetPos = FromPosition + (ToPosition - FromPosition) * AnimationData.Evaluate (TimerCnt);
//		else
//			targetPos = FromPosition + ((ToPosition - FromPosition) - (ToPosition - FromPosition) * AnimationData.Evaluate (TimerCnt));



		switch (tweenConstraint) {
		case CONSTRAINT_ONLY.NONE:
			transform.localPosition = targetPos;
			break;
		case CONSTRAINT_ONLY.X_ONLY:
			transform.localPosition = new Vector3(targetPos.x,transform.localPosition.y,transform.localPosition.z);
			break;
		case CONSTRAINT_ONLY.Y_ONLY:
			transform.localPosition = new Vector3(transform.localPosition.x,targetPos.y,transform.localPosition.z);
			break;
		case CONSTRAINT_ONLY.Z_ONLY:
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,targetPos.z);
			break;
		}




	}

	public void Play(){
		ResetToOriginalValues ();
		play = true;
	}

	public void PlayReverse (){
		play = true;
		isReverse = true;
		TimerCnt = 1.0f;
		StartDelayCnt = 0;
		this.enabled = true;
	}

	public void ResetToOriginalValues(){

		switch (tweenConstraint) {
		case CONSTRAINT_ONLY.NONE:
			transform.localPosition = FromPosition;
			break;
		case CONSTRAINT_ONLY.X_ONLY:
			transform.localPosition = new Vector3(FromPosition.x,transform.localPosition.y,transform.localPosition.z);
			break;
		case CONSTRAINT_ONLY.Y_ONLY:
			transform.localPosition = new Vector3(transform.localPosition.x,FromPosition.y,transform.localPosition.z);
			break;
		case CONSTRAINT_ONLY.Z_ONLY:
			transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y,FromPosition.z);
			break;
		}

		play = false;
		TimerCnt = 0;
		StartDelayCnt = 0;
		isReverse = false;
		this.enabled = true;
	}


}
