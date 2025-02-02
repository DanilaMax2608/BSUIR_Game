using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] public float interactionDistance = 3f;
    [SerializeField] public KeyCode interactionKey = KeyCode.E;
    [SerializeField] private Image interactionIndicator;
    [SerializeField] private Bag playerBag; // —сылка на мешок игрока

    private bool isDialogueActive = false;

    void Update()
    {
        RaycastHit hit;

        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * interactionDistance, Color.red);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactionDistance))
        {
            IInteractable interactable = hit.transform.GetComponent<IInteractable>();
            IPickable pickable = hit.transform.GetComponent<IPickable>();
            IEatable eatable = hit.transform.GetComponent<IEatable>();
            IBlockable blockable = hit.transform.GetComponent<IBlockable>();

            if (interactable != null || pickable != null || eatable != null)
            {
                if (blockable != null && blockable.IsBlocked)
                {
                    interactionIndicator.enabled = false;
                    return;
                }

                interactionIndicator.enabled = true;

                if (Input.GetKeyDown(interactionKey) && !isDialogueActive)
                {
                    if (interactable != null)
                    {
                        interactable.Interact();
                    }
                    else if (pickable != null)
                    {
                        pickable.PickUp();
                        playerBag.AddObject(hit.transform.gameObject);
                    }
                    else if (eatable != null)
                    {
                        eatable.Eat();
                    }
                }
            }
            else
            {
                interactionIndicator.enabled = false;
            }
        }
        else
        {
            interactionIndicator.enabled = false;
        }
    }

    public void SetDialogueActive(bool active)
    {
        isDialogueActive = active;
    }
}
