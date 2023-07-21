using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadLevel : MonoBehaviour
{
    private GameObject pauseMenuUI;
    private GameObject pauseButton;
    private GameObject quitButton;
    private bool isGameMode=true;

    private void Awake() 
    {
        pauseMenuUI=GameAssets.GetInstance().pauseMenuUI;
        pauseButton=GameAssets.GetInstance().pauseButton;
        quitButton=GameAssets.GetInstance().quitButton;
    }
    private void Start() 
    {
        if(SceneManager.GetActiveScene().buildIndex==0)
        {
            isGameMode=!isGameMode;
        }

        if(Application.platform==RuntimePlatform.WebGLPlayer)
        {
            quitButton.SetActive(false);
        }

        if(!isGameMode) return;
        pauseButton.SetActive(false);
    }
    private void Update() 
    {
        if(!isGameMode) return;

        if(Bird.GetInstance().state==Bird.State.Playing)
        {
            pauseButton.SetActive(true);
        }
        else{
            pauseButton.SetActive(false);
        }
    }
    public void LevelLoad(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void Resume()
    {
        Bird.GetInstance().SetState(Bird.State.Playing);
        Time.timeScale=1f;
        pauseMenuUI.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Pause()
    {
        Bird.GetInstance().PauseGame();
    }
}
