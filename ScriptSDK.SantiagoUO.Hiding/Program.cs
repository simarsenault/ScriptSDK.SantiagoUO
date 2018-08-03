using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Hiding
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;
        private static readonly int MAXIMUM_TIMEOUT = 5000;

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.Hiding, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.Hiding) < MAXIMUM_SKILL_VALUE)
            {
                DateTime dateTime = DateTime.Now;
                DateTime maxDateTime = dateTime.AddMilliseconds(MAXIMUM_TIMEOUT);

                StealthAPI.Stealth.Client.UseSkill(Skill.Hiding);

                while (DateTime.Now < maxDateTime)
                {
                    if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You can't seem to hide here", dateTime, DateTime.Now) >= 0 || StealthAPI.Stealth.Client.InJournalBetweenTimes("You have hidden yourself well", dateTime, DateTime.Now) >= 0)
                        break;

                    Thread.Sleep(50);
                }
            }

            skillGainTracker.Stop();
        }
    }
}
