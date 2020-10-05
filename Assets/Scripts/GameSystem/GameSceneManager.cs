using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public string[] scenes;
    public string deathScene;
    public int sceneIndex = 0;
    private int lastScene = -1;
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CleanupOldScene()
    {
        SceneTrigger[] triggers = FindObjectsOfType<SceneTrigger>();

        for (int i = 0; i < triggers.Length; i++)
        {
            triggers[i].EndScene();
        }
    }
    // deathScene
    IEnumerator LoadScene(int sceneId)
    {
        CleanupOldScene();

        sceneIndex = sceneId;

        if(scenes[sceneIndex] == ""){
            sceneIndex = 0;
        }

        if (lastScene != -1)
        {
            SceneManager.UnloadSceneAsync(scenes[lastScene]);
        }

        lastScene = sceneId;

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scenes[sceneIndex], LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes[sceneIndex]));
    }

    public void NextScene()
    {
        sceneIndex++;
        CleanupOldScene();
        StartCoroutine(LoadScene(sceneIndex));
    }
    
    public void PreviousScene()
    {
        sceneIndex--;
        CleanupOldScene();
        StartCoroutine(LoadScene(sceneIndex));
    }

    public void LoadDeathScene()
    {
        CleanupOldScene();

        if (lastScene != -1)
        {
            SceneManager.UnloadSceneAsync(scenes[lastScene]);
        }

        GameObject.FindObjectOfType<MenuSounds>().GetComponent<AudioSource>().PlayOneShot(deathSound, StateManager.volume);

        SceneManager.LoadScene(deathScene, LoadSceneMode.Additive);

    }

    public void LeaveDeathScene()
    {
        CleanupOldScene();
        SceneManager.UnloadSceneAsync(deathScene);
        SceneManager.LoadScene(scenes[sceneIndex], LoadSceneMode.Additive);
    }
}
