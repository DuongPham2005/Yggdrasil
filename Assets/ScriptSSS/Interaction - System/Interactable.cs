using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Data")]
    public string interactableName = "";
    public float interactionDistance = 2;
    [SerializeField] bool isInteractable = true;

    InteractableNameText interactableNameText;
    GameObject interactableNameCanvas;

    public virtual void Start()
    {
		interactableNameCanvas = GameObject.FindGameObjectWithTag("Canvas");
		if (interactableNameCanvas != null)
		{
			interactableNameText = interactableNameCanvas.GetComponentInChildren<InteractableNameText>();
		}
		// Fallback: tìm trực tiếp trong scene nếu Canvas không gắn tag hoặc không có component
		if (interactableNameText == null)
		{
			interactableNameText = FindObjectOfType<InteractableNameText>();
		}
		if (interactableNameText == null)
		{
			Debug.LogWarning("InteractableNameText not found in scene. Please place it under a Canvas and tag the Canvas as 'Canvas'.");
		}
    }

    public void TargetOn()
    {
		if (interactableNameText == null) return;
		interactableNameText.ShowText(this);
		interactableNameText.SetInteractableNamePosition(this);
    }

    public void TargetOff()
    {
		if (interactableNameText == null) return;
		interactableNameText.HideText();
    }

    public void Interact()
    {
        if (isInteractable) Interaction();
    }

    protected virtual void Interaction()
    {
        //print("interact with: " + this.name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
    private void OnDestroy()
    {
        TargetOff();
    }
}