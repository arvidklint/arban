using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkClient : NetworkBehaviour {

	public GameObject cityPrefabViewer;
	public GameObject cityPrefabSurface;
	public GameObject parent;
	GameObject city;
	bool isSurface;
	
	NetworkClient client;

	void Start() {
		isSurface = (Application.loadedLevelName == "Surface");

		Debug.Log ("Start");

		// Find parent
		if (isSurface) {
			parent = (GameObject)GameObject.Find ("CitySurface");
			if (parent == null) {
				Debug.LogError ("Could not find parent Map!");
			}
		} else {
			parent = (GameObject)GameObject.Find ("CityViewer");
			if (parent == null) {
				Debug.LogError ("Could not find parent ImageTarget!");
			}
		}

		if (isLocalPlayer) {
			FindMe();
		}

		

//		if (isSurface) {
//			city = (GameObject)Instantiate (cityPrefabSurface, Vector3.zero, Quaternion.identity);
//			city.transform.parent = parent.transform;
//		} else {
//			city = (GameObject)Instantiate(cityPrefabViewer, Vector3.zero, Quaternion.identity);
//			city.transform.parent = parent.transform;
//		}
	}

	void SetChildren() {

	}

	void FindMe() {
		Item[] items;
		items = parent.GetComponentsInChildren<Item>();
		foreach (Item item in items) {
			item.SetClient(this);
		}
		parent.GetComponent<MasterController>().SetClient(this);
	}

	void FixedUpdate() {
//		Debug.Log (parent.transform.position);
	}
	
	[ClientRpc]
	public void RpcMove(string name, Vector3 _position) {
		//Debug.Log ("Rpc Move " + name);
		if (!isSurface) {
			_position *= 50f;
			_position.x *= -1f;
			_position.z *= -1f;
		}
		GameObject.Find(name).transform.position = _position;
	}

	[ClientRpc]
	public void RpcRotate(string name, Quaternion _rotation) {
		GameObject.Find(name).transform.rotation = _rotation;
	}

	[ClientRpc]
	public void RpcAdd(GameObject item) {
		// 
	}

	[ClientRpc]
	public void RpcDelete(string name) {
		Destroy(GameObject.Find(name));
	}
}
