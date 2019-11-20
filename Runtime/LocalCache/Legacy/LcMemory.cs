using System.Collections;
using System.Collections.Generic;

using Assets.Tools.Script.Serialized.LocalCache.Core;

using UnityEngine;

public class LcMemory<T>: UnityLocalCache<T>
{
    public T cache;
    public LcMemory(string name)
        : base(name)
    {
    }
    public LcMemory(string name, T defaultValue)
        : base(name, defaultValue)
    {
    }

    public override bool HasCache()
    {
        return cache!=null&&default(T).Equals(cache);
    }

    public override void DeleteCache()
    {
        cache = default(T);
    }

    protected override T GetLocalCache()
    {
        return cache;
    }

    protected override void SaveLocalCache(T value)
    {
        cache = value;
    }
}
