using ScriptSDK.Engines;
using ScriptSDK.SantiagoUO.Utilities.SkillGainTracker;
using StealthAPI;

namespace ScriptSDK.SantiagoUO.RaillessLumberjacking
{
    class Program
    {
        static void Main(string[] args)
        {
            SkillGainTracker skillGainTracker = new SkillGainTracker(Skill.Lumberjacking, new DiscordSkillChangeEventHandler());
            skillGainTracker.Start();

            TileReader.Initialize();

            new RaillessLumberjacking().Start();

            skillGainTracker.Stop();
        }
    }
}
