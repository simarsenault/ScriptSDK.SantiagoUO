using ScriptSDK.SantiagoUO.Utilities;
using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Tracking
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            ConsoleSkillGainTracker consoleSkillGainTracker = new ConsoleSkillGainTracker(Skill.Tracking);
            consoleSkillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.Tracking) < MAXIMUM_SKILL_VALUE)
            {
                StealthAPI.Stealth.Client.WaitMenu("Tracking", "Anything that moves");
                StealthAPI.Stealth.Client.WaitMenu("Tracking", "TrackingTrainer");
                StealthAPI.Stealth.Client.UseSkill(Skill.Tracking);
                Thread.Sleep(1000);
                StealthAPI.Stealth.Client.SetWarMode(true);
                StealthAPI.Stealth.Client.SetWarMode(false);
            }

            consoleSkillGainTracker.Stop();
        }
    }
}
