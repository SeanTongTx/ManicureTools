using System.Collections;
using System.Collections.Generic;

using Assets.Tools.Script.Net.Downloader;
using Assets.Tools.Script.Serialized.LocalCache;

using UnityEngine;

public class UnityResourceLoader : LocalVersionEnableLoader
{
    public Object resouceObj;
    public UnityResourceLoader(DownLoadAgent config)
        : base(config)
    {
    }

    public override bool HasLocal()
    {
        return true;
    }

    protected override Coroutine LoadFromLocal()
    {
        return null;
    }

    protected override Coroutine LoadFromResource()
    {
        resouceObj = Resources.Load(url);
        if (resouceObj != null)
        {
            State = LoaderState.complete;
        }
        return null;
    }
}