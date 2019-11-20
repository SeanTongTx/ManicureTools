using LocalLoadProcess;
using SeanLib.Core;
using UnityEngine;

public class LoaderDemo : MonoBehaviour
{
    [InspectorPlus.Button]
    public void LoadAssetBundle()
    {
        Load(new AssetBundleLoader(), "canvas");
    }
    [InspectorPlus.Button]
    public void LoadFile()
    {
        Load(new FileLoader(), "11.jpg");
    }

    public void Load<T>(CacheLoaderBase<T> loader,string cahceName)
    {
        loader.OnComplete.AddEventListener((l) =>
        {
            Debug.Log("complete:" + loader.Data);
        });
        loader.OnError.AddEventListener((l) =>
        {
            Debug.Log("Error");
        });
        LoaderConfig config = new LoaderConfig();
        config.CacheName = cahceName;
        loader.Config = config;
        loader.Execute();
    }
}
