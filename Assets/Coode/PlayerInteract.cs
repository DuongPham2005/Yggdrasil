using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PlayerInteract : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach(Collider collider in colliderArray){
                if(collider.TryGetComponent(out NPCInteract npcInteract)){
                    npcInteract.Interact();
                }

            }
        }
    }
}
