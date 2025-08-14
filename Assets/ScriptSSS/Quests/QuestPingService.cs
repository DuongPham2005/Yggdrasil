using System.Collections.Generic;
using UnityEngine;

namespace ScriptSSS.Quests
{
	public static class QuestPingService
	{
		private static readonly Dictionary<string, Transform> idToTransform = new Dictionary<string, Transform>();

		public static void Register(string id, Transform transform)
		{
			if (string.IsNullOrEmpty(id) || transform == null) return;
			idToTransform[id] = transform;
		}

		public static void Unregister(string id, Transform transform)
		{
			if (string.IsNullOrEmpty(id)) return;
			if (idToTransform.TryGetValue(id, out var t) && t == transform)
			{
				idToTransform.Remove(id);
			}
		}

		public static Transform Get(string id)
		{
			if (string.IsNullOrEmpty(id)) return null;
			idToTransform.TryGetValue(id, out var t);
			return t;
		}
	}
}


