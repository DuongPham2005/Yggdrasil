using UnityEngine;
using UnityEngine.UI;

namespace ScriptSSS.Quests
{
	public class QuestPingUI : MonoBehaviour
	{
		[Header("Refs")]
		public Camera worldCamera;
		public RectTransform canvasRect;
		public RectTransform iconPrefab; // simple Image inside a RectTransform

		[Header("Targets")]
		public string[] pingIds; // list of ids to show for current step

		RectTransform[] icons;

		void Start()
		{
			if (worldCamera == null) worldCamera = Camera.main;
			if (canvasRect == null) canvasRect = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();
			SetupIcons();
		}

		public void SetTargets(params string[] ids)
		{
			pingIds = ids;
			SetupIcons();
		}

		void SetupIcons()
		{
			ClearIcons();
			if (iconPrefab == null || canvasRect == null || pingIds == null) return;
			icons = new RectTransform[pingIds.Length];
			for (int i = 0; i < pingIds.Length; i++)
			{
				var inst = Instantiate(iconPrefab, canvasRect);
				inst.gameObject.SetActive(true);
				icons[i] = inst;
			}
		}

		void ClearIcons()
		{
			if (icons == null) return;
			for (int i = 0; i < icons.Length; i++)
			{
				if (icons[i] != null) Destroy(icons[i].gameObject);
			}
			icons = null;
		}

		void LateUpdate()
		{
			if (icons == null || pingIds == null || worldCamera == null || canvasRect == null) return;
			for (int i = 0; i < pingIds.Length; i++)
			{
				var target = QuestPingService.Get(pingIds[i]);
				if (target == null || icons[i] == null) continue;

				Vector3 screenPos = worldCamera.WorldToScreenPoint(target.position);
				bool behind = screenPos.z < 0f;
				if (behind)
				{
					// Flip to edge if behind camera
					screenPos.x = Screen.width - screenPos.x;
					screenPos.y = Screen.height - screenPos.y;
				}

				Vector2 viewportPos = new Vector2(
					Mathf.Clamp01(screenPos.x / Screen.width),
					Mathf.Clamp01(screenPos.y / Screen.height)
				);

				Vector2 canvasPos = new Vector2(
					(viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
					(viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
				);

				icons[i].anchoredPosition = canvasPos;
			}
		}
	}
}


