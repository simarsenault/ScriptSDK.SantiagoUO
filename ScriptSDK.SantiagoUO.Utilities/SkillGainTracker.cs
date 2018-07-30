using StealthAPI;
using System;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Utilities
{
    public class ConsoleSkillGainTracker
    {
        private Skill skill;
        private Thread thread;
        private bool stopThread;

        public ConsoleSkillGainTracker(Skill skill)
        {
            this.skill = skill;
        }

        public void Start()
        {
            this.thread = new Thread(() => Run());
            this.stopThread = false;
            this.thread.Start();
        }

        public void Stop()
        {
            this.stopThread = true;
        }

        private void Run()
        {
            double oldSkillValue = Stealth.Client.GetSkillValue(this.skill);
            string character = Stealth.Client.GetCharName();

            while (!this.stopThread)
            {
                double currentSkillValue = Stealth.Client.GetSkillValue(this.skill);

                if (oldSkillValue != currentSkillValue)
                {
                    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [SkillGainTracker] " + character + ": " + skill.Value + " " + (oldSkillValue < currentSkillValue ? "increased" : "decreased") +" from " + oldSkillValue + " to " + currentSkillValue);

                    oldSkillValue = currentSkillValue;
                }
            }
        }
    }
}
