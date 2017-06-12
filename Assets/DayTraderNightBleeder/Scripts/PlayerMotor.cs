using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    private Rigidbody2D rb2d;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioClip punchSfx;
    [SerializeField] private AudioClip takeDamageSfx;
    [SerializeField] private AudioClip dieSfx;

    [SerializeField] private float m_MaxSpeedX = 10f;
    [SerializeField] private float m_MaxSpeedY = 6f;
    private bool m_FacingRight = true;
    private int health = 20;

    private List<GameObject> punchableEnemies = new List<GameObject>();

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (isInvulnerable) {
            Color tmp = spriteRenderer.color;
            tmp.a = Mathf.PingPong(Time.time * 3f, 1f);
            spriteRenderer.color = tmp;        
        }
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
        if (isInvulnerable) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.clip = punchSfx;
        audioSource.Play();

        foreach (GameObject victim in punchableEnemies) {
            victim.GetComponent<Enemy>().TakeDamage();
        }

        animator.SetTrigger("Punch");
    }

    private bool isInvulnerable = false;
    public void TakeDamage() {
        if (isInvulnerable) return;

        health--;

        if (health <= 0) {
            Die();
        }
        else {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.clip = takeDamageSfx;
            audioSource.Play();

            StartCoroutine(Invulnerable());
        }
    }

    private IEnumerator Invulnerable() {
        isInvulnerable = true;

        yield return new WaitForSeconds(.5f);

        Color tmp = spriteRenderer.color;
        tmp.a = 1f;
        spriteRenderer.color = tmp;
        isInvulnerable = false;
    }

    public void Die() {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.clip = punchSfx;
        audioSource.Play();

        StartCoroutine(_Die());
    }

    private IEnumerator _Die() {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
        GameManager.Instance.ShowGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            punchableEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy") {
            punchableEnemies.Remove(other.gameObject);
        }
    }

    public void ResetPlayer() {
        health = 20;
        m_FacingRight = true;
    }
}
