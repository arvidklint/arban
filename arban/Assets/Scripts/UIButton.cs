using UnityEngine;
using System.Collections;

public class UIButton : MonoBehaviour {
	public MasterController mc;
	public bool visibilityRequiresSelection;
	// Use this for initialization
	void Start () {
		mc.buttons.Add(this);
		updateSelf("select");
	}

	public void show() {
		if (!visibilityRequiresSelection || (visibilityRequiresSelection && mc.somethingSelected())) {
			gameObject.SetActive(true);
		}
	}

	public void hide() {
		gameObject.SetActive(false);
	}

	public void updateSelf(string msg) {
		if (msg == "select") {
			if (visibilityRequiresSelection) {
				if (mc.somethingSelected()) show();
				else hide();
			}
		}
	}
}
