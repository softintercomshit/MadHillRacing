using UnityEngine;
using System.Collections;

public class UpgradeLoader : MonoBehaviour {


	[Space(4)]
	[Header("Enter Car ID")]
	// Car id to loadhis upgrade
	public int carID = 0;


	[Space(3)]
	[Header("Assign Car Controller")]
	// Get car controller
	public CarController carController;

	[Space(3)]
	[Header("Upgrade values")]
	// How much upgrade car for each level
	public float[] engineUpgrade;
	public float[] speedUpgrade;
	public float[] rotateUpgrade;
	public float[] fuelUpgrade;

	[Space(3)]
	[Header("Log Debug Messages")]
	public bool debug;

	GameManager manager;

	void Awake () {
		carID = PlayerPrefs.GetInt ("SelectedCar");
		// pupalate engineUpgrade array
		engineUpgrade = new float[16] {1700, 1800, 1900, 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900, 3000, 3100, 3200};
		speedUpgrade = new float[16] {23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38};
		rotateUpgrade = new float[16] {210, 200, 190, 180, 170, 160, 150, 140, 130, 120, 110, 100, 90, 80, 70, 60};
		fuelUpgrade = new float[16] {0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f, 1.1f, 1.2f, 1.3f, 1.4f, 1.5f, 1.6f};

		// Read from upgrade menu
		try{
			carController.motorPower = engineUpgrade [PlayerPrefs.GetInt ("Engine" + carID.ToString ())];
		}catch{
			carController.motorPower = engineUpgrade [PlayerPrefs.GetInt ("Engine" + carID.ToString ())-1];
		}

		try{
			carController.maxSpeed = speedUpgrade [PlayerPrefs.GetInt ("Speed" + carID.ToString ())];
		}catch{
			carController.maxSpeed = speedUpgrade [PlayerPrefs.GetInt ("Speed" + carID.ToString ())-1];
		}

		// Suspension upgrade used as car rotate force on air (when isgrounded is false in CarController script)
		try{
			carController.RotateForce = rotateUpgrade [PlayerPrefs.GetInt ("Suspension" + carID.ToString ())];
		}catch{
			carController.RotateForce = rotateUpgrade [PlayerPrefs.GetInt ("Suspension" + carID.ToString ())-1];	
		}

		#if UNITY_EDITOR
		if (debug) {
			Debug.Log ("Engine Level : " + PlayerPrefs.GetInt ("Engine" + carID.ToString ()).ToString ());
			Debug.Log ("Speed Level : " + PlayerPrefs.GetInt ("Speed" + carID.ToString ()).ToString ());
			Debug.Log ("Rotate Level : " + PlayerPrefs.GetInt ("Suspension" + carID.ToString ()).ToString ());
		}
		#endif

		manager = GameObject.FindObjectOfType<GameManager> ();

		try{
			manager.FuelTime = fuelUpgrade [PlayerPrefs.GetInt ("Fuel" + carID.ToString ())];
		}catch{
			manager.FuelTime = fuelUpgrade [PlayerPrefs.GetInt ("Fuel" + carID.ToString ())-1];
		}
	}

}


