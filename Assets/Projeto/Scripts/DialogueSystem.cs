using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueBox;
    public TMP_Text dialogueText;
    public string[] dialogueLines;
    public float textSpeed = 0.05f;
    public AudioSource buttonSound;
    private int currentLine = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Vector3 offScreenPosition;
    private Vector3 onScreenPosition;

    public GameObject robotBox;
    public RobotManager robotManager;

    private bool playerInRange = false;

    void Start()
    {
        dialogueBox.SetActive(false);
        offScreenPosition = new Vector3(dialogueBox.transform.position.x, -Screen.height, dialogueBox.transform.position.z);
        onScreenPosition = dialogueBox.transform.position;
        dialogueBox.transform.position = offScreenPosition;
        robotBox.transform.position = offScreenPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDialogueActive && playerInRange)
            {
                StartDialogue();
            }
            else if (isDialogueActive && !isTyping)
            {
                NextDialogueLine();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueBox.SetActive(true);
        StartCoroutine(AnimateBoxOpen());
    }

    IEnumerator AnimateBoxOpen()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 startPos = offScreenPosition;
        Vector3 endPos = onScreenPosition;

        while (elapsedTime < duration)
        {
            dialogueBox.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dialogueBox.transform.position = endPos;
        StartCoroutine(TypeText());
    }

    IEnumerator AnimateRobotBoxOpen()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector2 startPos = new Vector2(0, 200);
        Vector2 endPos = new Vector2(200, 200);

        while (elapsedTime < duration)
        {
            robotBox.transform.position = Vector2.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        robotBox.transform.position = endPos;
    }

    void NextDialogueLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            StartCoroutine(TypeText());
        }
        else
        {
            StartCoroutine(AnimateBoxClose());
            StartCoroutine(AnimateRobotBoxOpen());
        }
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLine])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    IEnumerator AnimateBoxClose()
    {
        float elapsedTime = 0f;
        float duration = 0.5f;
        Vector3 startPos = onScreenPosition;
        Vector3 endPos = offScreenPosition;

        while (elapsedTime < duration)
        {
            dialogueBox.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dialogueBox.transform.position = endPos;
        dialogueBox.SetActive(false);
        isDialogueActive = false;
        currentLine = 0;
    }
}