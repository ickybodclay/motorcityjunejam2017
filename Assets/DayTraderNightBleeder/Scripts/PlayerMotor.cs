using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    private Rigidbody2D rigidbody2d;

    [SerializeField] private float m_MaxSpeedX = 10f;
    [SerializeField] private float m_MaxSpeedY = 6f;
    private bool m_FacingRight = true;

    private void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Move(float moveX, float moveY) {
        rigidbody2d.velocity = new Vector2(moveX * m_MaxSpeedX, moveY * m_MaxSpeedY);

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
}
