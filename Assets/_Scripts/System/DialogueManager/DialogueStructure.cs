using UnityEngine;

[System.Serializable]

public class DialogueStructure
{
    [SerializeField] private string speakername;
    [SerializeField] [TextArea] private string[] dialogue;

    
    public string SpeakerName => speakername;
    public string[] DialogueTexts => dialogue;

}