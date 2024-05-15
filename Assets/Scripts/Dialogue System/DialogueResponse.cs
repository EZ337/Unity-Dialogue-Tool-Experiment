using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class DialogueResponse
{
    [SerializeField, Tooltip("A single line response"), TextArea()]
    private string response;

    [SerializeField, Tooltip("Event fired **AFTER** This response finishes")]
    private UnityEvent responseEvent;

    // Other Helper functions

    /*
    public DialogueResponse(string response)
    {
        this.response = response;
    }
    */


    // public void SetResponse(string newResponse);

}
