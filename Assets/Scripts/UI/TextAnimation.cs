using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAnimation : MonoBehaviour
{

    public float letterPause = 0.2f;

    string message;
    TextMesh textComp;

    // Use this for initialization
    void Start()
    {
        textComp = GetComponent<TextMesh>();
        message = textComp.text;
        textComp.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            textComp.text += letter;

            if(letter.ToString() == "\n"){
                yield return new WaitForSeconds(0.9f);
            }
        }
    }
}