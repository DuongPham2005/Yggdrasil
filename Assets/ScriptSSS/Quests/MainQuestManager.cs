using UnityEngine;

namespace ScriptSSS.Quests
{
	public enum MainQuestStage
	{
		LeaveDungeon,
		TalkToVillageNPC,
		KillSkeletons,
		ReturnToVillageNPC,
		GoToCastle,
		TalkToCastleNPC,
		KillBoss,
		ReturnToCastleNPC,
		KillMainBoss,
		Completed
	}

	public class MainQuestManager : MonoBehaviour
	{
		public static MainQuestManager Instance { get; private set; }

		[Header("Config")]
		[SerializeField] string playerTag = "Player";
		[SerializeField] string skeletonId = "skeleton";
		[SerializeField] string bossId = "boss";
		[SerializeField] string mainBossId = "mainBoss";
		[SerializeField] int skeletonRequired = 5;

		[Header("State (runtime)")]
		[SerializeField] MainQuestStage currentStage = MainQuestStage.LeaveDungeon;
		[SerializeField] int skeletonKilled;

		public MainQuestStage CurrentStage => currentStage;
		public int SkeletonKilled => skeletonKilled;
		public int SkeletonRequired => skeletonRequired;

		void Awake()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
				return;
			}
			Instance = this;
		}

		void OnEnable()
		{
			QuestEvents.EnemyKilled += OnEnemyKilled;
		}

		void OnDisable()
		{
			QuestEvents.EnemyKilled -= OnEnemyKilled;
		}

		public void OnZoneReached(string zoneId)
		{
			if (string.IsNullOrEmpty(zoneId)) return;

			switch (currentStage)
			{
				case MainQuestStage.LeaveDungeon:
					if (zoneId == "dungeon_exit")
					{
						AdvanceTo(MainQuestStage.TalkToVillageNPC);
					}
					break;
				case MainQuestStage.GoToCastle:
					if (zoneId == "castle_gate")
					{
						AdvanceTo(MainQuestStage.TalkToCastleNPC);
					}
					break;
			}
		}

		public void OnNPCInteracted(string npcId)
		{
			if (string.IsNullOrEmpty(npcId)) return;

			switch (currentStage)
			{
				case MainQuestStage.TalkToVillageNPC:
					if (npcId == "village")
					{
						// After talking, receive kill skeletons quest
						AdvanceTo(MainQuestStage.KillSkeletons);
					}
					break;
				case MainQuestStage.ReturnToVillageNPC:
					if (npcId == "village")
					{
						// Turn in and get go-to-castle quest
						AdvanceTo(MainQuestStage.GoToCastle);
					}
					break;
				case MainQuestStage.TalkToCastleNPC:
					if (npcId == "castle")
					{
						// Receive kill boss quest
						AdvanceTo(MainQuestStage.KillBoss);
					}
					break;
				case MainQuestStage.ReturnToCastleNPC:
					if (npcId == "castle")
					{
						// Receive final quest to kill main boss
						AdvanceTo(MainQuestStage.KillMainBoss);
					}
					break;
			}
		}

		void OnEnemyKilled(string targetId)
		{
			if (string.IsNullOrEmpty(targetId)) return;

			if (currentStage == MainQuestStage.KillSkeletons && targetId == skeletonId)
			{
				skeletonKilled = Mathf.Clamp(skeletonKilled + 1, 0, skeletonRequired);
				if (skeletonKilled >= skeletonRequired)
				{
					AdvanceTo(MainQuestStage.ReturnToVillageNPC);
				}
			}
			else if (currentStage == MainQuestStage.KillBoss && targetId == bossId)
			{
				AdvanceTo(MainQuestStage.ReturnToCastleNPC);
			}
			else if (currentStage == MainQuestStage.KillMainBoss && targetId == mainBossId)
			{
				AdvanceTo(MainQuestStage.Completed);
			}
		}

		void AdvanceTo(MainQuestStage next)
		{
			currentStage = next;
			if (next == MainQuestStage.KillSkeletons)
			{
				skeletonKilled = 0;
			}
		}

		public string GetObjectiveText()
		{
			switch (currentStage)
			{
				case MainQuestStage.LeaveDungeon:
					return "Rời khỏi dungeon";
				case MainQuestStage.TalkToVillageNPC:
					return "Đến làng và nhấn E để nói chuyện với NPC";
				case MainQuestStage.KillSkeletons:
					return $"Tiêu diệt {skeletonRequired} skeleton ({skeletonKilled}/{skeletonRequired})";
				case MainQuestStage.ReturnToVillageNPC:
					return "Quay về NPC ở làng để báo cáo (nhấn E)";
				case MainQuestStage.GoToCastle:
					return "Đi đến lâu đài";
				case MainQuestStage.TalkToCastleNPC:
					return "Nhấn E nói chuyện với NPC ở lâu đài để nhận nhiệm vụ";
				case MainQuestStage.KillBoss:
					return "Tiêu diệt Boss";
				case MainQuestStage.ReturnToCastleNPC:
					return "Quay lại NPC ở lâu đài để báo cáo (nhấn E)";
				case MainQuestStage.KillMainBoss:
					return "Nhiệm vụ cuối: Tiêu diệt Main Boss";
				case MainQuestStage.Completed:
					return "Chuỗi nhiệm vụ chính đã hoàn thành!";
			}
			return string.Empty;
		}
	}
}


