﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    private Rigidbody2D rb2d;
    private AudioSource audioSource;
    private Animator animator;

    [SerializeField] private AudioClip punchSfx;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip hurtSfx;

    [SerializeField] private float m_MaxSpeedX = 10f;
    [SerializeField] private float m_MaxSpeedY = 6f;
    private bool m_FacingRight = true;

    private List<GameObject> punchableEnemies = new List<GameObject>();

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void Move(float moveX, float moveY) {
        rb2d.velocity = new Vector2(moveX * m_MaxSpeedX, moveY * m_MaxSpeedY);
        animator.SetFloat("Speed", rb2d.velocity.magnitude);

        if (moveX > 0 && !m_FacingRight) {
            Flip();
        }
        else if (moveX < 0 && m_FacingRight) {
            Flip();
        }
    }

    private void Flip() {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void Punch() {
        if (punchableEnemies.Count == 0) {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.clip = punchSfx;
            audioSource.Play();
        }
        else {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.clip = hitSfx;
            audioSource.Play();
        }

        foreach (GameObject victim in punchableEnemies) {
            victim.GetComponent<Enemy>().TakeDamage();
        }

        animator.SetTrigger("Punch");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            //Debug.Log("Enemy in range");
            punchableEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy") {
            //Debug.Log("Enemy out of range");
            punchableEnemies.Remove(other.gameObject);
        }
    }
}
