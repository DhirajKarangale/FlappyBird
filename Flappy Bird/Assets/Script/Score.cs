using UnityEngine.UI;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] Text scoreText;
    private int highScore;

    private void Awake()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    private void Update()
    {
        scoreText.text = level.GetPipePassed().ToString();
        if(level.GetPipePassed() > highScore)
        {
            highScore = level.GetPipePassed();
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
