using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sliderScript : MonoBehaviour {

	Slider lightSlider;
	float sunAngle;
	GameObject dirLight;

	void Start () {
		lightSlider = gameObject.GetComponent<Slider>();
		lightSlider.onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});
		dirLight = GameObject.Find ("Directional Light");
		sunAngle = dirLight.GetComponent<Light> ().transform.eulerAngles.x;
		//Debug.Log (sunAngle);
	}
	
	void ValueChangeCheck () {
		dirLight.GetComponent<Light> ().transform.eulerAngles = new Vector3 (lightSlider.value, dirLight.GetComponent<Light> ().transform.eulerAngles.y, dirLight.GetComponent<Light> ().transform.eulerAngles.z);
		Debug.Log(lightSlider.value);
	}
}
