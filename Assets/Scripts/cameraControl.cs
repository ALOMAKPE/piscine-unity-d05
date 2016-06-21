using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {

	public Camera cam { get; private set; }
	public Rigidbody body { get; private set; }

	public bool withMouse = true;
	public float mouseRatio = 0.5f;
	public bool withKeys = true;
	public float speed = 50f;

	private Vector3 lastMousePosition;
	private float cameraX;
	private float cameraY;

	void Start () {
		cam = GetComponent<Camera> ();
		body = GetComponent<Rigidbody> ();
		lastMousePosition = Input.mousePosition;
		cameraReset ();
	}
	
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			withMouse = withMouse ? false : true;
			cameraReset();
		}
		if (withMouse) {
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
//			Camera.main.transform.rotation *= (Vector3)(Input.mousePosition - lastPosition) * mouseRatio;
			Vector3 cameraDisplacement = (Input.mousePosition - lastMousePosition) * mouseRatio;
			cameraX -= cameraDisplacement.y;
			cameraY += cameraDisplacement.x;
			cam.transform.rotation = Quaternion.Euler (new Vector3 (cameraX, cameraY, 0));
		} else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
		if (withKeys) {
			float yAxis = 0f;
			if (Input.GetKey(KeyCode.E)) yAxis += 1;
			if (Input.GetKey(KeyCode.Q)) yAxis -= 1;
			body.velocity = cam.transform.rotation * new Vector3 (Input.GetAxis("Horizontal"), yAxis, Input.GetAxis("Vertical")) * speed;
		}
		body.angularVelocity *= 0f;
		lastMousePosition = Input.mousePosition;
	}

	public void cameraReset() {
		cameraX = cam.transform.rotation.eulerAngles.x;
		cameraY = cam.transform.rotation.eulerAngles.y;
	}
}
