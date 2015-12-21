using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class Item : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	MasterController mc;
	Vector2 mouseDownPos;
	bool mouseHasMoved;
	Renderer rend;
	Color transparent;

	void Start() {
		mc = transform.parent.GetComponent<MasterController>();
		mc.addObserver(this);
		rend = GetComponent<Renderer>();
		Color c = rend.material.color;
		if (mc.isSurface) {
			transparent = new Color (0.35f, 0.84f, 0.86f, 0.4f);
		} else {
			transparent = new Color (c.r, c.g, c.b, c.a);
		}
//		Debug.Log (transparent);
		rend.material.SetColor("_Color", transparent);
	}
	
	void OnMouseDown() {
		mouseDownPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseUp() {
		mouseHasMoved = mc.compareMousePositions(mouseDownPos, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		if (!mouseHasMoved) mc.toggleSelect(gameObject);
	}
	
	void OnMouseDrag() {
		if (!(Input.touchCount == 2) && isSelected()) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;
		}
	}

	public void updateSelf(string msg) {
		if (msg == "select") {
			updateSelectionStatus();
		}
	}

	public bool isSelected() {
		return (gameObject == mc.selected);
	}

	void updateSelectionStatus() {
		if (isSelected()) {
			markSelected();
		} else {
			markDeselected();
		}
	}

	void markSelected() {
		setItemSelectionColor(Color.blue);
	}

	void markDeselected() {
		setItemSelectionColor(Color.white);
	}

	void deselect() {
		setItemSelectionColor(transparent);
	}
	
	void setItemSelectionColor(Color c) {
		rend.material.SetColor ("_Color", c);
	}
}