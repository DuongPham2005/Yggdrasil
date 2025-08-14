using UnityEngine;

namespace ScriptSSS.Quests
{
	public class QuestPingTarget : MonoBehaviour
	{
		public string pingId; // e.g., "village_npc", "castle_npc", "dungeon_exit", "castle_gate"

		void OnEnable()
		{
			QuestPingService.Register(pingId, transform);
		}

		void OnDisable()
		{
			QuestPingService.Unregister(pingId, transform);
		}
	}
}


