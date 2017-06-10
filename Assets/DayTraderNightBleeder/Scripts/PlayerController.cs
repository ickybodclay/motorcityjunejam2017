using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    private PlayerMotor motor;

    public bool lockMovement = false;

	void Start () {
        motor = GetComponent<PlayerMotor>();		
	}
	
	void FixedUpdate () {
        float h = 0f;
        float v = 0f;
        if (!lockMovement) {
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");
        }

        motor.Move(h, v);
    }
}
