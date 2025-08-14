using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptSSS.Quests;

public class MinimapUI : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private RectTransform mapRect; // container rect (square recommended)
	[SerializeField] private RectTransform playerIconPrefab;
	[SerializeField] private RectTransform npcIconPrefab;
	[SerializeField] private RectTransform enemyIconPrefab;
	[SerializeField] private Transform player; // if null -> find by tag "Player"

	[Header("Settings")] 
	[SerializeField] private float worldRange = 50f; // meters shown from center to edge
	[SerializeField] private bool rotateWithPlayer = true; // rotate map by player's yaw
	[SerializeField] private float scanInterval = 1f; // seconds to refresh targets

	private RectTransform playerIcon;
	private readonly Dictionary<Transform, RectTransform> npcIcons = new Dictionary<Transform, RectTransform>();
	private readonly Dictionary<Transform, RectTransform> enemyIcons = new Dictionary<Transform, RectTransform>();
	private float timeToScan;

	void Awake()
	{
		if (mapRect == null) mapRect = GetComponent<RectTransform>();
	}

	void Start()
	{
		if (player == null)
		{
			var playerGo = GameObject.FindGameObjectWithTag("Player");
			if (playerGo != null) player = playerGo.transform;
		}

		if (playerIconPrefab != null && mapRect != null)
		{
			playerIcon = Instantiate(playerIconPrefab, mapRect);
			playerIcon.gameObject.SetActive(true);
		}

		ScanTargets();
	}

	void Update()
	{
		if (player == null || mapRect == null) return;

		// rotate map with player yaw if enabled
		if (rotateWithPlayer)
		{
			Vector3 e = mapRect.localEulerAngles;
			e.z = -player.eulerAngles.y; // UI rotates opposite to keep player up
			mapRect.localEulerAngles = e;
		}

		float unitsPerEdge = Mathf.Max(1f, worldRange);
		float pixelsPerUnit = Mathf.Min(mapRect.rect.width, mapRect.rect.height) * 0.5f / unitsPerEdge;

		// Update player icon at center
		if (playerIcon != null)
		{
			playerIcon.anchoredPosition = Vector2.zero;
		}

		// Update NPC icons
		UpdateIconPositions(npcIcons, pixelsPerUnit);
		// Update Enemy icons
		UpdateIconPositions(enemyIcons, pixelsPerUnit);

		timeToScan -= Time.deltaTime;
		if (timeToScan <= 0f)
		{
			ScanTargets();
		}
	}

	private void UpdateIconPositions(Dictionary<Transform, RectTransform> dict, float pixelsPerUnit)
	{
		var toRemove = (List<Transform>)null;
		foreach (var pair in dict)
		{
			Transform t = pair.Key;
			RectTransform icon = pair.Value;
			if (t == null || icon == null)
			{
				if (toRemove == null) toRemove = new List<Transform>();
				toRemove.Add(t);
				continue;
			}

			Vector3 delta = t.position - player.position;
			Vector2 flat = new Vector2(delta.x, delta.z);
			float distance = flat.magnitude;
			// clamp to edge if outside range
			if (distance > worldRange)
			{
				flat = flat.normalized * worldRange;
			}

			// rotate inverse of player yaw so that icons rotate with map
			float yaw = rotateWithPlayer ? -player.eulerAngles.y * Mathf.Deg2Rad : 0f;
			float cos = Mathf.Cos(yaw);
			float sin = Mathf.Sin(yaw);
			Vector2 rotated = new Vector2(
				flat.x * cos - flat.y * sin,
				flat.x * sin + flat.y * cos
			);

			icon.anchoredPosition = rotated * pixelsPerUnit;
		}

		if (toRemove != null)
		{
			for (int i = 0; i < toRemove.Count; i++)
			{
				var key = toRemove[i];
				if (dict.TryGetValue(key, out var icon) && icon != null) Destroy(icon.gameObject);
				dict.Remove(key);
			}
		}
	}

	private void ScanTargets()
	{
		timeToScan = Mathf.Max(0.2f, scanInterval);
		// cleanup invalid
		CleanupDict(npcIcons);
		CleanupDict(enemyIcons);

		if (npcIconPrefab != null)
		{
			var npcs = GameObject.FindObjectsOfType<InteractableNPC>();
			AddMissingIcons(npcs, npcIcons, npcIconPrefab);
		}
		if (enemyIconPrefab != null)
		{
			var enemies = GameObject.FindObjectsOfType<Enemy>();
			AddMissingIcons(enemies, enemyIcons, enemyIconPrefab);
		}
	}

	private void AddMissingIcons<T>(IReadOnlyList<T> list, Dictionary<Transform, RectTransform> dict, RectTransform prefab) where T : Component
	{
		for (int i = 0; i < list.Count; i++)
		{
			var t = list[i].transform;
			if (t == null || dict.ContainsKey(t)) continue;
			var icon = Instantiate(prefab, mapRect);
			icon.gameObject.SetActive(true);
			dict[t] = icon;
		}
	}

	private void CleanupDict(Dictionary<Transform, RectTransform> dict)
	{
		var remove = new List<Transform>();
		foreach (var pair in dict)
		{
			if (pair.Key == null || pair.Value == null) remove.Add(pair.Key);
		}
		for (int i = 0; i < remove.Count; i++)
		{
			var key = remove[i];
			if (dict.TryGetValue(key, out var icon) && icon != null) Destroy(icon.gameObject);
			dict.Remove(key);
		}
	}
}


