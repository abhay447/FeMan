using UnityEngine;
using System.Collections;

public class Action : MonoBehaviour {
	public string status;
	private Driving drive;
	public float health, camStatus;
	private bool hurting;
	public Camera cam;
	private	Vector3 CamOffset1, CamOffset2;
	private HUDScript hud;

	// Use this for initialization
	void Start () {
		status = "foot" ; 
		health = 100.0f;
		hurting = false;
		camStatus = 0;
		CamOffset1 = new Vector3 (0.256f, 1.753f, -0.322f);
		CamOffset2 = new Vector3 (0.256f, 2.039f, -2.035f);
		hud = (HUDScript) gameObject.GetComponent (typeof(HUDScript));
	}
	
	// Update is called once per frame
	void Update () {
		checkAlive ();

		if (Input.GetKeyDown ("e")) {
			if (status == "foot")
				tryRide ();
		}

		if (Input.GetKeyDown ("c")) {
			toggleCam ();
		}
		hud.setSliderValue (health);
	}

	void checkAlive(){
		if (hurting) {
			health -= Time.deltaTime * 1.0f;
		}
		if (health < 0.0f) {
			Destroy (gameObject);
		}
	}

	void tryRide(){
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Vehicle");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				closest = go;
				distance = curDistance;
			}
		}
		if (distance < 200) {
			Transform vehicle = closest.transform.GetChild (0);
			vehicle.GetChild(5).GetComponent<Camera> ().enabled = true;
			drive = closest.GetComponent<Driving> ();
			drive.drivable = true;
			Destroy (gameObject);
		}
	}

	void toggleCam(){
		camStatus += 1;
		if (camStatus % 2 == 0) {
			cam.transform.localPosition = CamOffset1;
		}
		else {
			cam.transform.localPosition = CamOffset2;
		}
	}
	void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag){
		case "Zombie":
				hurting = true;
				health -= 10.0f;
				break;
			case "Bullet":
				health -= 20.0f;
				break;
		}
	}

	void OnCollisionExit(Collision collision) {
		switch (collision.gameObject.tag){
			case "Zombie":
				hurting = false;
				break;
		}
	}

	/*void OnControllerColliderHit(ControllerColliderHit collision) {
		switch (collision.gameObject.tag){
			case "Zombie":
			hurting = true;
			health -= 10.0f;
			break;
			case "Bullet":
			health -= 20.0f;
			break;
		}
	}*/
}
