using System;
using System.Collections.Generic;

using Assets.Tools.Script.Net.Downloader;
using SeanLib.Core;
using ServiceTools.Reflect;
using UnityEngine;

public class LoaderFactory:MonoBehaviour
{
    public static Dictionary<string,UrlLoader>Loaders=new Dictionary<string, UrlLoader>();

    public static T GetLoader<T>(DownLoadAgent config)where T:UrlLoader
    {
        UrlLoader loader = null;
        if (Loaders.TryGetValue(config.GUID, out loader))
        {
            if (loader is T)
            {
                return loader as T;
            }
            else
            {
                DebugConsole.Warning("LoaderFactory", "GetLoader","同一个资源文件被不同的类型请求");
                return null;
            }
        }
        loader = ReflecTool.Instantiate<T>(new Type[] { typeof(DownLoadAgent) }, new object[] { config });
        Loaders[config.GUID] = loader;
        return loader as T;
    }
}
