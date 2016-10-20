using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {
	public Slider slider;
	// Use this for initialization
	void Start () {
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setSliderValue(float x){
		slider.value = x;
	}

}
