using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] GameObject gameOverWindow;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;

    private void Awake()
    {
        gameOverWindow.SetActive(false);
    }

    private void Start()
    {
        Bird.instace.onDie += OnDey;
    }

    private void OnDey(object sender,System.EventArgs eventArgs)
    {
        scoreText.text = level.GetPipePassed().ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        gameOverWindow.SetActive(true);
    }

    public void RestartButton()
    {
        // Loader.Load(Loader.Scene.Game);
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
