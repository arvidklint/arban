using UnityEngine;
using System.Collections;

public class rotator : MonoBehaviour {

	public Transform tf;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		tf.Rotate (new Vector3 (0.5f, 0.5f, 0.5f));
	}
}
