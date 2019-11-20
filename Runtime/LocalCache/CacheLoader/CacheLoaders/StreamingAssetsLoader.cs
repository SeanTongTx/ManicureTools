
using LocalLoadProcess;
using SeanLib.Core;
using System.Collections;
using UnityEngine;

public class StreamingAssetsLoader<T> : CacheLoaderBase<T>
{
    public StreamingAssetsLoader()
    {
    }

    public StreamingAssetsLoader(LoaderConfig config) : base(config)
    {
    }

    public override void Execute()
    {
        if(Async)
        {
            CoroutineCall.Call(WwwLoad);
        }
        else
        {
            Error();
        }
    }
    private IEnumerator WwwLoad()
    {
        WWW www = new WWW(Config.LoadPath);
        yield return www;
        if(string.IsNullOrEmpty(www.error))
        {
            SetData(www);
            Complete();
        }
        else
        {
            Error();
        }
    }
    private void SetData(WWW www)
    {
        object o = null;
        if (typeof(T) == typeof(byte[]))
        {
            o= www.bytes;
            Data = (T)o;
        }
        else if (typeof(T)== typeof(AssetBundle))
        {
            o = www.assetBundle;
            Data = (T)o;
        }
        if (typeof(T) == typeof(AudioClip))
        {
            o = www.GetAudioClip();
            Data = (T)o;
        }
    }
}
