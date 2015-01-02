using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//public static XmlDocument XmlDoc;
//public static XmlNodeList xnl;
//public TextAsset TA;

public class MagneticClick {
	//  Concept: two FIR filters,  running on Magnetics and tilt.
	//  If the tilt hasn't changed, but the compass has, then the magnetic field moved
	//  without device this is the essence of a cardboard magnet click.
	private Vector3 lastTiltVector;
	public float tiltedBaseLine = 0f;
	public float magnetBaseLine = 0f;
	
	public float tiltedMagn = 0f;
	public float magnetMagn = 0f;
	
	private int N_SlowFIR = 25;
	private int N_FastFIR_magnet = 3;
	private int N_FastFIR_tilted = 5;  // Clicking the magnet tends to tilt the device slightly.
	
	
	public float threshold = 1.0f;
	
	bool click = false;
	bool clickReported = false;
	
	public void init() {
		Input.compass.enabled = true;
		
		// Note that init is platform specific to unity.
		magnetMagn = Input.compass.rawVector.magnitude;
		magnetBaseLine = Input.compass.rawVector.magnitude;
		tiltedBaseLine = Input.acceleration.magnitude;
		tiltedMagn = Input.acceleration.magnitude;
	}
	
	public void magUpdate(Vector3 acc,  Vector3 compass) {
		// Call this function in the Update of a monobehaviour as follows:
		// <magneticClickInstance>.magUpdate(Input.acceleration, Input.compass.rawVector);
		
		// we are interested in the change of the tilt not the actual tilt.
		Vector3 TiltNow = acc;
		Vector3 motionVec3 = TiltNow - lastTiltVector;
		lastTiltVector = TiltNow;
		
		// update tilt and compass "fast" values
		tiltedMagn = ((N_FastFIR_tilted-1) * tiltedMagn + motionVec3.magnitude) / N_FastFIR_tilted;
		magnetMagn = ((N_FastFIR_magnet-1) * magnetMagn + compass.magnitude) / N_FastFIR_magnet;
		
		// update the "slow" values
		tiltedBaseLine = ( (N_SlowFIR-1) * tiltedBaseLine + motionVec3.magnitude) / N_SlowFIR;
		magnetBaseLine = ( (N_SlowFIR-1) * magnetBaseLine + compass.magnitude) / N_SlowFIR;
		
		if( tiltedMagn < 0.2 && (magnetMagn / magnetBaseLine) > 1.1  ) {
			if( clickReported == false) {
				click = true;
			}
			clickReported = true;
		} else  {
			clickReported = false;
		}
	}
	
	public bool clicked()  {
		// Basic premise is that the magnitude of magnetic field should change while the 
		// device is steady.  This seems to be suiltable for menus etc.
		
		// Clear the click by reading (so each 'click' returns true only once)
		if(click == true) {
			click = false;
			return true;
		} else {
			return false;
		}
	}
}