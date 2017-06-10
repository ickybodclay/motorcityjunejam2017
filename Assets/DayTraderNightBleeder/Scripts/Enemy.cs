using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private AudioSource audioSource;

	private void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void TakeDamage() {
        Debug.Log(name + " hit!");
    }
}
