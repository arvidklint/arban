using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterController : MonoBehaviour {
	
	public Item selected;
	public ArrayList items = new ArrayList();
	public bool isSurface;
	NetworkClient client;

	public void DeleteSelected() {
		items.Remove(selected);
		Destroy (selected.gameObject);
	}

	public void toggleSelect(Item item) {
		if (item == selected) deselect(item); else select(item);
	}

	public void select(Item item) {
		selected = item;
		notifyObservers("select");
	}

	public void deselect(Item item) {
		// Ska implementeras i framtiden när enstaka objekt kan avmarkeras. Tills vidare kör den DeselectAll. 
		DeselectAll();
	}

	public void DeselectAll() {
		selected = null;
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

	public bool somethingSelected() {
		return (selected != null);
	}

	public bool compareMousePositions(Vector2 pos1, Vector2 pos2) {
		return ((pos1[0] == pos2[0]) && (pos1[1] == pos2[1])) ? false : true;
	}
	
	void LateUpdate() {
		if (somethingSelected()) {
			float pinchAmount = 0;
			Quaternion desiredRotation = selected.transform.rotation;
			
			DetectTouchMovement.Calculate();
			
			if (Mathf.Abs(DetectTouchMovement.turnAngleDelta) > 0) { // rotate
				Debug.Log("Is rotating");
				Vector3 rotationDeg = Vector3.zero;
				rotationDeg.z = DetectTouchMovement.turnAngleDelta;
				desiredRotation *= Quaternion.Euler(rotationDeg*2);
			}
			
			// not so sure those will work:
			selected.transform.rotation = desiredRotation;
			selected.transform.position += Vector3.forward * pinchAmount;
		}

		if (client) {
			client.RpcRotate(selected.name, selected.transform.rotation);
		}
	}

	public void SetClient(NetworkClient _client) {
		client = _client;
	}
}
