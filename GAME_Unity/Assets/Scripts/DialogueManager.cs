using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private List<string> dialogueLines;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private KeyCode nextDialogueKey = KeyCode.Return;
    [SerializeField] private bool destroyAfterDialogue = false;

    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool dialogueActive = false;

    private PlayerMovement playerMovement;
    private PlayerLook playerLook;
    private AudioSource playerAudioSource;
    private PlayerInteraction playerInteraction;

    public delegate void DialogueEndEventHandler();
    public event DialogueEndEventHandler OnDialogueEnd;

    void Start()
    {
        dialogueText.text = "";
        playerInteraction = FindObjectOfType<PlayerInteraction>();
    }

    public void StartDialogue(PlayerMovement playerMovement, PlayerLook playerLook)
    {
        currentLineIndex = 0;
        dialogueActive = true;
        this.playerMovement = playerMovement;
        this.playerLook = playerLook;

        playerMovement.SetInteracting(true); // �������� ��������� ��������
        playerLook.SetInteracting(true); // �������� ��������� ��������� ����� ����

        playerAudioSource = playerMovement.GetComponent<AudioSource>();
        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = false;
        }

        if (playerInteraction != null)
        {
            playerInteraction.SetDialogueActive(true);
        }

        StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(nextDialogueKey))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                currentLineIndex++;
                if (currentLineIndex < dialogueLines.Count)
                {
                    StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private IEnumerator TypeDialogue(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    private void EndDialogue()
    {
        dialogueActive = false;
        dialogueText.text = "";

        playerMovement.SetInteracting(false); // �������� �������� ��������
        playerLook.SetInteracting(false); // �������� �������� ��������� ����� ����

        if (playerAudioSource != null)
        {
            playerAudioSource.enabled = true;
        }

        if (playerInteraction != null)
        {
            playerInteraction.SetDialogueActive(false);
        }

        if (destroyAfterDialogue)
        {
            Destroy(gameObject);
        }

        // �������� ������� ���������� �������
        OnDialogueEnd?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            PlayerLook playerLook = playerMovement.GetComponentInChildren<PlayerLook>();
            playerMovement.SetInteracting(true); // �������� ��������� ��������
            playerLook.SetInteracting(true); // �������� ��������� ��������� ����� ����
            StartDialogue(playerMovement, playerLook);
        }
    }
}
