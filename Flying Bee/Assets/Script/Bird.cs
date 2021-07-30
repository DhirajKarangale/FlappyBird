using UnityEngine;
using System;


public class Bird : MonoBehaviour
{
    [SerializeField] GameObject WaitToStartPanel;
    [SerializeField] GameObject pauseButtonObject;
    [SerializeField] GameObject scoreTextObject;
    [SerializeField] GameObject pausePannel;
    [SerializeField] AudioSource birdJumpSound;
    [SerializeField] AudioSource hitSound;
    [SerializeField] AudioSource bgSound;
    [SerializeField] AudioSource gameOverSound;
    [SerializeField] AudioSource buttonSound;
    private Rigidbody2D birdRigidBody;
    private Animator animator;
    private const float jumpForce = 4.5f;
    public event EventHandler onDie;
    public event EventHandler onStart;
    public event EventHandler onGameOver;
    public static Bird instace;
    private State state;
    private bool isHitPipe,isPause;
    private int pipeCollision;

    [Header("Camera Shake")]
    private Vector3 cameraInitialPosition;
    private float shakeMagnetude = 0.07f, shakeTime = 0.215f;
    [SerializeField] Camera mainCamera;

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
        bgSound.Play();
        pauseButtonObject.SetActive(false);
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
                    ResumeButton();
                    birdRigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (onStart != null) onStart(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (isPause) ResumeButton();
                    else PauseButton();
                }
                pauseButtonObject.SetActive(true);

              
                if (Input.GetMouseButtonDown(0) && !isHitPipe && !isPause)
                {
                    Jump();
                }
                transform.eulerAngles = new Vector3(0, 0, birdRigidBody.velocity.y * 5f);
                break;
            case State.Dead:
                pauseButtonObject.SetActive(false);
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
                state = State.Dead;
                if (gameOverSound.isPlaying) gameOverSound.Stop();
                gameOverSound.Play();
                bgSound.Stop();
                ShakeIt();
                isHitPipe = true;
                if (hitSound.isPlaying) hitSound.Stop();
                hitSound.Play();
                if (onDie != null) onDie(this, EventArgs.Empty);
                scoreTextObject.SetActive(false);
                pipeCollision = 1;
                Invoke("GameOver", 1f);
            }
            else if (pipeCollision == 1)
            {
                pipeCollision = 2;
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
            state = State.Dead;
            if (pipeCollision == 0)
            {
                if (gameOverSound.isPlaying) gameOverSound.Stop();
                gameOverSound.Play();
                ShakeIt();
            }
            bgSound.Stop();
            isHitPipe = true;
            if(pipeCollision != 2)
            {
                if (hitSound.isPlaying) hitSound.Stop();
                hitSound.Play();
            }
            Invoke("GameOver", 0.7f);
            if (onDie != null) onDie(this, EventArgs.Empty);
       }
    }

    private void GameOver()
    {
        animator.enabled = false;
        if (onGameOver != null) onGameOver(this, EventArgs.Empty);
    }


    private void ShakeIt()
    {
        cameraInitialPosition = mainCamera.transform.position;
        InvokeRepeating("StartCameraShaking", 0f, 0.005f);
        Invoke("StopCameraShaking", shakeTime);
    }

    private void StartCameraShaking()
    {
        float cameraShakingOffsetX = UnityEngine.Random.value * shakeMagnetude * 2 - shakeMagnetude;
        float cameraShakingOffsetY = UnityEngine.Random.value * shakeMagnetude * 2 - shakeMagnetude;
        Vector3 cameraIntermadiatePosition = mainCamera.transform.position;
        cameraIntermadiatePosition.x += cameraShakingOffsetX;
        cameraIntermadiatePosition.y += cameraShakingOffsetY;
        mainCamera.transform.position = cameraIntermadiatePosition;
    }

    private void StopCameraShaking()
    {
        CancelInvoke("StartCameraShaking");
        mainCamera.transform.position = cameraInitialPosition;
    }

    public void PauseButton()
    {
        buttonSound.Play();
        bgSound.Pause();
        scoreTextObject.SetActive(false);
        isPause = true;
        Time.timeScale = 0;
        pausePannel.SetActive(true);
    }

    public void ResumeButton()
    {
        buttonSound.Play();
        bgSound.Play();
        scoreTextObject.SetActive(true);
        isPause = false;
        Time.timeScale = 1;
        pausePannel.SetActive(false);
    }
}
