using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeController : MonoBehaviour {
    private AudioSource audioSource;

    [SerializeField] private AudioClip bossMusic;

	void Start () {
        audioSource = Camera.main.GetComponent<AudioSource>();

        if (audioSource == null) throw new MissingComponentException("Main camera missing Audio Source component");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            audioSource.Stop();
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }
}
