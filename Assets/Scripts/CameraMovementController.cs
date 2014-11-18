using UnityEngine;
using System.Collections;

public class CameraMovementController : MonoBehaviour {

	public GameObject player;
	private Vector3 offset;


	void Start () {
		offset = transform.position;
		offset.y = 1;
	}
	

	void LateUpdate () {
		transform.position = player.transform.position + offset;
	
	}
}
