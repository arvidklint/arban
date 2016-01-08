using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterController : MonoBehaviour {
	
	public Item selected;
	public ArrayList items = new ArrayList();
	public ArrayList buttons = new ArrayList();
	public bool isSurface;
	NetworkClient client;
	public DeleteModal deleteModal;
	public addItemsPanel addItemsPanel;
	public bool itemsClickable = true;
	string destroyedName;
	public Prefabs prefabs;
	int newItemNumber = 0;
	Vector3 stdPosition = new Vector3(2f, 0f, -4f);
    string surfaceName = "CitySurface";
    string viewerName = "CityViewer";

	public void DeleteSelected() {
		items.Remove(selected);
		destroyedName = selected.gameObject.name;
		Destroy(selected.gameObject);
		if (client) client.RpcDelete(destroyedName);
		DeselectAll();
		closeDeleteModal();
	}

	public void showDeleteModal() {
		if (somethingSelected()) {
			itemsClickable = false;
			deleteModal.show();
		}
	}

	public void closeDeleteModal() {
		itemsClickable = true;
		deleteModal.hide();
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

	public void showAddItemsPanel() {
		toggleButtons(false);
		addItemsPanel.show();
	}

	public void hideAddItemsPanel() {
		addItemsPanel.hide();
		toggleButtons(true);
	}

	public void toggleButtons(bool show) {
		if (show) {
			foreach (UIButton button in buttons) {
				button.show();
			}
		} else {
			foreach (UIButton button in buttons) {
				button.hide();
			}
		}
	}

	public void addObserver(Item item) {
		items.Add(item);
	}

	public void notifyObservers(string msg) {
		foreach (Item item in items) {
			item.updateSelf(msg);
		}

		foreach (UIButton button in buttons) {
			button.updateSelf(msg);
		}
	}

	public bool somethingSelected() {
		return (selected != null);
	}

	public bool compareMousePositions(Vector2 pos1, Vector2 pos2) {
		return ((pos1[0] == pos2[0]) && (pos1[1] == pos2[1]));
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
            if (client)
            {
                client.RpcRotate(selected.name, selected.transform.rotation);
            }
        }

		
	}

	public void SetClient(NetworkClient _client) {
		client = _client;
	}

	public void addItem(string prefabName) {
        GameObject prefab = prefabs.getPrefab(prefabName);
        GameObject newItem = Instantiate(prefab, stdPosition, Quaternion.identity) as GameObject;

        newItemNumber++;
        string newItemName = "new" + newItemNumber.ToString();
        newItem.name = newItemName;

        newItem.transform.parent = GameObject.Find(surfaceName).transform; // add the new item as a child to CitySurface

        if (client) client.RpcAddToViewer(prefabName, newItemName);
	}
}
