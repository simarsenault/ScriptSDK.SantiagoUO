using ScriptSDK.Mobiles;
using StealthAPI;
using System.Collections.Generic;
using System.Threading;

namespace ScriptSDK.SantiagoUO.Utilities.SkillGainTracker
{
    public class SkillGainTracker
    {
        private Dictionary<Skill, double> skills;
        private List<ISkillChangeEventHandler> skillChangeEventHandlers;
        private Thread thread;
        private bool stopThread;

        public SkillGainTracker(Skill skill, ISkillChangeEventHandler skillChangeEventHandler)
        {
            this.skills = new Dictionary<Skill, double>();
            this.skills.Add(skill, StealthAPI.Stealth.Client.GetSkillValue(skill));
           
            this.skillChangeEventHandlers = new List<ISkillChangeEventHandler>();
            this.skillChangeEventHandlers.Add(skillChangeEventHandler);
        }

        public SkillGainTracker(List<Skill> skills, List<ISkillChangeEventHandler> skillChangeEventHandlers)
        {
            this.skills = new Dictionary<Skill, double>();
            foreach (var skill in skills)
            {
                this.skills.Add(skill, StealthAPI.Stealth.Client.GetSkillValue(skill));
            }
            this.skillChangeEventHandlers = skillChangeEventHandlers;
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
            while (!this.stopThread)
            {
                foreach (var skillValueEntry in this.skills)
                {
                    var newSkillValue = StealthAPI.Stealth.Client.GetSkillValue(skillValueEntry.Key);

                    if (newSkillValue != skillValueEntry.Value)
                    {
                        SkillChangeEvent skillChangeEvent = new SkillChangeEvent(PlayerMobile.GetPlayer(), skillValueEntry.Key, skillValueEntry.Value, newSkillValue);

                        foreach (var skillChangeEventHandler in skillChangeEventHandlers)
                        {
                            skillChangeEventHandler.Handle(skillChangeEvent);
                        }

                        this.skills.Remove(skillValueEntry.Key);
                        this.skills.Add(skillValueEntry.Key, newSkillValue);

                        break;
                    }
                }

                Thread.Sleep(1000);
            }
        }
    }
}
