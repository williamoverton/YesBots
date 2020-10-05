using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOtherScene : SceneTrigger
{
    public string sceneName;
    public override void EndScene(){
        SceneManager.UnloadSceneAsync(sceneName);
    }
}