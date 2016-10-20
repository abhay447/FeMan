using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System.Threading;

public class Motion : MonoBehaviour {
    private Animator anim;
	private float mvFwd, myTurn, Direction;

	/*public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 7F;
	public float sensitivityY = 7F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 100F;*/
	public float gravityY = 20F;
	/*float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;*/
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool clampVerticalRotation = true;
	public float MinimumX = -50F;
	public float MaximumX = 60F;
	public bool smooth;
	public float smoothTime = 5f;
	public bool lockCursor = true;


	private Quaternion m_transformTargetRot;
	private Quaternion m_CameraTargetRot;
	private bool m_cursorIsLocked = true;

	// Use this for initialization
	void Start () {
    	anim = GetComponent<Animator>();
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		//originalRotation = transform.localRotation;
		m_transformTargetRot = transform.localRotation;
		m_CameraTargetRot = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		mvFwd = Input.GetAxis ("Vertical");
		myTurn = Input.GetAxis ("Horizontal");
		LookRotation ();
		Run ();
		close ();
	}

	void close(){
		if (Input.GetKey ("escape"))
			Application.Quit ();
	}

	void Run(){
		anim.SetFloat ("Speed", mvFwd * 10.0f);
		if (myTurn > 0)
			Direction = 1.0f;			
		if (myTurn < 0)
			Direction = -1.0f;
		if (myTurn == 0)
			Direction = 0.0f;
		anim.SetFloat ("Direction", Direction);
		anim.SetInteger ("Side", (int)Direction);


		Vector3 fwd = transform.forward;
		//transform.Translate (fwd * mvFwd * 10 * Time.deltaTime);
		gameObject.GetComponent <CharacterController>().Move (fwd * mvFwd * 10 * Time.deltaTime);
		gameObject.GetComponent <CharacterController> ().Move (new Vector3(0,-gravityY,0) * Time.deltaTime);
		transform.Translate (Vector3.right * myTurn * 3 * Time.deltaTime);
	}

	/*void Look(){
		
		if (axes == RotationAxes.MouseXAndY)
		{
			// Read the mouse input axis
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
			transform.localRotation = originalRotation * xQuaternion * yQuaternion;
			Debug.Log (rotationX);
		}
		else if (axes == RotationAxes.MouseX)
		{
			rotationX += Input.GetAxis("Mouse X") * sensitivityX;
			rotationX = ClampAngle (rotationX, minimumX, maximumX);
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			transform.localRotation = originalRotation * xQuaternion;
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = ClampAngle (rotationY, minimumY, maximumY);
			Quaternion yQuaternion = Quaternion.AngleAxis (-rotationY, Vector3.right);
			transform.rotation = originalRotation * yQuaternion;
		}
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}*/
	public void LookRotation()
	{
		float yRot = Input.GetAxis("Mouse X") * XSensitivity;
		float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

		m_transformTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

		if(clampVerticalRotation)
			m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);
		//m_transformTargetRot.z = m_CameraTargetRot.x;
		if(smooth)
		{
			transform.localRotation = Quaternion.Slerp (transform.localRotation, m_transformTargetRot*m_CameraTargetRot,
			                                            smoothTime * Time.deltaTime);
			//camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,smoothTime * Time.deltaTime);
		}
		else
		{
			transform.localRotation = m_transformTargetRot*m_CameraTargetRot;
			//camera.localRotation = m_CameraTargetRot;
		}

		UpdateCursorLock();
	}

	public void SetCursorLock(bool value)
	{
		lockCursor = value;
		if(!lockCursor)
		{//we force unlock the cursor if the user disable the cursor locking helper
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	public void UpdateCursorLock()
	{
		//if the user set "lockCursor" we check & properly lock the cursos
		if (lockCursor)
			InternalLockUpdate();
	}

	private void InternalLockUpdate()
	{
		if(Input.GetKeyUp(KeyCode.Escape))
		{
			m_cursorIsLocked = false;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			m_cursorIsLocked = true;
		}

		if (m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if (!m_cursorIsLocked)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}


}
