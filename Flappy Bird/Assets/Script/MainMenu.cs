using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource quitButtonSound;
    [SerializeField] AudioSource startButtonSound;
    public void StartButton()
    {
        startButtonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        quitButtonSound.Play();
        Application.Quit();
    }
}
