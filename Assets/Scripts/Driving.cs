using UnityEngine;
using System.Collections;

public class Driving : MonoBehaviour {
	public bool drivable;
	public Light leftTail,rightTail;
	private float Speed, Turn, TopSpeedFwd, TopSpeedBk;
	private Rigidbody rb;
	private float acceleration, velocity, deccelerationLimit;
	public GameObject Passenger1,Passenger2;

	// Use this for initialization
	void Start () {
		drivable = false;
		rb = GetComponent<Rigidbody> ();
		acceleration = 0.0f;
		velocity = 0.0f;
		TopSpeedFwd = 2.0f;
		TopSpeedBk = -0.5f;
		deccelerationLimit = -0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		TailLights ();
		if (drivable) {
			Speed = Input.GetAxis ("Vertical") * 5.0f;
			Turn = Input.GetAxis ("Horizontal") * 2.0f;
			if (Input.GetKeyDown ("f")) {
				exitRide ();
			}
		} else {
			Speed = 0.0f;
		}
		acceleration = Speed;
		if (acceleration < deccelerationLimit)
			acceleration = deccelerationLimit;
		velocity += acceleration * Time.deltaTime;
		if (velocity > TopSpeedFwd)
			velocity = TopSpeedFwd;
		if (velocity < TopSpeedBk)
			velocity = TopSpeedBk;
		transform.Translate (Vector3.forward * velocity * 30 * Time.deltaTime);
		velocity = 0.99f * velocity;
		//rb.AddForce(transform.forward * Speed * 500000);
		if(Mathf.Abs(velocity)>0.1)
			transform.RotateAround(transform.position, transform.up, Time.deltaTime * 50f * Turn * velocity/Mathf.Abs(velocity));


	}

	void TailLights(){
		if (acceleration >= 0) {
			leftTail.intensity = 0.0f;
			rightTail.intensity = 0.0f;
		}
		if (acceleration < 0) {
			leftTail.intensity = 1.0f;
			rightTail.intensity = 1.0f;
		}
	}

	void exitRide(){
		GameObject go;
		go = GameObject.FindGameObjectWithTag("Player");
		Destroy (go);
		GameObject clone = (GameObject)Instantiate(Passenger1, transform.position, transform.rotation);
		drivable = false;
	}
}
