using ScriptSDK.Engines;
using ScriptSDK.Items;
using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Anatomy
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            ConsoleSkillGainTracker consoleSkillGainTracker = new ConsoleSkillGainTracker(Skill.Anatomy);
            consoleSkillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.Anatomy) < MAXIMUM_SKILL_VALUE)
            {
                var human = ItemFinder.Find<Mobile>(EasyUOItem.HUMANS, 2).Find(_human => _human.Serial.Value != PlayerMobile.GetPlayer().Serial.Value);
                if (human == null)
                {
                    StealthAPI.Stealth.Client.Wait(1000);

                    continue;
                }

                StealthAPI.Stealth.Client.UseSkill(Skill.Anatomy);
                StealthAPI.Stealth.Client.WaitForTarget(5000);
                StealthAPI.Stealth.Client.TargetToObject(human.Serial.Value);
                StealthAPI.Stealth.Client.Wait(4000);
            }

            consoleSkillGainTracker.Stop();
        }
    }
}
