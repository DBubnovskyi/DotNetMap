using DotNetMap.Models.Map;
using DotNetMap.Processors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TileManager tileManager = CustomTileProcessor.GetManager;

            foreach (var provider in tileManager.TileServers)
            {
                Console.WriteLine($"{provider.Name}");
                foreach (var tileProvider in provider.TileProviders)
                {
                    Console.WriteLine($"    {tileProvider.Name}");
                }
            }
            Console.ReadKey();
        }
    }
}
