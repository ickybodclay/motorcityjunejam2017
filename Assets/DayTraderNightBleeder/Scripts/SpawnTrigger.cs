using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour {
    [SerializeField] private Spawner spawner;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player" && !spawner.IsActivated()) {
            spawner.Activate();
            Destroy(gameObject);
        }
    }
}
