using DotNetMap.Models.Map;
using Newtonsoft.Json;
using System;

namespace DotNetMap.Processors
{
    public class CustomTileProcessor
    {
        public const string CUSTOM_FILE_NAME = "CustomTileProviders.json";

        public static TileManager GetManager => GetCustomManager();

        public static TileManager GetCustomManager()
        {
            string json = ReadCustomProviders();
            TileManager manager = LoadFromJson(json);
            return manager;
        }

        public static string ReadCustomProviders(string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = $"{FileProcessor.AppPath}{CUSTOM_FILE_NAME}";
            }
            return FileProcessor.ReadTextFile(filePath);
        }

        public static TileManager LoadFromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("JSON string cannot be null or empty.", nameof(json));
            }
            return JsonConvert.DeserializeObject<TileManager>(json);
        }
    }
}
