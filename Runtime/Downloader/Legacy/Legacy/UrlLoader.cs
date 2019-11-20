using SeanLib.Core;
using SeanLib.Core.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Tools.Script.Net.Downloader
{
    /// <summary>
    /// 加载器基类
    /// </summary>
    public abstract class UrlLoader : ILoader
    {
        public enum LoaderState
        {
            idle,
            loading,
            complete,
            error,
            suspend
        }
        #region Debug

        public int id;
        public DownLoadAgent.DataSource CurrentDataSource { get; private set; }

        public bool debug;
        #endregion
        public LoaderState State = LoaderState.idle;
        /// <summary>
        /// 默认加载重试次数
        /// </summary>
        public static int defaultAttempts = 1;
        #region Event
        /// <summary>
        /// 加载开始
        /// </summary>
        public Signal<ILoader> onLoadStart { get; private set; }
        /// <summary>
        /// 加载完成
        /// </summary>
        public Signal<ILoader> onLoadComplete { get; private set; }
        /// <summary>
        /// 加载失败
        /// </summary>
        public Signal<ILoader> onLoadError { get; private set; }
        /// <summary>
        /// 卸载本地缓存开始
        /// </summary>
        public Signal<ILoader> onUnloadLocalStart { get; private set; }
        /// <summary>
        /// 卸载本地缓存完成
        /// </summary>
        public Signal<ILoader> onUnloadLocalComplete { get; private set; }
        /// <summary>
        /// 卸载本地缓存错误
        /// </summary>
        public Signal<ILoader> onUnloadLocalError { get; private set; }
        #endregion
        /// <summary>
        /// 加载失败最大重试次数
        /// </summary>
        public int attempts { get; private set; }
        /// <summary>
        /// 设置最大重试次数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public UrlLoader SetAttempts(int value)
        {
            this.attempts = value;
            return this;
        }

        public DownLoadAgent config;

        /// <summary>
        /// 加载的url
        /// </summary>
        public string url
        {
            get
            {
                return CurrentDataSource.url;
            }
        }
        /// <summary>
        /// 加载的WWW
        /// </summary>
        public WWW www { get; protected set; }
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 加载器
        /// </summary>
        /// <param name="url">远程地址</param>
        /// <param name="version">如果version>=0则表示要缓存本地，同时如果本地以及存在相应版本或更高版本则从本地加载</param>
        protected UrlLoader(DownLoadAgent config)
        {
            this.attempts = defaultAttempts;

            onLoadStart = new Signal<ILoader>();
            onLoadComplete = new Signal<ILoader>();
            onLoadError = new Signal<ILoader>();
            onUnloadLocalStart = new Signal<ILoader>();
            onUnloadLocalComplete = new Signal<ILoader>();
            onUnloadLocalError = new Signal<ILoader>();
            this.config = config;
        }


        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 卸载本地资源
        /// </summary>
        public void DeletLocal()
        {
            try
            {
                onUnloadLocalStart.Dispatch(this);
                if (HasLocal())
                    OnUnloadLocal();
                onUnloadLocalComplete.Dispatch(this);
            }
            catch (Exception e)
            {
                onUnloadLocalError.Dispatch(this);
            }

        }
        /// <summary>
        /// 对比本地是否是更新的版本
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNewVersion()
        {
            return true;
        }
        /// <summary>
        /// 开始加载
        /// </summary>
        /// <param name="catchMemory">是否需要保存在内存，如果不需要，在抛出完成事件后会析构下载的资源</param>
        public void Load()
        {
            if (State == LoaderState.idle || State == LoaderState.error)
            {
                State = LoaderState.loading;
                onLoadStart.Dispatch(this);
                onLoadStart.Clear();
                CoroutineCall.Call(this.LoadingSequence);
            }
            else if(State==LoaderState.complete)
            {
                this.LoadComplete();
            }
        }
        /// <summary>
        /// 卸载内存数据
        /// </summary>
        public void Unload()
        {
            try
            {
                OnUnload();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
            }
        }
        IEnumerator LoadingSequence()
        {
            int i = 0;
            while (i < config.dataSources.Count)
            {
                CurrentDataSource = config.dataSources[i];
                if (debug) DebugConsole.Info("Loader", "加载URl", url, CurrentDataSource.ToString() + id.ToString());
                switch (CurrentDataSource.source)
                {
                    case DownLoadAgent.DataSourceType.Cache:
                        if(this.HasLocal())
                        {
                            Coroutine async = this.LoadFromLocal();
                            if (debug) DebugConsole.Info("Loader", "本地缓存", (async == null).ToString(), id.ToString());
                            //异步加载中
                            if (async != null)
                            {
                                yield return async;
                            }
                            if (this.State == LoaderState.complete)
                            {
                                this.LoadComplete();
                                yield break;
                            }
                            else
                            {
                                i++;
                                continue;
                            }
                        }
                        else
                        {
                            i++;
                            continue;
                        }
                    case DownLoadAgent.DataSourceType.Resource:
                        Coroutine Resasync = this.LoadFromResource();
                        if (debug) DebugConsole.Info("Loader", "资源文件", (Resasync == null).ToString(), id.ToString());
                        //异步加载中
                        if (Resasync != null)
                        {
                            yield return Resasync;
                        }
                        if (this.State == LoaderState.complete)
                        {
                            this.LoadComplete();
                            yield break;
                        }
                        else
                        {
                            i++;
                            continue;
                        }
                    case DownLoadAgent.DataSourceType.StreamAsset: 
                        try
                        {
                            www = new WWW(url);
                        }
                        catch (Exception e)
                        {
                            if (debug) DebugConsole.Info("Loader", "StreamAsset", "加载失败", id.ToString());
                            i++;
                            continue;
                        }
                        yield return www;
                        if (!String.IsNullOrEmpty(www.error))//出错
                        {
                            DebugConsole.Warning("Loader", "Server", this.url, www.error);
                            www = null;//直接释放
                            i++;
                            continue;
                        }
                        else if (www.isDone)
                        {
                            LoadComplete();
                            yield break;
                        }
                        i++;
                        continue;

                    case DownLoadAgent.DataSourceType.Server:
                        if (this.HasNewVersion())
                        {
                            try
                            {
                                www = new WWW(url);
                            }
                            catch (Exception e)
                            {
                                if (debug) DebugConsole.Info("Loader", "Server", "加载失败", id.ToString());
                                i++;
                                continue;
                            }
                            yield return www;
                            if (!String.IsNullOrEmpty(www.error)) //出错
                            {
                                DebugConsole.Warning("Loader", "Server", this.url, www.error);
                                www = null; //直接释放
                                i++;
                                continue;
                            }
                            else if (www.isDone)
                            {
                                LoadComplete();
                                yield break;
                            }
                        }
                        i++;
                        continue;

                }
            }
            this.LoadError();
        }

        /// <summary>
        /// 加载完成时
        /// </summary>
        protected void LoadComplete()
        {
            State = LoaderState.complete;
           if(debug)DebugConsole.Info("Loader", "加载完成", this.url, CurrentDataSource.ToString());
            OnLoadCompleteHandler();
            onLoadComplete.Dispatch(this);
            onLoadComplete.Clear();
            if(CurrentDataSource.source!=DownLoadAgent.DataSourceType.Cache)
            { 
            SaveToLocal();
                }
        }
        /// <summary>
        /// 加载出错时
        /// </summary>
        protected void LoadError(String ErrorString = "")
        {
            State = LoaderState.error;
            if (String.IsNullOrEmpty(ErrorString))
            {
                DebugConsole.Warning("Loader", "加载", this.url, www != null ? www.error : "www=null");
            }
            else
            {
                DebugConsole.Warning("Loader", "加载", this.url, ErrorString);
            }

            onLoadError.Dispatch(this);
            onLoadError.Clear();
            OnLoadErrorHandler();
        }

        /// <summary>
        /// 保存到本地
        /// </summary>
        protected virtual void SaveToLocal()
        {
            try
            {
                OnSaveToLocal();
            }
            catch (Exception e)
            {
                DebugConsole.Log(e);
                DeletLocal();
            }
        }
        //----------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 本地是否有缓存
        /// </summary>
        /// <returns></returns>
        public abstract bool HasLocal();

        /// <summary>
        /// 如何卸载内存资源
        /// </summary>
        protected virtual void OnUnload()
        {
            www = null;
        }
        /// <summary>
        /// 完成加载后，做的相应处理
        /// </summary>
        protected virtual void OnLoadCompleteHandler() { }
        /// <summary>
        /// 出错处理
        /// </summary>
        protected virtual void OnLoadErrorHandler() { }

        /// <summary>
        /// 保存到本地磁盘的方法
        /// </summary>
        protected abstract void OnSaveToLocal();
        /// <summary>
        /// 从本地磁盘移除
        /// </summary>
        protected abstract void OnUnloadLocal();
        /// <summary>
        /// 从本地读取
        /// 如果是异步加载 需要返回unity coroutine 对象
        /// 结果需要设置 loader.state 
        /// </summary>
        protected abstract Coroutine LoadFromLocal();

        /// <summary>
        /// 从安装包的资源文件中读取
        /// 如果是异步加载 需要返回unity coroutine 对象
        /// 结果需要设置 loader.state 
        protected abstract Coroutine LoadFromResource();

        public void SaveLocal()
        {
            SaveToLocal();
        }
    }
}