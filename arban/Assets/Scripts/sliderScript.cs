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
		float sunY = lightSlider.value - 90;
		float sunX = -0.007237f*(sunY*sunY)+58.62f;
		dirLight.GetComponent<Light> ().transform.eulerAngles = new Vector3 (sunX, sunY, dirLight.GetComponent<Light> ().transform.eulerAngles.z);
	}
}
