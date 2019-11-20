using Assets.Tools.Script.Serialized.LocalCache;

namespace Assets.Tools.Script.Net.Downloader
{
    using Assets.Tools.Script.Serialized.LocalCache.Core;

    using UnityEngine;

    /// <summary>
    /// 文本加载器
    /// </summary>
    public class TextLoader : LocalVersionEnableLoader
    {
        //本地保存用
        public UnityLocalCache<string> _localCache;
        public TextLoader(DownLoadAgent config)
            : base(config)
        {
            switch (config.cacheType)
            {
                case DownLoadAgent.LocalCacheType.File: _localCache = new LcStringFile(cacheName); break;
                case DownLoadAgent.LocalCacheType.PlayerPrefs: _localCache = new LcString(cacheName); break;
                case DownLoadAgent.LocalCacheType.Memory:
                    _localCache = new LcMemory<string>(cacheName); break;
            }
        }
        /// <summary>
        /// 加载完成后的文本内容
        /// </summary>
        public string text { get; private set; }

        protected override Coroutine LoadFromResource()
        {
            object obj = Resources.Load(url);
            if (obj != null)
            {
                text = obj.ToString();
                State=LoaderState.complete;
            }
            return null;
        }

        protected override Coroutine LoadFromLocal()
        {
            text = _localCache.Value;
            State=LoaderState.complete;
            return null;
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            _localCache.Value = text;
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            _localCache.DeleteCache();
        }

        protected override void OnLoadCompleteHandler()
        {
            base.OnLoadCompleteHandler();
            if (www != null)
                text = www.text;
        }


        protected override void OnUnload()
        {
            base.OnUnload();
            _localCache = null;
            text = null;
        }

        public override bool HasLocal()
        {
            return _localCache.HasCache();
        }

    }
}