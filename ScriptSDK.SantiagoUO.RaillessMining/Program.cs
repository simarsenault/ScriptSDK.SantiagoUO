using ScriptSDK.Engines;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.RaillessMining
{
    class Program
    {
        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.Mining, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            ScriptLogger.LogToStealth = true;
            TileReader.Initialize();

            new RaillessMining().MineCave();

            skillGainTracker.Stop();
        }
    }
}
