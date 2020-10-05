using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

	public bool backwards = false;

	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		GameSceneManager gm = GameObject.FindObjectOfType<GameSceneManager>();
        if(gm){
			if(backwards){
				gm.PreviousScene();
			}else{
				gm.NextScene();
			}
        }
	}
}