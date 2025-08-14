using UnityEngine;
using UnityEngine.UI;

namespace ScriptSSS.Quests
{
	public class MainQuestUI : MonoBehaviour
	{
		[SerializeField] private MainQuestManager manager;
		[SerializeField] private Text text;

		void Start()
		{
			if (manager == null) manager = FindObjectOfType<MainQuestManager>();
		}

		void Update()
		{
			if (manager == null || text == null) return;
			text.text = manager.GetObjectiveText();
		}
	}
}


