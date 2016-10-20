using UnityEngine;
using System.Collections;

public class GFScript : MonoBehaviour {
	private GameObject Player;
	private AudioSource mouth;
	private AudioClip mission;
	Actions actions;
	// Use this for initialization
	void Start () {
		actions = GetComponent<Actions> ();
		mouth = gameObject.GetComponent <AudioSource> ();
		mission = Resources.Load ("AayushiVoices/mission1_briefing",typeof(AudioClip)) as AudioClip;
	}
	
	// Update is called once per frame
	void Update () {
		Defend ();
	}

	void Move(){
	}
	void Defend(){
		Player = GameObject.FindWithTag("Player");
		transform.LookAt (Player.transform.position);
		Vector3 diff = Player.transform.position - transform.position;
		float distance = diff.sqrMagnitude;
		/*if (distance < 50 && !mouth.isPlaying) {
			mouth.PlayOneShot (mission);
		}*/
	}
}
