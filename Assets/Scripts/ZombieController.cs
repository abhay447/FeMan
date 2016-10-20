using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {
	private GameObject Meat,Human;
	private Animator anim;
	public float health;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		health = 100.0f;
	}

	// Update is called once per frame
	void Update () {
		shouldBePresent ();
		findMeat ();
		Move ();
	}

	void Move(){
		Vector3 fwd = Vector3.forward;
		transform.Translate (fwd * 3 * Time.deltaTime);
	}

	void shouldBePresent(){
		//health criteria
		if(health < 0.0f)
			Destroy (gameObject);

		//distance from player criteria
		GameObject go;
		go = GameObject.FindGameObjectWithTag("Player");
		if (go == null)
			return;
		Vector3 position = transform.position;
		Vector3 diff = go.transform.position - position;
		float distance = diff.sqrMagnitude;
		if (distance > 10000) {
			Destroy (gameObject);
		}
	}

	void findMeat(){
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Human");
		Meat = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				Meat = go;
				distance = curDistance;
			}
		}
		if (distance < 8000) {
			transform.LookAt (Meat.transform);
		}
		if (distance > 20000) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag){
			case "Terrain":				
				break;
			case "Player":
				break;
			case "Human":
				health -= 5f;
				anim.SetInteger ("Attack", 1);
				break;
			case "Bullet":
				health -= 20.0f;
				break;
			default:
				transform.RotateAround(transform.position, transform.up, 90);
				break;
		}
	}

	void OnCollisionExit(Collision collision) {
		switch (collision.gameObject.tag){
			case "Terrain":				
			break;
			case "Player":
			break;
			case "Human":
			anim.SetInteger ("Attack", 0);
			break;
			default:
			transform.RotateAround(transform.position, transform.up, 180);
			break;
		}
	}
}
