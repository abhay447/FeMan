using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameProgress : MonoBehaviour {
	public GameObject[] objectives;
	public string[] objMsg;
	public int mission;
	public Text msg;
	// Use this for initialization
	void Start () {
		mission = 0;

	}
	
	// Update is called once per frame
	void Update () {
		msg.text = objMsg [mission];
	}
	void OnControllerColliderHit(ControllerColliderHit hit) {
		Debug.Log (mission);
		if (hit.gameObject.Equals (objectives [mission]))
			mission = mission+1;
	}
}
