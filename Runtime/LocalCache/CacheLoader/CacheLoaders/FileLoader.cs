using SeanLib.Core;
using System;
using System.Collections;
using System.IO;

namespace LocalLoadProcess
{
    public class FileLoader : CacheLoaderBase<byte[]>
    {
        public int bufferSize = 256 * 1024;//256k 字节/一帧
        FileStream fs;
        MemoryStream ms;
        public FileLoader()
        {

        }
        public FileLoader(LoaderConfig config) : base(config)
        {
        }

        public override void Execute()
        {
            if (Async)
            {
                CoroutineCall.Call(LoadFromFile);
            }
            else
            {
                try
                {

                    Data = File.ReadAllBytes(Config.LoadPath);
                    Complete();
                }
                catch(Exception e)
                {
                    ErrorStr = e.Message;
                    Error();
                }
            }
        }
        private IEnumerator LoadFromFile()
        {
            byte[] buffer = new byte[bufferSize];
            try
            {
                fs = new FileStream(Config.LoadPath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true);
                ms = new MemoryStream();
            }catch(Exception e)
            {
                ErrorStr = e.Message;
                if (fs!=null)
                fs.Dispose();
                if (ms != null)
                    ms.Dispose();
                Error();
                yield break;
            }
            //原因不明：异步读取 依然同步 所以使用coroutine 分拆成异步
            //fs.BeginRead(buffer, 0, buffer.Length, OnCompleteRead, fs);
            while (fs.Position < fs.Length)
            {
                int readByts = fs.Read(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, readByts);
                yield return fs.Position < fs.Length;
            }
            Data = new byte[ms.Length];
            ms.Read(Data, 0, (int)ms.Length);
            fs.Dispose();
            ms.Dispose();
            Complete();
        }

    }
}
