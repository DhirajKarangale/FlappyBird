using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
    private Rigidbody2D birdRigidBody;
    private const int jumpForce = 16;
    public event EventHandler onDie;
    public event EventHandler onStart;
    public static Bird instace;
    private State state;

    private enum State
    {
        WatingToStart,
        Playing,
        Dead,
    }

    private void Awake()
    {
        if(instace == null)
        {
            instace = this;
        }

        birdRigidBody = GetComponent<Rigidbody2D>();
        birdRigidBody.bodyType = RigidbodyType2D.Static;
        state = State.WatingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WatingToStart:
                if (Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdRigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStart != null) onStart(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
       birdRigidBody.velocity = Vector2.up * jumpForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdRigidBody.bodyType = RigidbodyType2D.Static;
        if (onDie != null) onDie(this, EventArgs.Empty);
    }
}
