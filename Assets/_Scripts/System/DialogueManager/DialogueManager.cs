using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : StaticInstance<DialogueManager>
{

    [SerializeField] private DialogueActivator tutorialActivator;

    void Start()
    {
        if (GameManager.Instance.playTime == 1)
        {
            tutorialActivator.ActivateTutorialDialogue();
            GameManager.Instance.isPaulsed = true;
        }
    }

    void Update()
    {
        
    }
}
