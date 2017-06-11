using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;
    private Seeker seeker;

    public Transform targetPosition;

    //[SerializeField] private AudioClip hitSfx;

    // The calculated path
    public Path path;
    // The AI's speed in meters per second
    public float speed = 2;
    // The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    // The waypoint we are currently moving towards
    private int currentWaypoint = 0;
    // How often to recalculate the path (in seconds)
    public float repathRate = 0.5f;
    private float lastRepath = -9999;

    public bool m_FacingRight = true;

    public AudioClip takeDamageSfx;
    public AudioClip dieSfx;

    private int health = 3;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
    }

    private void Update() {
        if (isDying) {
            Color tmp = spriteRenderer.color;
            tmp.a = Mathf.PingPong(Time.time * 10f, 1f);
            spriteRenderer.color = tmp;

            return;
        }

        if (Time.time - lastRepath > repathRate && seeker.IsDone()) {
            lastRepath = Time.time + Random.value * repathRate * 0.5f;
            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

            float deltaX = transform.position.x - targetPosition.position.x;

            if (deltaX < 0 && !m_FacingRight) {
                Flip();
            }
            else if (deltaX > 0 && m_FacingRight) {
                Flip();
            }
        }
        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }
        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count) {
            //Debug.Log("End Of Path Reached");
            currentWaypoint++;
            return;
        }
        // Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed;
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        Move(dir);
        // The commented line is equivalent to the one below, but the one that is used
        // is slightly faster since it does not have to calculate a square root
        //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
        if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }

    public void Move(Vector3 dir) {
        if (isStunned) return;

        dir.z = 0;
        rb2d.velocity = dir;
        animator.SetFloat("Speed", rb2d.velocity.magnitude);
    }

    private float hitForce = 150f;
    private bool isStunned = false;
    public void TakeDamage() {
        //Debug.Log(name + " hit!");
        Vector3 dir = transform.position - targetPosition.position;
        dir = dir.normalized * hitForce;
        rb2d.AddForce(dir, ForceMode2D.Impulse);
        health--;

        if (health <= 0) {
            Die();
        }
        else {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.clip = takeDamageSfx;
            audioSource.Play();

            StartCoroutine(Stun());
        }
    }

    private IEnumerator Stun() {
        isStunned = true;

        yield return new WaitForSeconds(.5f);

        isStunned = false;
    }

    private bool isDying = false;
    private void Die() {
        if (isDying) return;

        isDying = true;
        GameManager.Instance.EnemyCount -= 1;

        StartCoroutine(_Die());
    }

    private IEnumerator _Die() {

        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    public void OnPathComplete(Path p) {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    private void Flip() {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Attack();
        }
    }

    private void Attack() {
        if (isStunned) return;

        animator.SetTrigger("Punch");
    }
}
