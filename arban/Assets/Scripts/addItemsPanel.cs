using UnityEngine;
using System.Collections;

public class addItemsPanel : MonoBehaviour {
	public MasterController mc;

	// Use this for initialization
	void Start () {
		mc.addItemsPanel = this;
		hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void hide() {
		gameObject.SetActive(false);
	}

	public void show() {
		gameObject.SetActive(true);
	}
}
