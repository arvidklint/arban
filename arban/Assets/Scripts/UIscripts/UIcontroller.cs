using UnityEngine;
using System.Collections;

public class UIcontroller : MonoBehaviour {
	MasterController mc;
	
	void Start() {
		mc = transform.parent.GetComponent<MasterController>();
//		mc.addObserver(this);
	}

	public void Delete() {
//		mc.DeleteSelected();
		Debug.Log ("test");
	}


}
