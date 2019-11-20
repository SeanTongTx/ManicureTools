using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class CoroutineManager
{
    public Clock clock { get; private set; }
    private HashSet<CoroutineIter> cIters;
    private List<CoroutineIter> itAddTemp;
    private List<CoroutineIter> itDelTemp;

    private CoroutineManager()
    {
        cIters = new HashSet<CoroutineIter>();
        itAddTemp = new List<CoroutineIter>();
        itDelTemp = new List<CoroutineIter>();
        clock = new Clock();
    }

    private static CoroutineManager instance = null;
    public static CoroutineManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CoroutineManager();
            }
            return instance;
        }
    }

    public static float time
    {
        get
        {
            return Instance.clock.time;
        }
    }
    public CoroutineIter StartCoroutine(IEnumerator _it)
    {
        var iter = new CoroutineIter(_it);
        itAddTemp.Add(iter);
        return iter;
    }

    public void StopCoroutine(CoroutineIter cIter)
    {
        if (cIter != null)
        {
            itDelTemp.Add(cIter);
        }
    }

    public bool doUpdate(float dt)
    {
        if (itAddTemp.Count > 0)
        {  // 添加 
            itAddTemp.ForEach(it => { cIters.Add(it); });
            itAddTemp.Clear();
        }
        if (clock.tick(dt) && cIters.Count > 0)
        {
            foreach (var i in cIters)
            {
                i.doUpdate(dt);
            }
            cIters.RemoveWhere(i => i.isEnd);
        }
        if (itDelTemp.Count > 0)
        { // 删除
            itDelTemp.ForEach(it => { cIters.Remove(it); });
            itDelTemp.Clear();
        }
        return cIters.Count > 0;
    }

    public class Clock
    {
        public int frame { get; private set; }
        public float time { get; private set; }
        public float dt { get; private set; }
        public void reset()
        {
            time = dt = 0;
        }
        public bool tick(float _dt)
        {
            if (frame < Time.frameCount)
            {
                dt = _dt;
                time += _dt;
                frame = Time.frameCount;
                return true;
            }
            return false;
        }
    }
}

public class CoroutineIter
{
    public bool isEnd { get; private set; }
    Stack<IEnumerator> stack = new Stack<IEnumerator>();
    IEnumerator it;
    public CoroutineIter(IEnumerator _it)
    {
        it = _it;
        isEnd = it == null;
    }

    public void doUpdate(float dt)
    {
        if (!isEnd)
        {
            if (it.MoveNext())
            {
                dealCurrent(it.Current);
            }
            else
            {
                it = stack.Count > 0 ? stack.Pop() : null;
            }
            isEnd = it == null;
        }
    }

    private void dealCurrent(object cur)
    {
        if (it.Current is IEnumerator)
        {
            stack.Push(it);
            it = it.Current as IEnumerator;
        }
        else if (it.Current is WaitForSeconds)
        {
            stack.Push(it);
            it = new MyWaitForSecond(it.Current as WaitForSeconds);
        }
    }
}

class MyWaitForSecond : CustomYieldInstruction
{
    private float duration;
    private float startTime;
    CoroutineManager cm;
    public MyWaitForSecond(WaitForSeconds wfs)
    {
        duration = GetPrivateFieldValue<float>(wfs, "m_Seconds");
        cm = CoroutineManager.Instance;
        startTime = cm.clock.time;
    }

    public override bool keepWaiting
    {
        get
        {
            return (cm.clock.time - startTime) < duration;
        }
    }

    private static T GetPrivateFieldValue<T>(object obj, string propName)
    {
        if (obj == null)
            throw new ArgumentNullException("obj");
        Type t = obj.GetType();
        FieldInfo fi = null;
        while (fi == null && t != null)
        {
            fi = t.GetField(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            t = t.BaseType;
        }
        if (fi == null)
            throw new ArgumentOutOfRangeException("propName",
                                                  string.Format("Field {0} was not found in Type {1}", propName,
                                                                obj.GetType().FullName));
        return (T)fi.GetValue(obj);
    }
}