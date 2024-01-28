using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivator : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogue;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private List<GameObject> tutorialInstructions;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateDialogue()
    {
        //dialogueUI.ShowDialogue(dialogue);
    }

    public void ActivateTutorialDialogue()
    {
        StartCoroutine(ActivateTutorialDialogue1());
    }


    public IEnumerator ActivateTutorialDialogue1()
    {
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[0]));
        StartCoroutine(ActivateTutorialDialogue2());
    }

    public IEnumerator ActivateTutorialDialogue2()
    {
        GameManager.Instance.isTutorialPhase1 = false;
        GameManager.Instance.isTutorialPhase2 = true;
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[1]));
        StartCoroutine(ActivateTutorialDialogue3());
    }

    public IEnumerator ActivateTutorialDialogue3()
    {
        GameManager.Instance.isTutorialPhase2 = false;
        GameManager.Instance.isTutorialPhase3 = true;
        tutorialInstructions[0].SetActive(true);
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[2]));
        StartCoroutine(ActivateTutorialDialogue4());
    }

    public IEnumerator ActivateTutorialDialogue4()
    {
        GameManager.Instance.isTutorialPhase3 = false;
        GameManager.Instance.isTutorialPhase4 = true;
        tutorialInstructions[0].SetActive(false);
        tutorialInstructions[1].SetActive(true);
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[3]));
        StartCoroutine(ActivateTutorialDialogue5());
    }

    public IEnumerator ActivateTutorialDialogue5()
    {
        GameManager.Instance.isTutorialPhase4 = false;
        GameManager.Instance.isTutorialPhase5 = true;
        tutorialInstructions[1].SetActive(false);
        tutorialInstructions[2].SetActive(true);
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[4]));
        StartCoroutine(ActivateTutorialDialogue6());
    }

    public IEnumerator ActivateTutorialDialogue6()
    {
        GameManager.Instance.isTutorialPhase5 = false;
        GameManager.Instance.isTutorialPhase6 = true;
        tutorialInstructions[2].SetActive(false);
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[5]));
        StartCoroutine(ActivateTutorialDialogue7());
    }

    public IEnumerator ActivateTutorialDialogue7()
    {
        GameManager.Instance.isTutorialPhase6 = false;
        yield return StartCoroutine(dialogueUI.ShowDialogue(dialogue[6]));
        GameManager.Instance.isPaulsed = false;
        GameManager.Instance.isTutorial = false;
        //GameManager.Instance.karmaScore[0] = 0;
        if (UIManager.Instance.FruitList.Count != 0)
        {
            UIManager.Instance.RemoveInventory(tutorialInstructions[1].GetComponentInChildren<FruitPrefab>());
        }
    }
}
