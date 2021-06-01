using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    private List<Pipe> pipeList;
    private int pipePassedCount;
    private int pipeSpwaned;
    private float pipeSpwanTimer;
    private float pipeSpwanTimerMax;
    private float pipeSpwanXPosition = 4;
    private float gapSize;
    private State state;
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
                gapSize = 5;
                pipeSpwanTimerMax = 1.3f;
                break;
            case Difficulty.Medium:
                gapSize = 4.3f;
                pipeSpwanTimerMax = 1.15f;
                break;
            case Difficulty.Hard:
                gapSize = 3.4f;
                pipeSpwanTimerMax = 1f;
                break;
            case Difficulty.Impossible:
                gapSize = 2.45f;
                pipeSpwanTimerMax = 0.9f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipeSpwaned >= 35) return Difficulty.Impossible;
        if (pipeSpwaned >= 24) return Difficulty.Hard;
        if (pipeSpwaned >= 10) return Difficulty.Medium;
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
        Transform pipe = Instantiate(GameAssets.instance.pipe);
        if (creatBottom)
        {
            pipe.position = new Vector2(xPosition, -5);
            pipe.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            pipe.position = new Vector2(xPosition, 5);
            pipe.rotation = Quaternion.Euler(180, 0, 0);
        }

        SpriteRenderer pipeSpriteRenderer = pipe.GetComponent<SpriteRenderer>();
        pipeSpriteRenderer.size = new Vector2(0.75f, height);
        BoxCollider2D pipeBoxCollider2D = pipe.GetComponent<BoxCollider2D>();
        pipeBoxCollider2D.offset = new Vector2(0, (height * 0.5f));
        pipeBoxCollider2D.size = new Vector2(0.75f, height);

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
            pipeTransform.position += new Vector3(-1, 0, 0) * 2.8f * Time.deltaTime;
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
