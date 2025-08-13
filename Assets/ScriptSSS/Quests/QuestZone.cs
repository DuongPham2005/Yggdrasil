using UnityEngine;

namespace ScriptSSS.Quests
{
	[RequireComponent(typeof(Collider))]
	public class QuestZone : MonoBehaviour
	{
		[SerializeField] string zoneId = "dungeon_exit"; // or "castle_gate"
		[SerializeField] string playerTag = "Player";

		void Reset()
		{
			var col = GetComponent<Collider>();
			col.isTrigger = true;
		}

		void OnTriggerEnter(Collider other)
		{
			if (!other.CompareTag(playerTag)) return;
			if (MainQuestManager.Instance != null)
			{
				MainQuestManager.Instance.OnZoneReached(zoneId);
			}
		}
	}
}


