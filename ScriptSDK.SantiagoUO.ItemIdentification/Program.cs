using ScriptSDK.Items;
using ScriptSDK.SantiagoUO.Utilities;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.ItemIdentification
{
    class Program
    {
        private static readonly double MAXIMUM_SKILL_VALUE = 100;

        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.ItemIdentification, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            while (StealthAPI.Stealth.Client.GetSkillValue(Skill.ItemIdentification) < MAXIMUM_SKILL_VALUE)
            {
                var weapon = ObjectsFinder.FindInBackpackOrPaperdoll<Item>(EasyUOItem.PICKAXES);
                if (weapon.Count == 0)
                {
                    StealthAPI.Stealth.Client.Wait(1000);

                    continue;
                }

                StealthAPI.Stealth.Client.UseSkill(Skill.ItemIdentification);
                StealthAPI.Stealth.Client.WaitForTarget(5000);
                StealthAPI.Stealth.Client.TargetToObject(weapon[0].Serial.Value);

                DateTime dateTime = DateTime.Now;
                DateTime maxDateTime = dateTime.AddMilliseconds(6000);

                while (DateTime.Now < maxDateTime)
                {
                    if (StealthAPI.Stealth.Client.InJournalBetweenTimes("You estimate", dateTime, DateTime.Now) >= 0)
                        break;

                    Thread.Sleep(100);
                }
            }

            skillGainTracker.Stop();
        }
    }
}
