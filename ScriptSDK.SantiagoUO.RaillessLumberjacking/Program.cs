using ScriptSDK.Engines;
using StealthAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptSDK.SantiagoUO.RaillessLumberjacking
{
    class Program
    {
        static void Main(string[] args)
        {
            TileReader.Initialize();

            new RaillessLumberjacking().Start();
        }
    }
}
