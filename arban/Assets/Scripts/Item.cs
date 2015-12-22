using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshCollider))]

public class Item : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	MasterController mc;
	Vector2 mouseDownPos;
	bool mouseHasMoved;
	Renderer rend;
	float alpha;
	Color deselectedColor;
	Color selectedColor;
	NetworkClient client;

	void Start() {
		mc = transform.parent.GetComponent<MasterController>();
		mc.addObserver(this);
		rend = GetComponent<Renderer>();
		Color c = rend.material.color;
		if (mc.isSurface) {
			alpha = 0.4f;
			deselectedColor = new Color (0.35f, 0.84f, 0.86f, alpha);
		} else {
			alpha = 1f;
			deselectedColor = new Color (c.r, c.g, c.b, c.a);
		}

		selectedColor = new Color(0f, 0f, 1f, alpha);

		markDeselected();
	}
	
	void OnMouseDown() {
		if (mc.itemsClickable) {
			mouseDownPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		}
	}

	void OnMouseUp() {
		if (mc.itemsClickable) {
			mouseHasMoved = mc.compareMousePositions(mouseDownPos, new Vector2(Input.mousePosition.x, Input.mousePosition.y));
			if (!mouseHasMoved) mc.toggleSelect(this);
		}
	}
	
	void OnMouseDrag() {
		if (mc.itemsClickable) {
			if (!(Input.touchCount == 2) && isSelected()) {
				Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
				transform.position = curPosition;

				if (client) {
					client.RpcMove(this.name, transform.position);
				}
			}
		}
	}

	public void updateSelf(string msg) {
		if (msg == "select") {
			updateSelectionStatus();
		}
	}

	public bool isSelected() {
		return (this == mc.selected);
	}

	void updateSelectionStatus() {
		if (isSelected()) {
			markSelected();
		} else {
			markDeselected();
		}
	}

	void markSelected() {
		setItemSelectionColor(selectedColor);
	}

	public void SetClient(NetworkClient _client) {
		client = _client;
	}

	public void setRedMaterial() {
		Color c = rend.material.color;
		Color red = new Color(244f / 255f, 63f / 255f, 74f / 255f, c.a);
		rend.material.SetColor("_Color", red);
	}

	void markDeselected() {
		setItemSelectionColor(deselectedColor);
	}
	
	void setItemSelectionColor(Color c) {
		rend.material.SetColor ("_Color", c);
	}
}