using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue")]

public class Dialogue : ScriptableObject
{
    [SerializeField] private DialogueStructure dialogue;
    [SerializeField] private bool activateOnStart;


    public DialogueStructure dialogueStructure => dialogue;
    public bool ActivateOnStart => activateOnStart;
}