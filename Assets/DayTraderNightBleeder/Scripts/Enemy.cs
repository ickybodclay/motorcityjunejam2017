using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private AudioSource audioSource;

	private void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void TakeDamage() {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.Play();
    }
}
