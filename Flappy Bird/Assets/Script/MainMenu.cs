using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;

    public void StartButton()
    {
        buttonSound.Play();
        SceneManager.LoadScene(1);
    }

    public void QuitButton()
    {
        buttonSound.Play();
        Application.Quit();
    }
}
