using System;

namespace Assets.Tools.Script.Serialized.LocalCache.Core
{
    /// <summary>
    /// 能够缓存到本地的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UnityLocalCache<T> : IUnityLocalCache
    {
        private readonly string _cacheName;

        protected UnityLocalCache(string name)
        {
            _cacheName = name;
        }

        protected UnityLocalCache(string name, T defaultValue)
        {
            _cacheName = name;
            if (!this.HasCache())
                Value = defaultValue;
        }
        /// <summary>
        /// 保存的key,(baseName_Suffix)
        /// </summary>
        public string CacheName
        {
            get
            {
                return _cacheName;
            }
        }

        protected T _value;
        /// <summary>
        /// 保存值，在设置值时会缓存到本地
        /// 如果从未设置，读取类型的默认值
        /// </summary>
        public T Value
        {
            get
            {
                LoadCache();
                return _value;
            }
            set
            {
                _value = value;
                SaveCache();
            }
        }

        object IUnityLocalCache.Value
        {
            get
            {
                return Value;
            }

            set
            {
                Value = (T)value;
            }
        }

        /// <summary>
        /// 本地外存是否已经保存过这个值
        /// </summary>
        /// <returns></returns>
        public abstract bool HasCache();
        /// <summary>
        /// 删除缓存
        /// </summary>
        public abstract void DeleteCache();

        //--------------------------------------------------------------
        //保存和读取方法
        protected abstract T GetLocalCache();
        protected abstract void SaveLocalCache(T value);

        public virtual void UnLoadCache()
        {
            _value = default(T);
        }

        public void SaveCache()
        {
            SaveLocalCache(_value);
        }

        public void LoadCache()
        {
            if (_value == null || _value.Equals(default(T)))
            {
                if (HasCache())
                    _value = GetLocalCache();
                else
                    _value = default(T);
            }
        }
    }
}