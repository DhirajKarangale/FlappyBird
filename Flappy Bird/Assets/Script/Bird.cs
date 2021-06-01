using UnityEngine;
using System;

public class Bird : MonoBehaviour
{
    [SerializeField] GameObject WaitToStartPanel;
    [SerializeField] GameObject scoreTextObject;
    [SerializeField] AudioSource birdJumpSound;
    [SerializeField] AudioSource hitSound;
    private Rigidbody2D birdRigidBody;
    private Animator animator;
    private const int jumpForce = 6;
    public event EventHandler onDie;
    public event EventHandler onStart;
    public event EventHandler onGameOver;
    public static Bird instace;
    private State state;
    private bool isHitPipe;
    private int pipeCollision;

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
        animator = GetComponent<Animator>();
        birdRigidBody.bodyType = RigidbodyType2D.Static;
        state = State.WatingToStart;
        WaitToStartPanel.SetActive(true);
        scoreTextObject.SetActive(true);
        isHitPipe = false;
        animator.enabled = true;
        pipeCollision = 0;
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
                    WaitToStartPanel.SetActive(false);
                    birdRigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStart != null) onStart(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetMouseButtonDown(0) && !isHitPipe)
                {
                    Jump();
                }
                transform.eulerAngles = new Vector3(0, 0, birdRigidBody.velocity.y * 2f);
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
       birdRigidBody.velocity = Vector2.up * jumpForce;
        if (birdJumpSound.isPlaying) birdJumpSound.Stop();
        birdJumpSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Pipe")
        {
            if(pipeCollision == 0)
            {
                isHitPipe = true;
                if (hitSound.isPlaying) hitSound.Stop();
                hitSound.Play();
                transform.eulerAngles = new Vector3(0, 0, birdRigidBody.velocity.y * -900f);
                if (onDie != null) onDie(this, EventArgs.Empty);
                scoreTextObject.SetActive(false);
                pipeCollision = 1;
                Invoke("GameOver", 1f);
            }
            else if(pipeCollision == 1)
            {
                if (hitSound.isPlaying) hitSound.Stop();
                hitSound.Play();
                GameOver();
            }
            else
            {
                GameOver();
            }
        }
       else if(collision.gameObject.tag == "Ground")
       {
            isHitPipe = true;
            if (hitSound.isPlaying) hitSound.Stop();
            hitSound.Play();
            GameOver();
            if (onDie != null) onDie(this, EventArgs.Empty);
       }
    }

    private void GameOver()
    {
        animator.enabled = false;
        if (onGameOver != null) onGameOver(this, EventArgs.Empty);
    }
}
