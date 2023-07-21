using UnityEngine;

public static class Score
{
    private const string highScorePrefs="highscore";
    public static int GetHighscore() {
        return PlayerPrefs.GetInt(highScorePrefs);
    }

    public static bool TrySetNewHighscore(int score) {
        int currentHighscore = GetHighscore();
        if (score > currentHighscore) {
            PlayerPrefs.SetInt(highScorePrefs, score);
            PlayerPrefs.Save();
            return true;
        } else {
            return false;
        }
    }

    public static void ResetHighscore() {
        PlayerPrefs.SetInt(highScorePrefs, 0);
        PlayerPrefs.Save();
    }
}

