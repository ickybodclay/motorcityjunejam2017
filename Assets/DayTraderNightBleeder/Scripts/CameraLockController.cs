using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class CameraLockController : MonoBehaviour {

    public Camera targetCamera;
    public bool shouldLockX;
    public bool shouldLockY;

    private CameraFollow cameraFollow;
    private bool isTriggered = false;

    private void Start() {
        cameraFollow = targetCamera.GetComponent<CameraFollow>();

        if (cameraFollow == null) throw new MissingComponentException("Target camera missing Camera Follow component");
    }

    private void Update() {
        if (isTriggered && GameManager.Instance.EnemyCount <= 0) {
            UnlockCamera();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !isTriggered) {
            LockCamera();
            isTriggered = true;
        }
    }

    private void LockCamera() {
        if (shouldLockX) {
            cameraFollow.lockX = true;
        }

        if (shouldLockY) {
            cameraFollow.lockY = true;
        }
    }

    private void UnlockCamera() {
        cameraFollow.lockX = false;
        cameraFollow.lockY = false;
    }
}
