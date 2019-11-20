namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface IUnityLocalCache
    {
        bool HasCache();
        void SaveCache();
        void DeleteCache();
        void LoadCache();
        void UnLoadCache();
        object Value { get; set; }
        string CacheName { get; }
    }
}