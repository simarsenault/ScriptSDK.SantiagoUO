using ScriptSDK.Items;
using ScriptSDK.SantiagoUO.Utilities;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.ArmsLore
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;
        private static readonly Skill ArmsLore = new Skill() { Value = "Arms Lore" };

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(ArmsLore, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(ArmsLore) < MAXIMUM_SKILL_VALUE)
            {
                var weapon = ObjetsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.PICKAXES);
                if (weapon.Count == 0)
                {
                    StealthAPI.Stealth.Client.Wait(1000);

                    continue;
                }

                StealthAPI.Stealth.Client.UseSkill(ArmsLore);
                StealthAPI.Stealth.Client.WaitForTarget(5000);
                StealthAPI.Stealth.Client.TargetToObject(weapon[0].Serial.Value);
                StealthAPI.Stealth.Client.Wait(2000);
            }

            skillGainTracker.Stop();
        }
    }
}
