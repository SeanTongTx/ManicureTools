

using Assets.Tools.Script.Serialized.LocalCache.Core;
using SeanLib.Core;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 文件保存字符串
    /// </summary>
    public class LcStringFile : UnityLocalESFileCache<string>
    {
        public LcStringFile(string name) : base(name)
        {
        }

        protected override string GetLocalCache()
        {
            return ESFile.LoadString(fileName);
        }

        protected override void SaveLocalCache(string value)
        {
            ESFile.Save(value,fileName);
        }
    }
}