using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class Item : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	MasterController mc;

	void Start() {
		mc = transform.parent.GetComponent<MasterController>();
		mc.addObserver(this);
	}
	
	void OnMouseDown() {
		mc.select(gameObject);
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag() {
		if (!(Input.touchCount == 2)) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}

	public void updateSelf(string msg) {
		if (msg == "select") {
			if (gameObject == mc.selected) {
				select ();
			} else {
				deselect ();
			}
		}
	}

	void select() {
		setItemSelectionColor(Color.blue);
	}

	void deselect() {
		setItemSelectionColor(Color.white);
	}

	void setItemSelectionColor(Color c) {
		Renderer rend = GetComponent<Renderer>();
		rend.material.SetColor ("_Color", c);
	}
}