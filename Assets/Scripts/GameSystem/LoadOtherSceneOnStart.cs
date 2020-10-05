using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadOtherSceneOnStart : MonoBehaviour
{
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
}
