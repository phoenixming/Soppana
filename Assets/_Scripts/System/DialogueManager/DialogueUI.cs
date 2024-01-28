using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{

    public bool IsOpen { get; private set; }

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject speakerName;
    [SerializeField] private TMP_Text DialogueText;
    [SerializeField] private TMP_Text speaker;

    private TMP_Text textLabel;



    [SerializeField] private DialogueEffect dialogueEffect;

    public GameObject DialogueBox => dialogueBox;

    private void Start()
    {
        //dialogueEffect =  GetComponent<DialogueEffect>();

        //CloseDialogueBox();
    }


    public IEnumerator ShowDialogue(Dialogue dialogue)
    {
        if(dialogue.dialogueStructure.DialogueTexts.Length > 0)
        {


            if(dialogue.dialogueStructure.SpeakerName != "")
            {
                speakerName.SetActive(true);
            }
            else
            {
                speakerName.SetActive(false);
            }


            textLabel = DialogueText;
            dialogueBox.SetActive(true);

            IsOpen = true;
        }

        yield return StartCoroutine(routine: StepThroughDialogue(dialogue));
    }



    private IEnumerator StepThroughDialogue (Dialogue dialogue)
    {


        for (int j = 0; j < dialogue.dialogueStructure.DialogueTexts.Length; j++)
        {
            
            speaker.text = dialogue.dialogueStructure.SpeakerName;

            string d = dialogue.dialogueStructure.DialogueTexts[j];

            yield return dialogueEffect.Run(d, textLabel);


            if (j == dialogue.dialogueStructure.DialogueTexts.Length - 1)
                break;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
            
        }

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));

        CloseDialogueBox();   
    }
 


    public void CloseDialogueBox()
    {
        speakerName.SetActive(false);
        dialogueBox.SetActive(false);

        IsOpen = false;

        DialogueText.text = string.Empty;

        speaker.text = string.Empty;
    }
}