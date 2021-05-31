using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    private List<Pipe> pipeList;
    private int pipeSpwaned;
    private float pipeSpwanTimer;
    private float pipeSpwanTimerMax;
    private float pipeSpwanXPosition = 3;
    private float gapSize;
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
    }

    private void Awake()
    {
        pipeSpwanTimerMax = 1.1f;
        pipeList = new List<Pipe>();
        SetDifficuilty(Difficulty.Easy);
    }

    private void Update()
    {
        PipeMovement();
        PipeSpwaning();
    }

    private void PipeSpwaning()
    {
        pipeSpwanTimer -= Time.deltaTime;
        if(pipeSpwanTimer<=0)
        {
            pipeSpwanTimer += pipeSpwanTimerMax;
            float minHeight = gapSize * 0.5f + 2;
            float maxHeight = 10 - (gapSize * 0.5f) - 2;
            float height = Random.Range(minHeight, maxHeight);
            PipeGap(height, gapSize, pipeSpwanXPosition);
        }
    }

    private void PipeMovement()
    {
        for(int i=0;i<pipeList.Count;i++)
        {
            Pipe pipe = pipeList[i];
            pipe.Move();
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
        if (pipeSpwaned >= 40) return Difficulty.Impossible;
        if (pipeSpwaned >= 25) return Difficulty.Hard;
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

        Pipe pipe1 = new Pipe(pipe);
        pipeList.Add(pipe1);
    }

    private class Pipe
    {
        private Transform pipeTransform;

        public Pipe(Transform pipeTransform)
        {
            this.pipeTransform = pipeTransform;
        }

        public void Move()
        {
            pipeTransform.position += new Vector3(-1, 0, 0) * 2.8f * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeTransform.position.x;
        }

        public void DestroySelf()
        {
            Destroy(pipeTransform.gameObject);
        }
    }
}
