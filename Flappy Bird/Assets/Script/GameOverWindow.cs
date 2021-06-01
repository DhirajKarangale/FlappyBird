using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] Level level;
    [SerializeField] GameObject gameOverWindow;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] AudioSource buttonSound;

    private void Awake()
    {
        gameOverWindow.SetActive(false);
    }

    private void Start()
    {
        Bird.instace.onGameOver += OnDey;
    }

    private void OnDey(object sender,System.EventArgs eventArgs)
    {
        scoreText.text = level.GetPipePassed().ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        gameOverWindow.SetActive(true);
    }

    public void RestartButton()
    {
        buttonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void MainMenuButton()
    {
        buttonSound.Play();
        SceneManager.LoadScene(0);
    }
    public void QuitButton()
    {
        buttonSound.Play();
        Application.Quit();
    }
}
