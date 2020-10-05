using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLeaderboardCode : MonoBehaviour
{
    public string publicKey;
    public string privateKey;
    private dreamloLeaderBoard dl;

    // Start is called before the first frame update
    void Start()
    {
        dl = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
        if(!dl) return;
        dl.publicCode = publicKey;
        dl.privateCode = privateKey;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
