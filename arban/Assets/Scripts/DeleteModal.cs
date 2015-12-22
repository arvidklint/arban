using UnityEngine;
using System.Collections;

public class DeleteModal : MonoBehaviour {
	public MasterController mc;

	// Use this for initialization
	void Start () {
		mc.deleteModal = this;
		hide();
	}

	public void hide() {
		gameObject.SetActive(false);
	}

	public void show() {
		gameObject.SetActive(true);
	}
}
