using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D m_rigidBody;
    private Vector3 touchesEnd;

    public float horizontalBoundary;
    public float horizontalSpeed;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        touchesEnd = new Vector3();
        m_rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _CheckBounds();
    }

    private void _Move()
    {
        float direction = 0.0f;

        // simple touch input

        foreach (var touch in Input.touches)
        {
            var worldTouch = Camera.main.ScreenToWorldPoint(touch.position);

            if (worldTouch.x > transform.position.x)
            {
                // direction is positive

                direction = 1.0f;
            }

            if (worldTouch.x < transform.position.x)
            {
                // direction is negative

                direction = -1.0f;
            }

            touchesEnd = worldTouch;
        }

        // keyboard input 

        if (Input.GetAxis("Horizontal") >= 0.1f)
        {
            // direction is positive

            direction = 1.0f;
        }

        if (Input.GetAxis("Horizontal") <= -0.1f)
        {
            // direction is negative

            direction = -1.0f;
        }

        Vector2 newVelocity = m_rigidBody.velocity + new Vector2(direction * horizontalSpeed, 0.0f);
        m_rigidBody.velocity = Vector2.ClampMagnitude(newVelocity, maxSpeed);
        m_rigidBody.velocity *= 0.99f;

        if (touchesEnd.x != 0.0f)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, touchesEnd.x, 0.01f), transform.position.y);
        }
    }

    private void _CheckBounds()
    {
        // check right bounds

        if (transform.position.x >= horizontalBoundary)
        {
            transform.position = new Vector3(horizontalBoundary, transform.position.y, 0.0f);
        }

        // check left bounds

        if (transform.position.x <= -horizontalBoundary)
        {
            transform.position = new Vector3(-horizontalBoundary, transform.position.y, 0.0f);
        }
    }
}
