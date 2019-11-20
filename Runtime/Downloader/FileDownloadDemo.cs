
using SeanLib.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDownloadDemo : MonoBehaviour
{
    public string Url;
    FileDownLoader downLoader;
    [InspectorPlus.Button]
    public void Download()
    {
        DownloadConfig config = new DownloadConfig()
        {
            URL = Url,
            GUID = "TestFile",
            CacheFilePath = Application.dataPath + "/TestFile"
        };
        downLoader = new FileDownLoader(config);
        downLoader.OnError.AddEventListener((l) =>
        {
            Debug.Log(l.ErrorStr);
        });
        downLoader.OnComplete.AddEventListener((l) =>
        {
            Debug.Log("Complete filepath:" + (l as FileDownLoader).config.CacheFilePath);
        });
        downLoader.Execute();
        Debug.Log("Call End");
    }
    FileDownLoader loader;

    [InspectorPlus.Button]
    public void DownloadAsync()
    {
        DownloadConfig config = new DownloadConfig()
        {
            URL = Url,
            GUID = "TestFile",
            CacheFilePath = Application.dataPath + "/TestFile"
        };
        loader = new FileDownLoader(config) { Async = true };
        loader.OnComplete.AddEventListener((l) =>
        {
            Debug.Log("Complete filepath:" + loader.config.CacheFilePath);
        });
        loader.OnError.AddEventListener((l) =>
        {
            Debug.Log(loader.ErrorStr);
        });
        loader.OnCancel.AddEventListener((l) =>
        {
            Debug.Log("OnCancel");
        });
        loader.Execute();
        Debug.Log("Call End");
    }
    [InspectorPlus.Button]
    public void Cancel()
    {
        if(loader!=null)
        {
            loader.Cancel();
        }
    }
    private void Update()
    {
        if (loader != null)
        {
            Debug.Log(loader.progress*100+"%");
        }
    }

}
