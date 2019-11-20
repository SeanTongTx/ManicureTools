using System;
using System.Collections;
using Assets.Tools.Script.Serialized.LocalCache;
using Assets.Tools.Script.Serialized.LocalCache.Core;
using SeanLib.Core;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 加载一个AssetBundle
    /// </summary>
    public class AssetBundleLoader : LocalVersionEnableLoader
    {
        public AssetBundleLoader(DownLoadAgent config) : base(config)
        {
        }
        /// <summary>
        /// 加载到的资源
        /// </summary>
        public AssetBundle assetBundle
        {
            get;private set;
        }

        protected override Coroutine LoadFromLocal()
        {
           return CoroutineCall.Call(StartLoadLocal).coroutine;
        }
        protected override Coroutine LoadFromResource()
        {
            return null;
        }
        private IEnumerator StartLoadLocal()
        {
            AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync(LoadPath.saveLoadPath + cacheName);
            yield return bundleLoadRequest;
            var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
            if (myLoadedAssetBundle == null)
            {
                LoadError();
            }
            else if (bundleLoadRequest.isDone)
            {
                assetBundle = bundleLoadRequest.assetBundle;
                LoadComplete();
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            assetBundle = null;
        }
        protected override string GetCacheName()
        {
            return LoadPath.replace("bundle/" + base.GetCacheName() + ".assetbundle");
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            if (www!=null)
            {
                ESFile.SaveRaw(www.bytes,cacheName);
            }     
        }

        protected override void OnUnloadLocal()
        {
            base.OnUnloadLocal();
            ESFile.Delete(cacheName);
        }

        protected override void OnLoadCompleteHandler()
        {
            base.OnLoadCompleteHandler();
            if(www!=null)
            assetBundle = www.assetBundle;
        }

        public override bool HasLocal()
        {
            return ESFile.Exists(cacheName);
        }
    }
}