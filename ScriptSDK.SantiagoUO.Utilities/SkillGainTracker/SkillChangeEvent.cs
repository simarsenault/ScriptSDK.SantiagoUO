using ScriptSDK.Mobiles;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.Utilities.SkillGainTracker
{
    public class SkillChangeEvent
    {
        public Mobile Player { get; }
        public Skill Skill { get; }
        public double OldValue { get; }
        public double NewValue { get; }

        public SkillChangeEvent(Mobile player, Skill skill, double oldValue, double newValue)
        {
            Player = player;
            Skill = skill;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
