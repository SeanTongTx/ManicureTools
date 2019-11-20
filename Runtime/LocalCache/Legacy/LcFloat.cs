using Assets.Tools.Script.Serialized.LocalCache.Core;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    /// <summary>
    /// 保存float
    /// </summary>
    public class LcFloat : UnityLocalPlayerPrefsCache<float>
    {
        public LcFloat(string name)
            : base(name)
        {
        }

        public LcFloat(string name, float defaultValue)
            : base(name, defaultValue)
        {
        }

        protected override float GetLocalCache()
        {
            return PlayerPrefs.GetFloat(CacheName);
        }

        protected override void SaveLocalCache(float value)
        {
            PlayerPrefs.SetFloat(CacheName, value);
            PlayerPrefs.Save();
        }
    }
}