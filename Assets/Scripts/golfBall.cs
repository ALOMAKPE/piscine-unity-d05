using UnityEngine;
using System.Collections;

public class golfBall : MonoBehaviour {

	public static golfBall instance { get; private set; }
	public Rigidbody body { get; private set; }
	
	public SphereCollider hole;
	public GameObject arrow;

	void Awake () {
		if (!instance)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
