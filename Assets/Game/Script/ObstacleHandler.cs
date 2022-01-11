using UnityEngine;
using System.Collections;

public class ObstacleHandler : MonoBehaviour {
	[SerializeField]
	ParticleSystem targetParticle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EmmitParticle(){
		targetParticle.enableEmission = true;
		targetParticle.Emit (1000);
	}
}
