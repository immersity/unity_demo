using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	
	public float forwardSpeed;
	public float sidewaySpeed;


	public GameObject camera;
		
	public float inputSpeed;

	public float getReadyTime;

	public bool magnetEnabled;
	public bool touchEnabled;

	
	private MagneticClick magneticClick;


	private bool playerIsMoving = false;
	
	void Start() {
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		magneticClick = new MagneticClick ();
		magneticClick.init ();
	} 
	
	
	
	void FixedUpdate(){

				
		float verticalInput = Input.GetAxis("Vertical");
		float horizontalInput = Input.GetAxis ("Horizontal");


		if (magnetEnabled) {
		
			magneticClick.magUpdate(Input.acceleration, Input.compass.rawVector);

			if (magneticClick.clicked ()) 
			{
				if(Time.time < getReadyTime){
					playerIsMoving = false;
				}
				else {
					playerIsMoving = !playerIsMoving;
				}

			}
		}


		if (touchEnabled) {
			if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)) {
				playerIsMoving = true;
			}
			else {
				playerIsMoving = false;
			}

		}

		if (playerIsMoving && (touchEnabled || magnetEnabled)) 
		{
			verticalInput = inputSpeed * 0.5f;		
		}

		Vector3 forwardMovement = new Vector3 (verticalInput * Sin (camera.transform.rotation.eulerAngles.y),
		                                0.0f, 
		                               verticalInput * Cos (camera.transform.rotation.eulerAngles.y));


		Vector3 sidewayMovement = new Vector3 (horizontalInput * Cos (camera.transform.rotation.eulerAngles.y),
		                                          0.0f, 
		                                          -1 * horizontalInput * Sin (camera.transform.rotation.eulerAngles.y));

		Vector3 forwardTranslation = forwardMovement * forwardSpeed * Time.deltaTime * Cos (camera.transform.rotation.eulerAngles.x);
		Vector3 horizontalTranslation = sidewayMovement * sidewaySpeed * Time.deltaTime ;

		transform.Translate (forwardTranslation + horizontalTranslation);
		
	}
	
	private float Cos( float degrees){
		return Mathf.Cos (Mathf.Deg2Rad * degrees);
	}
	
	private float Sin( float degrees){
		return Mathf.Sin (Mathf.Deg2Rad * degrees);
	}
	
}