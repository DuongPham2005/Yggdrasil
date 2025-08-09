using System;
using System.Collections.Generic;

namespace ScriptSSS.Quests
{
    [Serializable]
    public class QuestProgress
    {
        public string questId;
        public Dictionary<string, int> counters = new Dictionary<string, int>();
        public bool completed;
    }
}
