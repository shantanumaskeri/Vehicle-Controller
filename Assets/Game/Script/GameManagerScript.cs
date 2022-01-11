using UnityEngine;
using System.Collections;

public class GameManagerScript : MonoBehaviour {

	[SerializeField]
	GameObject cameraObject;

	[SerializeField]
	Transform startPosTrans;

	[SerializeField]
	Transform stopPosTrans;

	public enum GAME_STATE {
		STATE_ROTATE_CAMERA,
		START_GAME
	};

	public GAME_STATE currState = GAME_STATE.STATE_ROTATE_CAMERA;
	// Use this for initialization
	void Start () {
		SwitchState (GAME_STATE.STATE_ROTATE_CAMERA);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwitchState(GAME_STATE targetState){

		currState = targetState;

		switch(targetState){
		case GAME_STATE.STATE_ROTATE_CAMERA:
			cameraObject.transform.position = stopPosTrans.position;
			cameraObject.transform.rotation = stopPosTrans.rotation;
			cameraObject.gameObject.GetComponent<RotateArroundCustom>().enabled = true;
			Invoke("StartGame",8.0f);
			break;
		case GAME_STATE.START_GAME:
			cameraObject.gameObject.GetComponent<RotateArroundCustom>().enabled = false;
			cameraObject.transform.position = startPosTrans.position;
			cameraObject.transform.rotation = startPosTrans.rotation;

			break;
		}
	}

	public void StartGame()
	{
		SwitchState (GAME_STATE.START_GAME);
		Debug.Log("Start Game");
	}
}
