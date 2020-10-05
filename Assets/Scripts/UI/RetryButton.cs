using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour {


	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		GameSceneManager gm = GameObject.FindObjectOfType<GameSceneManager>();
        if(gm){
            gm.LeaveDeathScene();
        }
	}
}