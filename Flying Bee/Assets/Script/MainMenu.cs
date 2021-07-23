using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] AudioSource buttonSound;
    [SerializeField] AudioSource startButtonSound;
    [SerializeField] GameObject bird;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject quitPanel;
    [SerializeField] GameObject aboutPanel;
    [SerializeField] GameObject startbutton;
    [SerializeField] GameObject quitbutton;
    [SerializeField] GameObject aboutbutton;
    private bool isQuitPanel,isAboutPanel;

    private void Update()
    {
        if (!isAboutPanel && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isQuitPanel) NoButton();
            else QuitPanel();
        }

        if(isAboutPanel && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAboutButton();
        }
    }

    public void StartButton()
    {
        startButtonSound.Play();
        bird.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 245);
        Invoke("LoadGame", 0.6f);
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
        aboutbutton.SetActive(false);
    }

    public void NoButton()
    {
        buttonSound.Play();
        isQuitPanel = false;
        quitPanel.SetActive(false);
        quitbutton.SetActive(true);
        startbutton.SetActive(true);
        aboutbutton.SetActive(true);
    }

    public void MoreGamesButton()
    {
        buttonSound.Play();
        Application.OpenURL("https://play.google.com/store/apps/developer?id=DK_Software");
    }

    private void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void AboutButton()
    {
        buttonSound.Play();
        isAboutPanel = true;
        mainPanel.SetActive(false);
        aboutPanel.SetActive(true);
    }

    public void LinkedInButton()
    {
        buttonSound.Play();
        Application.OpenURL("https://www.linkedin.com/in/dhiraj-karangale-464ab91bb");
    }

    public void YoutubeButton()
    {
        buttonSound.Play();
        Application.OpenURL("https://www.youtube.com/channel/UC_Dnn-QqlnrdYpKXycyzJDA");
    }

    public void CloseAboutButton()
    {
        buttonSound.Play();
        isAboutPanel = false;
        aboutPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
