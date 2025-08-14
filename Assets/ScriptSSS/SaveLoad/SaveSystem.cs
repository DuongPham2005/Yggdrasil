using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace ScriptSSS.SaveLoad
{
	public static class SaveSystem
	{
		private static string SavesFolder => Path.Combine(Application.persistentDataPath, "Saves");

		public static string GetSavePath(string slotName)
		{
			if (string.IsNullOrWhiteSpace(slotName)) slotName = "slot1";
			Directory.CreateDirectory(SavesFolder);
			return Path.Combine(SavesFolder, slotName + ".json");
		}

		public static void SaveJson(object data, string slotName)
		{
			try
			{
				string path = GetSavePath(slotName);
				string json = JsonUtility.ToJson(data, true);
				File.WriteAllText(path, json, Encoding.UTF8);
				Debug.Log($"Saved to {path}");
			}
			catch (Exception ex)
			{
				Debug.LogError($"Save failed: {ex}");
			}
		}

		public static bool TryLoadJson<T>(string slotName, out T data)
		{
			data = default;
			try
			{
				string path = GetSavePath(slotName);
				if (!File.Exists(path)) return false;
				string json = File.ReadAllText(path, Encoding.UTF8);
				data = JsonUtility.FromJson<T>(json);
				return data != null;
			}
			catch (Exception ex)
			{
				Debug.LogError($"Load failed: {ex}");
				return false;
			}
		}
	}
}


