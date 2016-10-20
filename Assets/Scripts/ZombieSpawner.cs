using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {
	public GameObject zombiePrefab;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!inSafeZone())
			StartCoroutine (Spawn ());
	}

	IEnumerator Spawn(){
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("Zombie");
		if (gos.Length < 1) {
			for (int i=0; i<6; i++) {
				Vector3 position = transform.position + new Vector3 (Random.Range (-100.0f, 100.0f), 0, Random.Range (-100.0f, 100.0f));
				Instantiate (zombiePrefab, position, Quaternion.identity);
				yield return new WaitForSeconds (2.0f);
			}
		}
		yield return new WaitForSeconds (10.0f);
	}

	bool inSafeZone(){
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("SafeZone");
		GameObject safez = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance) {
				safez = go;
				distance = curDistance;
			}
		}
		if (distance < 5000f)
			return true;
		return false;
		Debug.Log (distance);
	}
}
