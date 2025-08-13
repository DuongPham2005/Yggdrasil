using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Interactor : MonoBehaviour
{
    [SerializeField] float maxInteractingDistance = 10;
    [SerializeField] float interactingRadius = 1;
    [SerializeField] LayerMask detectionLayers; // optional override; if 0 -> all layers
    [SerializeField] KeyCode fallbackInteractKey = KeyCode.E; // fallback if PlayerInput/Action missing

    LayerMask layerMask;
    Transform cameraTransform;
    InputAction interactAction;

    //For Gizmo
    Vector3 origin;
    Vector3 direction;
    Vector3 hitPosition;
    float hitDistance;

    [HideInInspector] public Interactable interactableTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        layerMask = detectionLayers.value == 0 ? ~0 : detectionLayers; // default: all layers, filter by component later

        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            var actionAsset = playerInput.actions;
            if (actionAsset != null)
            {
                var action = actionAsset.FindAction("Interact", throwIfNotFound: false);
                if (action != null)
                {
                    interactAction = action;
                    interactAction.performed += Interact;
                }
                else
                {
                    Debug.LogWarning("InputAction 'Interact' not found in PlayerInput actions. Fallback to KeyCode.E.");
                }
            }
            else
            {
                Debug.LogWarning("PlayerInput has no actions asset. Fallback to KeyCode.E.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerInput component not found. Fallback to KeyCode.E.");
        }
    }
    // Update is called once per frame
    void Update()
    {
        direction = cameraTransform.forward;
        origin = cameraTransform.position;
        RaycastHit hit;

        if (Physics.SphereCast(origin, interactingRadius, direction, out hit, maxInteractingDistance, layerMask))
        {
            hitPosition = hit.point;
            hitDistance = hit.distance;
            if (hit.transform.TryGetComponent<Interactable>(out interactableTarget))
            {
                interactableTarget.TargetOn();
            }
        }
        else if (interactableTarget)
        {
            interactableTarget.TargetOff();
            interactableTarget = null;
        }

        // Fallback key support
        if (Input.GetKeyDown(fallbackInteractKey))
        {
            PerformInteract();
        }
    }
    private void Interact(InputAction.CallbackContext obj)
    {
        PerformInteract();
    }

    private void PerformInteract()
    {
        if (interactableTarget != null)
        {
            if (Vector3.Distance(transform.position, interactableTarget.transform.position) <= interactableTarget.interactionDistance)
            {
                interactableTarget.Interact();
            }
        }
        else
        {
            // Optional debug
            // print("nothing to interact!");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + direction * hitDistance);
        Gizmos.DrawWireSphere(hitPosition, interactingRadius);
    }
    private void OnDestroy()
    {
        interactAction.performed -= Interact;
    }
}