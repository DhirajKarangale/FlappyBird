using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    [SerializeField] GameObject quitPanel;
    [SerializeField] GameObject startbutton;
    [SerializeField] GameObject quitbutton;
    private bool isQuitPanel;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (isQuitPanel) NoButton();
            else QuitPanel();
        }
    }

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

    public void QuitPanel()
    {
        buttonSound.Play();
        isQuitPanel = true;
        quitPanel.SetActive(true);
        quitbutton.SetActive(false);
        startbutton.SetActive(false);
    }

    public void NoButton()
    {
        buttonSound.Play();
        isQuitPanel = false;
        quitPanel.SetActive(false);
        quitbutton.SetActive(true);
        startbutton.SetActive(true);
    }
}
