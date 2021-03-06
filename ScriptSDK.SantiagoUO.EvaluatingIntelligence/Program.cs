﻿using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.EvaluatingIntelligence
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.EvaluateIntelligence, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.EvaluateIntelligence) < MAXIMUM_SKILL_VALUE)
            {
                var human = ObjectsFinder.Find<Mobile>(EasyUOItem.MOBILE_HUMANS, 2).Find(_human => _human.Serial.Value != PlayerMobile.GetPlayer().Serial.Value);
                if (human == null)
                {
                    StealthAPI.Stealth.Client.Wait(1000);

                    continue;
                }

                StealthAPI.Stealth.Client.UseSkill(Skill.EvaluateIntelligence);
                StealthAPI.Stealth.Client.WaitForTarget(5000);
                StealthAPI.Stealth.Client.TargetToObject(human.Serial.Value);
                StealthAPI.Stealth.Client.Wait(2000);
            }

            skillGainTracker.Stop();
        }
    }
}
