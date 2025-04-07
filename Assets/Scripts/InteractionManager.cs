using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [Header("RAY SETTINGS")]
    [SerializeField] private float rayLength = 3.0f; // Max length of the ray
    [SerializeField] private LayerMask targetLayer; // Layer the ray can detect object
    [SerializeField] private TextMeshProUGUI promptText; // Reference to the text for displaying prompt

    private void Update()
    {
        SelectObject();
    }

    private void SelectObject()
    {
        promptText.text = " ";
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength, targetLayer))
        {
             promptText.text = hit.transform.name + "\n[E]";

            if (!Input.GetKeyDown(KeyCode.E)) return;

            if (hit.transform.TryGetComponent<Pickable>(out Pickable pickable))
            {
                // Call the interact method if the object hit is in pickable
                pickable.Interact();
            }
        }
    }
}
