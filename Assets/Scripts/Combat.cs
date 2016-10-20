using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour {
	public bool punch;
	private Animator anim;
	private int Weapon;
	public Light MuzzleLight;
	LineRenderer lr;
	public GameObject bulletPrefab;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		Weapon = 0;
		MuzzleLight.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		selectWeapon ();
		punch = Input.GetMouseButtonDown (0);
		anim.SetBool ("Punch", punch);
		if (Weapon == 1 && punch && anim.GetFloat("Direction") == 0.0f) {
			StartCoroutine (ShootPistol ());
		}
		drawLasernShoot ();
	}

	IEnumerator ShootPistol(){
		GameObject go = GameObject.FindGameObjectWithTag("Gun");
		go.GetComponent<AudioSource> ().Play ();
		go.transform.Find ("MuzzleFlash").GetComponent <Renderer>().enabled = true;

		MuzzleLight.enabled = true;
		yield return new WaitForSeconds(0.02f);
		go.transform.Find ("MuzzleFlash").GetComponent <Renderer>().enabled = false;
		MuzzleLight.enabled = false;

		GameObject bulletClone = Instantiate(bulletPrefab, go.transform.position - go.transform.right, go.transform.rotation) as GameObject;
		bulletClone.GetComponent <Rigidbody>().velocity = -go.transform.right*150f ;
	}

	void drawLasernShoot(){
		GameObject go = GameObject.FindGameObjectWithTag ("Gun");
		if(go.GetComponent<LineRenderer> ()==null)
			go.AddComponent<LineRenderer> ();
		lr = go.GetComponent<LineRenderer> ();
		lr.material = new Material (Shader.Find ("Particles/Alpha Blended Premultiply"));
		lr.SetColors(Color.red, Color.clear);
		lr.SetWidth (0.01f, 0.01f);
		lr.SetPosition (0, go.transform.position);
		if (anim.GetInteger ("Weapon") != 0)
			lr.SetPosition (1, go.transform.position - 50 * go.transform.right);
		else
			lr.SetPosition (1, go.transform.position);

	}
	void selectWeapon(){
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		float oldWeapon = Weapon;
		if ( scroll !=0f ) // forward
		{
			Weapon += Mathf.Abs (Mathf.FloorToInt (Mathf.Abs (scroll)/scroll));
			Weapon = Weapon % 2;
			anim.SetFloat ("Speed",0f);
			anim.SetBool ("Punch",false);
			anim.SetInteger ("Weapon", Weapon);
		}
		if (Weapon == 0 && Weapon != oldWeapon) {
			GameObject go = GameObject.FindGameObjectWithTag ("Gun").transform.GetChild (0).gameObject;
			Renderer[] rs = go.GetComponentsInChildren <Renderer>();
			foreach(Renderer r in rs)
				r.enabled = false;
		}

		if (Weapon == 1 && Weapon != oldWeapon) {
			GameObject go = GameObject.FindGameObjectWithTag ("Gun").transform.GetChild (0).gameObject;
			Renderer[] rs = go.GetComponentsInChildren <Renderer>();
			foreach(Renderer r in rs)
				r.enabled = true;
		}
	}
}
