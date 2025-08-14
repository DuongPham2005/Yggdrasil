using UnityEngine;
using UnityEngine.UI;

namespace ScriptSSS.SaveLoad
{
	public class SaveLoadMenu : MonoBehaviour
	{
		[SerializeField] private SaveManager saveManager;
		[SerializeField] private string slotName = "slot1";
		[SerializeField] private Button saveButton;
		[SerializeField] private Button loadButton;
		[SerializeField] private GameObject panel;
		[SerializeField] private KeyCode toggleKey = KeyCode.Tab;
		[SerializeField] private bool closeOnLoad = true;

		private enum ToggleMode { SetActive, CanvasGroup }
		private ToggleMode toggleMode = ToggleMode.SetActive;
		private CanvasGroup canvasGroup;

		public bool IsOpen => IsVisible();

		private void Awake()
		{
			if (saveButton != null) saveButton.onClick.AddListener(OnClickSave);
			if (loadButton != null) loadButton.onClick.AddListener(OnClickLoad);
		}

		private void Start()
		{
			if (panel == null) panel = gameObject;
			if (panel == gameObject)
			{
				canvasGroup = panel.GetComponent<CanvasGroup>();
				if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();
				toggleMode = ToggleMode.CanvasGroup;
			}
			else
			{
				toggleMode = ToggleMode.SetActive;
			}

			SetVisible(false);
		}

		private void Update()
		{
			if (Input.GetKeyDown(toggleKey))
			{
				SetVisible(!IsVisible());
			}
		}

		public void Open() => SetVisible(true);
		public void Close() => SetVisible(false);

		private void OnClickSave()
		{
			if (saveManager != null) saveManager.SaveGame(slotName);
		}

		private void OnClickLoad()
		{
			if (saveManager != null)
			{
				bool loaded = saveManager.LoadGame(slotName);
				if (loaded && closeOnLoad) SetVisible(false);
			}
		}

		private bool IsVisible()
		{
			switch (toggleMode)
			{
				case ToggleMode.CanvasGroup:
					return canvasGroup != null && canvasGroup.alpha > 0.5f;
				default:
					return panel != null && panel.activeSelf;
			}
		}

		private void SetVisible(bool visible)
		{
			switch (toggleMode)
			{
				case ToggleMode.CanvasGroup:
					if (canvasGroup == null) return;
					canvasGroup.alpha = visible ? 1f : 0f;
					canvasGroup.interactable = visible;
					canvasGroup.blocksRaycasts = visible;
					break;
				default:
					if (panel != null) panel.SetActive(visible);
					break;
			}
		}
	}
}


