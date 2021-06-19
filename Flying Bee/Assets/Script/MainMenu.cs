using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    [SerializeField] AudioSource startButtonSound;
    [SerializeField] GameObject bird;
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
        startButtonSound.Play();
        bird.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 245);
        Invoke("LoadGame", 0.8f);
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

    public void MoreGamesButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=DK_Software");
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
