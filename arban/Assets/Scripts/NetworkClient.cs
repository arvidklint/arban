using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkClient : NetworkBehaviour {

	public GameObject cityPrefabViewer;
	public GameObject cityPrefabSurface;
	public GameObject parent;
	GameObject city;
	bool isSurface;
    public Prefabs prefabs;
    float surfaceScale = 50f;
    Vector3 stdPosition;
    string viewerName = "CityViewer";
    string surfaceName = "CitySurface";

    NetworkClient client;

	void Start() {

		isSurface = (Application.loadedLevelName == "Surface");

        Debug.Log ("Start");

        stdPosition = new Vector3(2f * -1f, 0f, -4f * -1f) * surfaceScale;

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

		prefabs = (Prefabs)GameObject.Find ("PrefabStorage").GetComponent<Prefabs> ();

		

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
		Item[] items = parent.GetComponentsInChildren<Item>();

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
		Debug.Log ("Rpc Move " + name);
		if (isSurface) {
			GameObject.Find("/Map/" + surfaceName + "/" + name).transform.position = _position;
		} else {
			_position *= surfaceScale;
			_position.x *= -1f;
			_position.z *= -1f;
			GameObject.Find("/ImageTarget/" + viewerName + "/" + name).transform.position = _position;
		}
	}

	[ClientRpc]
	public void RpcRotate(string name, Quaternion _rotation) {
		if (isSurface) {
			Transform t = GameObject.Find ("/Map/" + surfaceName + "/" + name).transform;
			t.rotation = _rotation;
		} else {
			Transform t = GameObject.Find ("/ImageTarget/" + viewerName + "/" + name).transform;
			t.rotation = new Quaternion (_rotation.x, _rotation.y + 180f, _rotation.z, _rotation.w);
		}
	
	}

	[ClientRpc]
	public void RpcDelete(string name) {
		Destroy(GameObject.Find(name));
	}

	[ClientRpc]
	public void RpcAddToViewer(string prefabName, string newItemName) {
        if (!isSurface) {
            GameObject prefab = prefabs.getPrefab(prefabName);

            GameObject newItem = Instantiate(prefab, stdPosition, Quaternion.identity) as GameObject;

			newItem.transform.localScale *= surfaceScale;
			newItem.transform.Rotate(0f, 180f, 0f);

            newItem.GetComponent<Item>().SetClient(this);

            newItem.name = newItemName;
            newItem.transform.parent = GameObject.Find(viewerName).transform; // add the new item as a child to CitySurface
        }
    }
}
