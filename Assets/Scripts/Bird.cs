using UnityEngine;
using TMPro;
using System.Collections;

public class Bird : MonoBehaviour {

    private const float JUMP_AMOUNT = 90f;
    public float followSmoothness=1f;
    private GameObject gameOverPanel;
    private TextMeshProUGUI gameOverScoreText;
    private GameObject jumpTextAtLevelStart;
    private GameObject pauseMenuUI;
    private TextMeshProUGUI highScoreText;    


    private static Bird instance;
    public static Bird GetInstance()=>instance;

    public enum State{
        Stopped,
        Playing,
        Dead,
        Paused
    }
    public State state {get; private set;}
    public void SetState(State state)=>this.state=state;

    private Rigidbody2D birdRigidbody2D;

    private Vector3 birdPos;

    private void Awake()=>InitializeScript();

    private void InitializeScript()
    {
        InitializeVariables();
        instance = this;
        birdRigidbody2D = GetComponent<Rigidbody2D>();
        birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        gameOverPanel.SetActive(false);
        pauseMenuUI.SetActive(false);
        jumpTextAtLevelStart.SetActive(true);
        state = State.Stopped;
        highScoreText.text=Score.GetHighscore().ToString();
    }
    private void InitializeVariables()
    {
        gameOverPanel=GameAssets.GetInstance().gameOverPanel;
        gameOverScoreText=GameAssets.GetInstance().gameOverScoreText;
        jumpTextAtLevelStart=GameAssets.GetInstance().jumpTextAtLevelStart;
        pauseMenuUI=GameAssets.GetInstance().pauseMenuUI;
        highScoreText=GameAssets.GetInstance().highScoreText;
    }
    private void Update() 
    {
        switch(state)
        {
            default:
            case State.Stopped:
                Time.timeScale=1f;
                Cursor.visible=true;
                BirdStopped();
                break;
            case State.Playing:
                FollowMousePosition();
                if(state!=State.Paused)
                    CheckForESCButtonPressed();
                break;
            case State.Dead:
                Cursor.visible=true;
                break;
            case State.Paused:
                Cursor.visible=true;
                break;
        }
    }
    private void CheckForESCButtonPressed()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            state=State.Paused;
            PauseGame();
        }
    }
    public void PauseGame()
    {
        state=State.Paused;
        Cursor.visible=true;
        pauseMenuUI.SetActive(true);
        Time.timeScale=0f;
    }
    private void BirdStopped()
    {
        if(Input.GetMouseButtonDown(0))
        {
            state=State.Playing;
            jumpTextAtLevelStart.SetActive(false);
        }
    }
    private void FollowMousePosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z=0f;
        mousePosition.x=0f;
        float clampedY = Mathf.Clamp(mousePosition.y,-50,50);
        Vector3 movePos=new Vector3(0,clampedY,0);
        transform.position = Vector3.Lerp(transform.position,movePos,followSmoothness*Time.deltaTime);
        Cursor.visible=false;
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        SoundManager.PlaySound(SoundManager.Sound.Lose,0.5f);
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        state=State.Dead;
        birdRigidbody2D.bodyType=RigidbodyType2D.Static;
        
        yield return new WaitForSeconds(0.5f);
        gameOverPanel.SetActive(true);

        int score=Level.GetInstance().GetPipesPassedCount();
        gameOverScoreText.text="Score : "+score.ToString();
        Score.TrySetNewHighscore(score);

        int highScore=Score.GetHighscore();
        highScoreText.text="HighScore : "+highScore.ToString();
    }
}
