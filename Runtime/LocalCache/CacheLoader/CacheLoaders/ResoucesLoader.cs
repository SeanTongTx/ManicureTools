
using LocalLoadProcess;
using SeanLib.Core;
using System.Collections;
using UnityEngine;

public class ResoucesLoader<T> : CacheLoaderBase<T> where T :UnityEngine.Object
{
    public ResoucesLoader()
    {
    }

    public ResoucesLoader(LoaderConfig config) : base(config)
    {
    }

    public override void Execute()
    {
        if(Async)
        {
            CoroutineCall.Call(AsyncLoad);
        }
        else
        {
            Data = Resources.Load<T>(Config.LoadPath);
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

    private IEnumerator AsyncLoad()
    {
       ResourceRequest request= Resources.LoadAsync<T>(Config.LoadPath);
        yield return request;
        if(request.isDone)
        {
            Data = request.asset as T;
            if(Data==null)
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
