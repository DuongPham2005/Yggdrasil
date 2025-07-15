// using UnityEngine;

// public class QuestCompletionTrigger : MonoBehaviour
// {
//     public Quest questToCheck;

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             if (!questToCheck.isCompleted && questToCheck.currentAmount >= questToCheck.requiredAmount)
//             {
//                 questToCheck.isCompleted = true;
//                 Debug.Log($"Quest {questToCheck.questName} hoàn thành!");
//             }
//         }
//     }
// }
