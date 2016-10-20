using UnityEngine;
using System.Collections;

public class BulletLife : MonoBehaviour {
	float timeLeft;
	// Use this for initialization
	void Start () {
		timeLeft = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if(timeLeft < 0f)
			Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision) {
		Destroy (gameObject);
	}
}
