using ScriptSDK.Mobiles;
using ScriptSDK.SantiagoUO.Utilities;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.AnimalLore
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.AnimalLore, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.AnimalLore) < MAXIMUM_SKILL_VALUE)
            {
                var animals = ObjectsFinder.Find<Mobile>(EasyUOItem.GRAY_HORSE, 4);
                if (animals.Count == 0)
                {
                    StealthAPI.Stealth.Client.Wait(1000);

                    continue;
                }

                StealthAPI.Stealth.Client.UseSkill(Skill.AnimalLore);
                StealthAPI.Stealth.Client.WaitForTarget(5000);
                StealthAPI.Stealth.Client.TargetToObject(animals[0].Serial.Value);
                StealthAPI.Stealth.Client.Wait(4000);
            }

            skillGainTracker.Stop();
        }
    }
}
