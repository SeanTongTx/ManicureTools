using System.Collections;
using System.Collections.Generic;

using Assets.Tools.Script.Net.Downloader;
using Assets.Tools.Script.Net.Downloader.Tool;
using UnityEngine;
using SeanLib.Core;

public class DonwloadDemo : MonoBehaviour
{
    [Header("版本信息")]
    public DownLoadAgent VersionFile;
    [Header("贴图")]
    public DownLoadAgent TextureFile;
    [Header("表格")]
    public DownLoadAgent CsvFile;
    [Header("二进制文件")]
    public DownLoadAgent BytesFile;
    [Header("资源包文件")]
    public DownLoadAgent AssetBundleFile;
    public UrlLoadQueue queue = new UrlLoadQueue();
    /*
    public void Start()
    {
        TextLoader versionServerLoader = LoaderFactory.GetLoader<TextLoader>(VersionFile);
        versionServerLoader.debug = true;
        versionServerLoader.id = 1;

        versionServerLoader.onLoadComplete.AddEventListener(
            (l) => DebugConsole.Log((l as TextLoader).text, "versionServerLoader"));
        versionServerLoader.onLoadError.AddEventListener(
            (l) => DebugConsole.Log("versionServerLoaderError"));
        versionServerLoader.Load();

        TextLoader versionServerLoader2 = LoaderFactory.GetLoader<TextLoader>(VersionFile);
        versionServerLoader2.debug = true;
        versionServerLoader2.id = 2;

        versionServerLoader2.onLoadComplete.AddEventListener(
            (l) => DebugConsole.Log((l as TextLoader).text, "versionServerLoader2"));
        versionServerLoader2.onLoadError.AddEventListener(
            (l) => DebugConsole.Log("versionServerLoader2Error"));
        versionServerLoader2.Load();

        PictureLoader loader = new PictureLoader(TextureFile);
        loader.debug = true;
        loader.id = 3;

        loader.onLoadComplete.AddEventListener(
            (l) => DebugConsole.Log((l as PictureLoader).texture));
        loader.onLoadError.AddEventListener(
            (l) => DebugConsole.Log("PictureLoaderError"));
        loader.Load();
    }
    */
    public void LoadAB()
    {
        AssetBundleLoader loader = new AssetBundleLoader(AssetBundleFile);
        loader.debug = true;
        loader.onLoadComplete.AddEventListener((l) =>
        {
            Debug.Log(loader.assetBundle);
        });
        loader.Load();

    }
}
