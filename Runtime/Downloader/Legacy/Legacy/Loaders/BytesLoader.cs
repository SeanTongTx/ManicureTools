using Assets.Tools.Script.Serialized.LocalCache;

namespace Assets.Tools.Script.Net.Downloader
{
    using Assets.Tools.Script.Serialized.LocalCache.Core;
    using UnityEngine;

    /// <summary>
    /// 文本加载器
    /// </summary>
    public class BytesLoader:LocalVersionEnableLoader
    {
        private UnityLocalCache<byte[]> _localCache;
        public BytesLoader(DownLoadAgent config)
            : base(config)
        {
            switch (config.cacheType)
            {
                case DownLoadAgent.LocalCacheType.File: _localCache = new LcBytesFile(cacheName); break;
                case DownLoadAgent.LocalCacheType.PlayerPrefs: _localCache = new LcBytes(cacheName); break;
                case DownLoadAgent.LocalCacheType.Memory:
                    _localCache = new LcMemory<byte[]>(cacheName); break;
            }
        }
        /// <summary>
        /// 加载完成后的文本内容
        /// </summary>
        public byte[] Bytes { get; private set; }


        protected override Coroutine LoadFromLocal()
        {
            Bytes = _localCache.Value;
            State=LoaderState.complete;
            return null;
        }

        protected override Coroutine LoadFromResource()
        {
            //已字符串模式加载
            TextAsset text = Resources.Load<TextAsset>(url);
            if (text != null)
            {
                Bytes = text.bytes;//.ToString();
                State = LoaderState.complete;
            }
            return null;
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            _localCache.Value = Bytes;
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            _localCache.DeleteCache();
        }

//        protected override void OnDeleteLocal(Action onSucceed, Action onError)
//        {
//            _localCache.DeleteCache();
//            onSucceed();
//        }

        protected override void OnLoadCompleteHandler()
        {
            if(www!=null)
                Bytes = www.bytes;
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Bytes = null;
            _localCache = null;
        }

        public override bool HasLocal()
        {
            return _localCache.HasCache();
        }
    }
}