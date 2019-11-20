using Assets.Tools.Script.Serialized.LocalCache.Core;
using UnityEngine;

namespace Assets.Tools.Script.Serialized.LocalCache
{
    using System;

    /// <summary>
    /// 保存二进制字节
    /// </summary>
    public class LcBytes : UnityLocalPlayerPrefsCache<byte[]>
    {
        public LcBytes(string name)
            : base(name)
        {
        }

        protected override byte[] GetLocalCache()
        {

            return Convert.FromBase64String(PlayerPrefs.GetString(CacheName));
        }

        protected override void SaveLocalCache(byte[] value)
        {
            if (value == null)
            {
                PlayerPrefs.DeleteKey(CacheName);
            }
            else
            {
                PlayerPrefs.SetString(CacheName, Convert.ToBase64String(value));
            }
            PlayerPrefs.Save();
        }
    }
}