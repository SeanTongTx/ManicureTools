using SeanLib.Core;
using System.Collections;
using UnityEngine;
namespace LocalLoadProcess
{
    public class AssetBundleLoader : CacheLoaderBase<AssetBundle>
    {
        public AssetBundleLoader()
        {
        }

        public AssetBundleLoader(LoaderConfig config) : base(config)
        {
        }

        public override void Execute()
        {
            CoroutineCall.Call(LoadCacheFile);
        }
        private IEnumerator LoadCacheFile()
        {
            AssetBundleCreateRequest bundleLoadRequest = null;
            bundleLoadRequest = AssetBundle.LoadFromFileAsync(Config.LoadPath);
            yield return bundleLoadRequest;
            Data = bundleLoadRequest.assetBundle;
            if (Data == null)
            {
                Error();
            }
            else
            {
                Complete();
            }
        }
    }
}
