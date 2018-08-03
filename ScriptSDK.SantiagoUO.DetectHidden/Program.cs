using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Hiding
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.DetectHidden, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.DetectHidden) < MAXIMUM_SKILL_VALUE)
            {
                StealthAPI.Stealth.Client.UseSkill(Skill.DetectHidden);

                Thread.Sleep(4500);
            }

            skillGainTracker.Stop();
        }
    }
}
