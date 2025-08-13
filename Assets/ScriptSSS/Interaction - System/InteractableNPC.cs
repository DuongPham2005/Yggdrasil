using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptSSS.Quests;

[RequireComponent(typeof(Collider))]
public class InteractableNPC : Interactable
{
    private Animator animator;
    [Header("Main Quest")]
    [SerializeField] string npcId = "village"; // "village" or "castle"

    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    protected override void Interaction()
    {
        base.Interaction();
        Debug.Log($"InteractableNPC: Player interacted with NPC '{npcId}'");
        print("Hello! Unfortunately I don't have a dialog system yet.");
        animator.SetTrigger("Wave");

        //Start Dialogue System
        if (MainQuestManager.Instance != null)
        {
            MainQuestManager.Instance.OnNPCInteracted(npcId);
        }
        else
        {
            Debug.LogWarning("MainQuestManager not found in scene. Add MainQuestManager to a GameObject to progress main quest.");
        }
    }
}