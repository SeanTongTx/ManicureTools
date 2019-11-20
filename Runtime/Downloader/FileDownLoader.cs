using SeanLib.Core;
using SeanLib.Core.Event;
using SeanLib.Core.Sequence;
using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine.Networking;

public enum DownloadState
{
    idle,
    loading,
    complete,
    cancelled,
    error
}

public class WebClientTimeOut : WebClient
{
    protected override WebRequest GetWebRequest(Uri address)
    {
        var request= base.GetWebRequest(address);
        request.Timeout = 10000;
        return request;
    }
}

public class FileDownLoader : CommonProcess
{
    public bool Async;
    public Signal<FileDownLoader> OnCancel = new Signal<FileDownLoader>();

    DownloadState state = DownloadState.idle;
    public DownloadConfig config = new DownloadConfig();
    public bool isDone { get { return state == DownloadState.complete; } }

    public FileDownLoader(DownloadConfig config)
    {
        this.config = config;
    }

    public FileDownLoader()
    {
    }

    public override void Complete()
    {
        state = DownloadState.complete;
        base.Complete();
    }

    public override void Error()
    {
        state = DownloadState.error;
        base.Error();
    }

    public override void Execute()
    {
        if(state==DownloadState.loading)
        {
            ErrorStr = "loader is downloading" + config.URL;
            Error();
        }
        state = DownloadState.loading;
        if (Async)
        {
            CoroutineCall.Call(DownloadAsync);
        }
        else
        {
            using (WebClientTimeOut client = new WebClientTimeOut())
            {
                try
                {
                    client.DownloadFile(config.URL, config.CacheFilePath);
                }
                catch (Exception e)
                {
                    ErrorStr = e.ToString();
                    Error();
                    return;
                }
                Complete();
            }
        }

    }
    public IEnumerator DownloadAsync()
    {
        string tempPath = config.CacheFilePath + ".temp";
        UnityWebRequest headRequest = null;
        try
        {
            headRequest = UnityWebRequest.Head(config.URL);
        }
        catch (Exception e)
        {
            ErrorStr = e.ToString();
            Error();
            yield break;
        }
#if UNITY_2017_3_OR_NEWER
            yield return headRequest.SendWebRequest();
#else
        yield return headRequest.Send();
#endif
        if (headRequest.isNetworkError || headRequest.isHttpError)
        {
            ErrorStr = headRequest.error;
            Error();
            yield break;
        }
        var totalLength = long.Parse(headRequest.GetResponseHeader("Content-Length"));

        var dirPath = Path.GetDirectoryName(tempPath);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        FileStream fs;
        try
        {
            fs = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write);
        }
        catch (Exception e)
        {
            ErrorStr = e.ToString();
            Error();
            yield break;
        }
        var fileLength = fs.Length;

        if (fileLength < totalLength)
        {
            fs.Seek(fileLength, SeekOrigin.Begin);

            var request = UnityWebRequest.Get(config.URL);
            request.SetRequestHeader("Range", "bytes=" + fileLength + "-" + totalLength);
#if UNITY_2017_3_OR_NEWER
            request.SendWebRequest();
#else
            request.Send();
#endif
            if (request.isNetworkError || request.isHttpError)
            {
                ErrorStr = request.error;
                Error();
                yield break;
            }
            var index = 0;
            while (!request.isDone)
            {
                if (state == DownloadState.cancelled) break;
                yield return null;
                var buff = request.downloadHandler.data;
                if (buff != null)
                {
                    var length = buff.Length - index;
                    fs.Write(buff, index, length);
                    index += length;
                    fileLength += length;

                    if (fileLength == totalLength)
                    {
                        progress = 1f;
                    }
                    else
                    {
                        progress = fileLength / (float)totalLength;
                    }
                }
            }
        }
        else
        {
            progress = 1f;
        }

        fs.Close();
        fs.Dispose();
        if (progress >= 1f)
        {
            if(File.Exists(config.CacheFilePath))
            {
                File.Delete(config.CacheFilePath);
            }
            File.Move(tempPath, config.CacheFilePath);
            Complete();
        }
    }

    public void Cancel()
    {
        state = DownloadState.cancelled;
        OnCancel.Dispatch(this);
    }

}
