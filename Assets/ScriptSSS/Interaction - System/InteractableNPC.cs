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

    [Header("Teleport On Interact")]
    [SerializeField] private bool teleportOnInteract = false;
    [SerializeField] private Transform teleportTarget; // vị trí muốn đưa player tới
    [SerializeField] private float teleportYOffset = 0.1f; // tránh kẹt nền

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

        // Teleport if configured
        if (teleportOnInteract && teleportTarget != null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false; // tắt để set position an toàn
                player.transform.SetPositionAndRotation(
                    teleportTarget.position + Vector3.up * teleportYOffset,
                    teleportTarget.rotation);
                if (cc != null) cc.enabled = true;
            }
        }

        //Start Dialogue/Quest System
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