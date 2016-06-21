using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cameraControl : MonoBehaviour {

	public static cameraControl instance { get; private set; }
	
	public Camera cam { get; private set; }
	public Rigidbody body { get; private set; }

	public bool golfMode { get; private set; }
	public float mouseRatio = 0.5f;
	public float speed = 50f;
	
	private Vector3 lastMousePosition;
	private float cameraX;
	private float cameraY;

	private Quaternion fireDirection;
	private bool fireDirectionSet = false;

	private float firePower = 0f;
	public Vector3 clubPower = new Vector3 (0, 5, 10);

	public RectTransform powerMeter;

	void Awake () {
		if (!instance)
			instance = this;
	}

	void Start () {
		cam = GetComponent<Camera> ();
		body = GetComponent<Rigidbody> ();
		lastMousePosition = Input.mousePosition;
		golfMode = true;
		cameraReset ();
	}
	
	void Update () {
		if ((!golfMode && Input.GetKeyDown(KeyCode.Space)) || Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.F)) {
			golfMode = true;
			// NOT YET FIRING
			firePower = 0f;
			cameraReset();
		} else if (golfMode) {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Q)) {
				golfMode = false;
			}
		}
		if (!golfMode) {
			// ARROW
			golfBall.instance.arrow.SetActive(false);
			// CURSOR
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
			// MOUSE
			Vector3 cameraDisplacement = (Input.mousePosition - lastMousePosition) * mouseRatio;
			cameraX -= cameraDisplacement.y;
			cameraY += cameraDisplacement.x;
			cam.transform.rotation = Quaternion.Euler (new Vector3 (cameraX, cameraY, 0));
			// KEYS
			float yAxis = 0f;
			if (Input.GetKey(KeyCode.E)) yAxis += 1;
			if (Input.GetKey(KeyCode.Q)) yAxis -= 1;
			body.velocity = cam.transform.rotation * new Vector3 (Input.GetAxis("Horizontal"), yAxis, Input.GetAxis("Vertical")) * speed;
		} else {
			// ARROW
			golfBall.instance.arrow.SetActive(true);
			// CURSOR
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			// CAMERA POSITION BEHIND BALL
			body.velocity = Vector3.zero;
			if (!fireDirectionSet) {
				golfBall.instance.transform.LookAt(golfBall.instance.hole.transform.position);
				fireDirection = golfBall.instance.transform.rotation;
				fireDirectionSet = true;
			}
			Vector3 angles = fireDirection.eulerAngles;
			transform.rotation = Quaternion.Euler(new Vector3(angles.x, angles.y + Input.GetAxis("Horizontal"), angles.z));
			golfBall.instance.arrow.transform.rotation = transform.rotation;
			fireDirection = transform.rotation;
			transform.position = golfBall.instance.transform.position + (fireDirection * new Vector3 (0,1,-2));
			// KEYS
			if (firePower != 0) {
				if (Input.GetKeyDown(KeyCode.Space)) {
					// FIXME FIRE
					golfMode = false;
					golfBall.instance.body.velocity = cam.transform.rotation * clubPower * Mathf.PingPong(firePower, 1);
					firePower = 0f;
				}
				firePower += Time.deltaTime;
			} else if (Input.GetKeyDown(KeyCode.Space)) {
				firePower += Time.deltaTime;
			}
			powerMeter.offsetMax = new Vector2 (-290f + (Mathf.PingPong(firePower, 1) * 290f), powerMeter.offsetMax.y);
		}
		body.angularVelocity *= 0f;
		lastMousePosition = Input.mousePosition;
	}

	public void cameraReset() {
		cameraX = cam.transform.rotation.eulerAngles.x;
		cameraY = cam.transform.rotation.eulerAngles.y;
	}
}
