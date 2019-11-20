using Assets.Tools.Script.Serialized.LocalCache.Core;
using SeanLib.Core;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存二进制字节
    /// </summary>
    public class LcBytesFile : UnityLocalESFileCache<byte[]>
    {
        public LcBytesFile(string name)
            : base(name,".bytes")
        {
        }

        protected override byte[] GetLocalCache()
        {

            return ESFile.LoadRaw(fileName);
        }

        protected override void SaveLocalCache(byte[] value)
        {
            ESFile.SaveRaw(value,fileName);
        }
    }
}