using System;
using Assets.Tools.Script.Serialized.LocalCache;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    using Assets.Tools.Script.Serialized.LocalCache.Core;
    using SeanLib.Core;

    /// <summary>
    /// 加载png，jpg等图片资源
    /// </summary>
    public class PictureLoader : LocalVersionEnableLoader
    {
        public PictureLoader(DownLoadAgent config)
            : base(config)
        {
            switch (config.cacheType)
            {
                case DownLoadAgent.LocalCacheType.File: _localCache = new LcTexture2DFile(cacheName); break;
                case DownLoadAgent.LocalCacheType.PlayerPrefs: DebugConsole.Error("Loader", "TextureLoader", "不能选择PlayerPrefs"); break;
                case DownLoadAgent.LocalCacheType.Memory:
                    _localCache = new LcMemory<Texture2D>(cacheName); break;
            }
        }
        /// <summary>
        /// 加载完之后的Texture
        /// </summary>
        public Texture2D texture { get; private set; }
        private UnityLocalCache<Texture2D> _localCache;

        protected override Coroutine LoadFromResource()
        {
            Texture2D t = Resources.Load<Texture2D>(url);
            if (t != null)
            {
                texture = t;
                State=LoaderState.complete;
            }
            return null;
        }

        protected override string GetCacheName()
        {
            return "img/"+base.GetCacheName();
        }

        protected override Coroutine LoadFromLocal()
        {
            try
            {
                texture = _localCache.Value;
                State = LoaderState.complete;
                return null;
            }
            catch (Exception e)
            {
                DebugConsole.Warning("Loader","LoadTexture",e.ToString());
                _localCache.DeleteCache();
                return null;
            }
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            _localCache.Value = texture;
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
            {
                texture = www.texture;
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            texture = null;
            _localCache = null;
        }

        public override bool HasLocal()
        {
            return _localCache.HasCache();
        }

    }

//    [Serializable]
//    public class TextureCache
//    {
//        public byte[] Bytes;
//        public int Width;
//        public int Height;
//        public TextureFormat Format;
//
//        public TextureCache Init(Texture2D texture2D)
//        {
//            Format = texture2D.format;
//            Bytes = texture2D.EncodeToPNG();
//            Width = texture2D.width;
//            Height = texture2D.height;
//            return this;
//        }
//
//        public Texture2D GetTexture2D()
//        {
//            if (Bytes == null) return null;
//            Texture2D texture2D=new Texture2D(Width,Height,Format,false,false);
//            texture2D.LoadImage(Bytes);
//            return texture2D;
//        }
//    }
}