using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    private InputField input;
    public GameObject playButton;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputField>();
        input.onValueChanged.AddListener (delegate {ValueChangeCheck ();});
    }

    private void ValueChangeCheck(){
        StateManager.playerName = input.text;

        playButton.SetActive(StateManager.playerName.Length > 3 && StateManager.playerName.Length < 14);
    }
}
