﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(MeshCollider))]

public class Item : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;
	MasterController mc;
	Renderer rend;
	Color transparent;
	NetworkClient client;

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
		mc.select(gameObject);
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
	
	void OnMouseDrag() {
		if (!(Input.touchCount == 2)) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;
			transform.position = curPosition;

			if(client) {
				client.RpcMove(this.name, transform.position);
			}
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

	public void SetClient(NetworkClient _client) {
		client = _client;
	}

	void select() {
		setItemSelectionColor(Color.blue);
	}

	void deselect() {
		setItemSelectionColor(transparent);
	}

	void setItemSelectionColor(Color c) {
		rend.material.SetColor ("_Color", c);
	}
}