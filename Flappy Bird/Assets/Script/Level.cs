using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    private List<Transform> groundList;
    private List<Transform> groundListUp;
    private List<Pipe> pipeList;
    private int pipePassedCount;
    private int pipeSpwaned;
    private float pipeSpwanTimer;
    private float pipeSpwanTimerMax;
    private float pipeSpwanXPosition = 4;
    private float gapSize;
    private State state;
    [SerializeField] AudioSource scoreSound;
    public Transform pipeTr;
    public Transform ground;
    public Transform groundUp;
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private enum State
    {
        WatingToStart,
        Playing,
        Dead,
    }

    private void Awake()
    {
        state = State.WatingToStart;
        pipeSpwanTimerMax = 1.1f;
        pipeList = new List<Pipe>();
        groundList = new List<Transform>();
        groundListUp = new List<Transform>();
        SpwanInitialGround(true);
      //  SpwanInitialGround(false);
        SetDifficuilty(Difficulty.Easy);
    }

    private void Start()
    {
        Bird.instace.onDie += BirdDye;
        Bird.instace.onStart += OnStart;
    }

    public void BirdDye(object sender,System.EventArgs eventArgs)
    {
        state = State.Dead;
    }

    public void OnStart(object sender, System.EventArgs eventArgs)
    {
        state = State.Playing;
    }

    private void Update()
    {
        if(state == State.Playing)
        {
            PipeMovement();
            PipeSpwaning();
            Ground();
            GroundUp();
        }
    }

    private void SpwanInitialGround(bool bottom)
    {
        
        Transform groundTransform;
        groundTransform = Instantiate(ground, new Vector3(0, -5f, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(ground, new Vector3(5.742f, -5f, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(ground, new Vector3(11.484f, -5f, 0), Quaternion.identity);
        groundList.Add(groundTransform);
        groundTransform = Instantiate(groundUp, new Vector3(0, 5, 0), Quaternion.Euler(180, 0, 0));
        groundListUp.Add(groundTransform);
        groundTransform = Instantiate(groundUp, new Vector3(5.742f, 5f, 0), Quaternion.Euler(180, 0, 0));
        groundListUp.Add(groundTransform);
        groundTransform = Instantiate(groundUp, new Vector3(11.484f, 5f, 0), Quaternion.Euler(180, 0, 0));
        groundListUp.Add(groundTransform);
    }

    private void Ground()
    {
        foreach(Transform groundTransform in groundList)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * 1.2f * Time.deltaTime;
            if(groundTransform.position.x < -6)
            {
                float rightMousePosition = -10;
                for(int i=0;i<groundList.Count;i++)
                {
                   if(groundList[i].position.x > rightMousePosition)
                   {
                       rightMousePosition = groundList[i].position.x;
                   }
                }

                float groundWeidhtHalf = 5.7f;
                groundTransform.position = new Vector3(rightMousePosition + groundWeidhtHalf, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }


    private void GroundUp()
    {
        foreach (Transform groundTransform in groundListUp)
        {
            groundTransform.position += new Vector3(-1, 0, 0) * 1.2f * Time.deltaTime;
            if (groundTransform.position.x < -6)
            {
                float rightMousePosition = -10;
                for (int i = 0; i < groundListUp.Count; i++)
                {
                    if (groundListUp[i].position.x > rightMousePosition)
                    {
                        rightMousePosition = groundListUp[i].position.x;
                    }
                }

                float groundWeidhtHalf = 5.7f;
                groundTransform.position = new Vector3(rightMousePosition + groundWeidhtHalf, groundTransform.position.y, groundTransform.position.z);
            }
        }
    }



    private void PipeSpwaning()
    {
        pipeSpwanTimer -= Time.deltaTime;
        if(pipeSpwanTimer<=0)
        {
            pipeSpwanTimer += pipeSpwanTimerMax;
            float minHeight = gapSize * 0.5f + 0;
            float maxHeight = 10 - (gapSize * 0.5f) -1;
            float height = Random.Range(minHeight, maxHeight);
            PipeGap(height, gapSize, pipeSpwanXPosition);
        }
    }

    private void PipeMovement()
    {
        for(int i=0;i<pipeList.Count;i++)
        {
            Pipe pipe = pipeList[i];
            bool isPipeRightToBird = pipe.GetXPosition() > 0;
            pipe.Move();
            if(isPipeRightToBird && (pipe.GetXPosition() <= 0) && pipe.IsBottom())
            {
                pipePassedCount++;
                if (scoreSound.isPlaying) scoreSound.Stop();
                scoreSound.Play();
            }
            if (pipe.GetXPosition() < -3.75f)
            {
                pipe.DestroySelf();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void SetDifficuilty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 5f;
                pipeSpwanTimerMax = 4f;
                break;
            case Difficulty.Medium:
                gapSize = 4f;
                pipeSpwanTimerMax = 3f;
                break;
            case Difficulty.Hard:
                gapSize = 3f;
                pipeSpwanTimerMax = 2f;
                break;
            case Difficulty.Impossible:
                gapSize = 2f;
                pipeSpwanTimerMax = 1f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipeSpwaned >= 27) return Difficulty.Impossible;
        if (pipeSpwaned >= 15) return Difficulty.Hard;
        if (pipeSpwaned >= 5) return Difficulty.Medium;
        else return Difficulty.Easy;
    }

    private void PipeGap(float gapY,float gapSize,float xPosition)
    {
        CreatPipe(gapY - gapSize * 0.5f, xPosition,true);
        CreatPipe(5 * 2 - gapY - gapSize * 0.5f, xPosition, false);
        pipeSpwaned++;
        SetDifficuilty(GetDifficulty());
    }

    private void CreatPipe(float height, float xPosition, bool creatBottom)
    {
        Transform pipe = Instantiate(pipeTr);
        if (creatBottom)
        {
            pipe.position = new Vector2(xPosition, -4.012f);
            pipe.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            pipe.position = new Vector2(xPosition, 5f);
            pipe.rotation = Quaternion.Euler(180, 0, 0);
        }

        SpriteRenderer pipeSpriteRenderer = pipe.GetComponent<SpriteRenderer>();
        pipeSpriteRenderer.size = new Vector2(0.5f, height);
        BoxCollider2D pipeBoxCollider2D = pipe.GetComponent<BoxCollider2D>();
        pipeBoxCollider2D.offset = new Vector2(0, (height * 0.5f));
        pipeBoxCollider2D.size = new Vector2(0.5f, height);

        Pipe pipe1 = new Pipe(pipe , creatBottom);
        pipeList.Add(pipe1);
    }

    public int GetPipePassed()
    {
        return pipePassedCount;
    }

    private class Pipe
    {
        private Transform pipeTransform;
        private bool isBottom;

        public Pipe(Transform pipeTransform,bool isBottom)
        {
            this.pipeTransform = pipeTransform;
            this.isBottom = isBottom;
        }

        public void Move()
        {
            pipeTransform.position += new Vector3(-1, 0, 0) * 1.2f * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeTransform.position.x;
        }

        public bool IsBottom()
        {
            return isBottom;
        }

        public void DestroySelf()
        {
            Destroy(pipeTransform.gameObject);
        }
    }
}