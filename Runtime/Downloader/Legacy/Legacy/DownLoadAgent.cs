using SeanLib.Core;
using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// 下载地址配置
/// </summary>
[Serializable]
public class DownLoadAgent
{
    public IVersion version;
    public string GUID;
    public enum LocalCacheType
    {
        Memory,
        PlayerPrefs,
        File
    }
    [Serializable]
    public class DataSource
    {
        [SerializeField]
        public DataSourceType source;
        [SerializeField]
        [Multiline]
        public string url;
    }
    public enum DataSourceType
    {
        Server,
        Cache,
        StreamAsset,
        Resource
    }
    [Tooltip("本地缓存策略")]
    public LocalCacheType cacheType = LocalCacheType.Memory;
    [Tooltip("数据源")]
    public List<DataSource> dataSources = new List<DataSource>();

    public string GetCachePath()
    {
        switch (cacheType)
        {
            case LocalCacheType.Memory: return GUID;
            case LocalCacheType.PlayerPrefs: return GUID;
            case LocalCacheType.File: return LoadPath.saveLoadPath + GUID;
        }
        return null;
    }
}