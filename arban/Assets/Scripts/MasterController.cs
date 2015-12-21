using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterController : MonoBehaviour {
	
	public GameObject selected;
	public ArrayList items = new ArrayList();
	public bool isSurface;

	public void select(GameObject item) {
		selected = item;
		notifyObservers("select");
	}

	public void addObserver(Item item) {
		items.Add(item);
	}

	public void notifyObservers(string msg) {

		foreach (Item item in items) {
			item.updateSelf(msg);
		}
	}

	void LateUpdate() {
		if (selected == null) {
			return;
		}

		float pinchAmount = 0;
		Quaternion desiredRotation = selected.transform.rotation;
		
		DetectTouchMovement.Calculate();
		
		if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0) { // rotate
			Debug.Log("Is rotating");
			Vector3 rotationDeg = Vector3.zero;
			rotationDeg.y = -DetectTouchMovement.turnAngleDelta;
			desiredRotation *= Quaternion.Euler(rotationDeg*2);
		}
		
		// not so sure those will work:
		selected.transform.rotation = desiredRotation;
		selected.transform.position += Vector3.forward * pinchAmount;
	}
}
