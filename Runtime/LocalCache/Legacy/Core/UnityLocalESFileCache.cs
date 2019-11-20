using SeanLib.Core;

namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 通过文件保存的缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnityLocalESFileCache<T> : UnityLocalCache<T>
    {
        private readonly string _fileSuffix;

        /// <summary>
        /// 文件名
        /// </summary>
        public string fileName { get { return CacheName + _fileSuffix; } }

        protected UnityLocalESFileCache(string fileName, string fileSuffix = ".txt")
            : base(fileName)
        {
            _fileSuffix = fileSuffix;
        }

        override public bool HasCache()
        {
            return ESFile.Exists(fileName);
        }
        override public void DeleteCache()
        {
            ESFile.Delete(fileName);
        }



    }
}