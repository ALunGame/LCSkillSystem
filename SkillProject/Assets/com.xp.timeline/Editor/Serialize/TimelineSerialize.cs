using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using XPToolchains.Json;

namespace Timeline.Serialize
{
    /// <summary>
    ///  Timeline序列化
    /// </summary>
    public static class TimelineSerialize
    {
        public const string TimelineAssetExNam = ".timeline";

        public static void Save(SequenceData data, string savePath)
        {
            string filePath = string.Format("{0}/{1}", savePath, data.Name + TimelineAssetExNam);
            string jsonStr = JsonMapper.ToJson(data);
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllText(filePath, jsonStr, Encoding.UTF8);
        }

        public static SequenceData Load(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;
            string jsonStr = File.ReadAllText(filePath);
            return JsonMapper.ToObject<SequenceData>(jsonStr);
        }
    }
}