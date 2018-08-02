using ScriptSDK.Engines;

namespace ScriptSDK.SantiagoUO.RaillessMining
{
    class Program
    {
        static void Main(string[] args)
        {
            TileReader.Initialize();

            new RaillessMining().Start();
        }
    }
}
