using UnityEngine;
using TMPro;

public class GameAssets : MonoBehaviour {

    private static GameAssets instance;
    

    public static GameAssets GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }

    [Header("Spritea & Prefabs\n")]
    
    public Sprite pipeHeadSprite;
    public Transform pfPipeHead;
    public Transform pfPipeBody;
    public Transform pfGround;
    public Transform pfClouds;

    // UI's
    [Header("UI's\n")]
    public GameObject pauseMenuUI;
    public GameObject pauseButton;
    public GameObject quitButton;
    public TextMeshProUGUI currentScoreText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject jumpTextAtLevelStart;
    public TextMeshProUGUI highScoreText;    


    //Sounds
    [Header("\nSound and Audio\n")]

    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }


}
