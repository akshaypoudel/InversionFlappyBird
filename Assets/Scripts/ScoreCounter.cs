using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private void Awake() 
    {
        scoreText=GameAssets.GetInstance().currentScoreText;
    }

    void Update()
    {
        scoreText.text=Level.GetInstance().GetPipesPassedCount().ToString();
    }
}
