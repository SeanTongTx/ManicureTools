

namespace Assets.Tools.Script.Net.Downloader
{
    using SeanLib.Core;
    using UnityEngine;

    /// <summary>
    /// mov加载器
    /// </summary>
    public class MovLoader : LocalVersionEnableLoader
    {
        public MovLoader(DownLoadAgent config)
            : base(config)
        {
        }

        protected override string GetCacheName()
        {
            return "mov/"+base.GetCacheName()+".mov";
        }

        protected override Coroutine LoadFromLocal()
        {
          /*  try
            {
                LoadComplete();
            }
            catch (Exception)
            {
                ESFile.Delete(cacheName);
                throw;
            }*/
            //TODO:
            return null;
        }

        protected override Coroutine LoadFromResource()
        {
            return null;
        }

        public override bool HasLocal()
        {
            return ESFile.Exists(cacheName);
        }

        protected override void OnSaveToLocal()
        {
            base.OnSaveToLocal();
            ESFile.SaveRaw(www.bytes, cacheName);
        }
        
    }

}