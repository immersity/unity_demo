using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	
	public float speed;
	public GameObject camera;
		
	public float magnetSpeed;
	
	private MagneticClick magneticClick;

	private bool playerIsMoving = false;
	
	void Start() {
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		magneticClick = new MagneticClick ();
		magneticClick.init ();
	} 
	
	
	
	void FixedUpdate(){

				
		float moveVertical = Input.GetAxis("Vertical");
			
		magneticClick.magUpdate(Input.acceleration, Input.compass.rawVector);

		if(playerIsMoving)
		{
			moveVertical = magnetSpeed * 0.5f;
		}

		if (magneticClick.clicked ()) 
		{
			Debug.Log(Time.time + ": Click detected!");
			playerIsMoving = !playerIsMoving;
		}
			
		Debug.Log (Time.time + ": magnetMagn=" + magneticClick.magnetMagn + ",magnetMagnBaseLine=" + magneticClick.magnetBaseLine);
	
		
		Vector3 movement = new Vector3( moveVertical * Sin(camera.transform.rotation.eulerAngles.y), 0.0f, moveVertical * Cos(camera.transform.rotation.eulerAngles.y));
		transform.Translate (movement * speed * Time.deltaTime * Cos (camera.transform.rotation.eulerAngles.x));
		
	}
	
	private float Cos( float degrees){
		return Mathf.Cos (Mathf.Deg2Rad * degrees);
	}
	
	private float Sin( float degrees){
		return Mathf.Sin (Mathf.Deg2Rad * degrees);
	}
	
}