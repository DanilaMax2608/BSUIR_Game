using UnityEngine;

public class InteractableDialogueObject : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueManager dialogueManager;

    [System.Obsolete]
    public void Interact()
    {
        if (dialogueManager != null)
        {
            PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
            PlayerLook playerLook = FindObjectOfType<PlayerLook>();

            if (playerMovement != null && playerLook != null)
            {
                dialogueManager.StartDialogue(playerMovement, playerLook);
            }
        }
    }
}
