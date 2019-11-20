
namespace Assets.Tools.Script.Net.Downloader
{
    using SeanLib.Core;

    /// <summary>
    /// 带有本地版本管理功能的加载器
    /// </summary>
    public abstract class LocalVersionEnableLoader : UrlLoader
    {
        IVersion version
        {
            get
            {
                return config.version;
            }
        }
        protected LocalVersionEnableLoader(DownLoadAgent config) : base(config)
        {
        }
        /// <summary>
        /// 本地保存的名字
        /// </summary>
        public string cacheName
        {
            get { return _cacheName ?? (_cacheName = GetCacheName()); }
            set { _cacheName = value; }
        }
        private string _cacheName;
        public override bool HasNewVersion()
        {
            if(version!=null)
             return version.CheckChange();
            return true;
        }
        protected override void OnSaveToLocal()
        {
            if (version != null)
            version.Merge();
        }

        protected override void OnUnloadLocal()
        {
        }
        /// <summary>
        /// 创建LocalName
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCacheName()
        {
            /* if (string.IsNullOrEmpty(config.AssetGUID))
             {
                 throw new Exception("AssetGUID = NUll");
             }
             char[] chars = Path.GetInvalidFileNameChars();
             string localname = config.AssetGUID;
             if (localname.IndexOfAny(chars) > 0)
             {
                 //非法文件名 转换为guid
                 byte[] bs = Encoding.UTF8.GetBytes(localname);
                 localname = Convert.ToBase64String(bs);
             }
             return localname;*/
            return config.GUID;
        }
    }
}