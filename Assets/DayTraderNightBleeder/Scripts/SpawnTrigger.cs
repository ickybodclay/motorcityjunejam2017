using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour {
    [SerializeField] private Spawner[] spawners;

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !isActivated) {
            isActivated = true;
            foreach (Spawner spawner in spawners) {
                spawner.Activate();
            }
            Destroy(gameObject, 2f);
        }
    }
}
