using UnityEngine;
using System.Collections;

public class InGameAudioManager : MonoBehaviour {

	public static InGameAudioManager instance;

	public AudioClip BGSound;
	public AudioClip accelerationSound;
	public AudioClip breakSound;
	public AudioClip reverseSound;
	public AudioClip carHitSound;
	public AudioClip waterSplashSound;

	public AudioSource[] audioSourceArray;
	// Use this for initialization
	void Start () {
		instance = this;
		audioSourceArray = GetComponentsInChildren<AudioSource> ();

		PlayBgSound ();
	}
	
	public void PlayBgSound(){
		audioSourceArray [0].clip = BGSound;
		audioSourceArray [0].loop = true;
		audioSourceArray [0].Play ();
	}

	public void PlayAccelerationSound(){
		if (audioSourceArray [1].isPlaying && !audioSourceArray [1].clip.Equals (accelerationSound)) {
			audioSourceArray [1].Stop ();
		}

		if(!audioSourceArray [1].isPlaying){
			audioSourceArray [1].clip = accelerationSound;
			audioSourceArray [1].loop = true;
			audioSourceArray [1].Play ();
		} else {

		}
	}

	public void PlayReverseSound(){
		if (audioSourceArray [1].isPlaying && !audioSourceArray [1].clip.Equals (reverseSound)) {
			audioSourceArray [1].Stop ();
		}
		
		if(!audioSourceArray [1].isPlaying){
			audioSourceArray [1].clip = reverseSound;
			audioSourceArray [1].loop = true;
			audioSourceArray [1].Play ();
		} else {
			
		}
	}

	public void PlayBreakSound(){


		audioSourceArray [1].loop = false;
		if (audioSourceArray [1].isPlaying)
			audioSourceArray [1].Stop ();
		
		audioSourceArray [1].clip = breakSound;
		audioSourceArray [1].Play ();
	}

	public void PlayCarHitSound(){
		
		
		audioSourceArray [2].loop = false;
		if (audioSourceArray [2].isPlaying)
			audioSourceArray [2].Stop ();
		
		audioSourceArray [2].clip = carHitSound;
		audioSourceArray [2].Play ();
	}

	public void PlayWaterSplahSound(){
		audioSourceArray [3].loop = false;

		if (!audioSourceArray [3].isPlaying) {
			audioSourceArray [3].clip = waterSplashSound;
			audioSourceArray [3].Play ();
		}
	}
}
