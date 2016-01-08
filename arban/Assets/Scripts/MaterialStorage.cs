using UnityEngine;
using System.Collections;

public class MaterialStorage : MonoBehaviour {

	public Material[] materials;

	public Material GetMaterial(string name) {
		foreach (Material material in materials) {
			if (material.name == name)
				return material;
		}
		return null;
	}
}
