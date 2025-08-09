using System;

namespace ScriptSSS.Quests
{
    public static class QuestEvents
    {
        public static event Action<string> EnemyKilled; // string targetId

        public static void RaiseEnemyKilled(string targetId)
        {
            EnemyKilled?.Invoke(targetId);
        }
    }
}
