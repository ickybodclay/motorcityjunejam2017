using System;
using UnityEngine;


namespace UnityStandardAssets._2D {
    public class CameraFollow : MonoBehaviour {
        public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
        public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
        public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
        public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
        public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
        public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.

        [HideInInspector] public bool lockX;
        [HideInInspector] public bool lockY;

        private Transform m_Player; // Reference to the player's transform.

        private void Awake() {
            // Setting up the reference.
            m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private bool CheckXMargin() {
            // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
            return Mathf.Abs(transform.position.x - m_Player.position.x) > xMargin;
        }


        private bool CheckYMargin() {
            // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
            return Mathf.Abs(transform.position.y - m_Player.position.y) > yMargin;
        }

        private void Update() {
            TrackPlayer();
        }

        private void TrackPlayer() {
            float targetX = transform.position.x;
            float targetY = transform.position.y;

            if (!lockX) {
                if (CheckXMargin()) {
                    targetX = Mathf.Lerp(transform.position.x, m_Player.position.x, xSmooth * Time.deltaTime);
                }
                targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
            }

            if (!lockY) {
                if (CheckYMargin()) {
                    targetY = Mathf.Lerp(transform.position.y, m_Player.position.y, ySmooth * Time.deltaTime);
                }
                targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

            }

            transform.position = new Vector3(targetX, targetY, transform.position.z);
        }
    }
}
