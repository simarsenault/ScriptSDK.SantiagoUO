using ScriptSDK.Data;
using ScriptSDK.SantiagoUO.Utilities;
using StealthAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScriptSDK.SantiagoUO.Hiding
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;
        private static readonly int MAXIMUM_TIMEOUT = 5000;

        static void Main(string[] args)
        {
            ConsoleSkillGainTracker consoleSkillGainTracker = new ConsoleSkillGainTracker(Skill.Hiding);
            consoleSkillGainTracker.Start();

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

            consoleSkillGainTracker.Stop();
        }
    }
}
