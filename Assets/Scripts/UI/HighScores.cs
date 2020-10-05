using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    private dreamloLeaderBoard dl;
    private TextMesh text;
    // Start is called before the first frame update
    void Start()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();

        text = GetComponent<TextMesh>();
        StartCoroutine(WaitForScores());
    }

    IEnumerator WaitForScores(){
        while(dl.isWaitingForScores){
            yield return new WaitForSeconds(0.1f);
        }

        List<dreamloLeaderBoard.Score> scoreList = dl.ToListLowToHigh();

        Debug.Log($"Scores: {scoreList.Count}");

        text.text = "";

        for(int i = 0 ; i < (scoreList.Count > 10 ? 10 : scoreList.Count); i++){
            text.text += $"<color=white>{scoreList[i].playerName}</color> - {scoreList[i].score / 100f} Seconds\n";
        }
    }
}
