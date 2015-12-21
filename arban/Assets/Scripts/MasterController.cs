using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterController : MonoBehaviour {
	
	public GameObject selected;
	public ArrayList items = new ArrayList();
	public bool isSurface;
	NetworkClient client;

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

	public void SetClient(NetworkClient _client) {
		client = _client;
	}

	void LateUpdate() {
		if (selected == null) {
			return;
		}

		float pinchAmount = 0;
		Quaternion desiredRotation = selected.transform.rotation;
		
		DetectTouchMovement.Calculate();
		
		if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0) { // rotate
			Vector3 rotationDeg = Vector3.zero;
			rotationDeg.y = -DetectTouchMovement.turnAngleDelta;
			desiredRotation *= Quaternion.Euler(rotationDeg*2);
		}

		selected.transform.rotation = desiredRotation;
		//selected.transform.position += Vector3.forward * pinchAmount;

		if (client) {
			client.RpcRotate(selected.name, selected.transform.rotation);
		}
	}
}
