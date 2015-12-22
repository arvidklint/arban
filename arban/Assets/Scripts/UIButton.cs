using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour {
	public MasterController mc;
	// Use this for initialization
	void Start () {
		mc.buttons.Add(this);
	}

	public void show() {
		gameObject.SetActive(true);
	}

	public void hide() {
		gameObject.SetActive(false);
	}
}
