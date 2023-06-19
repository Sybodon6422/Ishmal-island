using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsoleBox : MonoBehaviour
{
    [SerializeField] GameObject textBoxPiece;
    [SerializeField] Transform textHolder;
    List<GameObject> messageList = new List<GameObject>();

    void Start()
    {
        
    }

    public void PrintToConsole(string _text)
    {
        if(messageList.Count > 8) { 
            Destroy(messageList[0]);
            messageList.RemoveAt(0);
        }
        var go = Instantiate(textBoxPiece, textHolder);
        go.GetComponent<TextMeshProUGUI>().text = _text;
        messageList.Add(go);
    }
}
