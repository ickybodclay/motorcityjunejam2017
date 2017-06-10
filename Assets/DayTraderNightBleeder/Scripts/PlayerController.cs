using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    private PlayerMotor motor;

    public bool lockMovement = false;

    private void Start() {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update() {
        lockMovement = GameManager.Instance.IsDialogShowing();
        if (GameManager.Instance.IsDialogShowing()) {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Punch")) {
            motor.Punch();
        }

        // FIXME for testing only
        if (CrossPlatformInputManager.GetButtonDown("Interact")) {
            GameManager.Instance.ShowDialog("hello world", "nice day for a punch isn't it?", "*PUNCH*");
        }
    }

    private void FixedUpdate() {
        float h = 0f;
        float v = 0f;
        if (!lockMovement) {
            h = CrossPlatformInputManager.GetAxis("Horizontal");
            v = CrossPlatformInputManager.GetAxis("Vertical");
        }

        motor.Move(h, v);
    }
}
