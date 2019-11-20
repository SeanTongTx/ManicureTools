using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewEditorTest
{
    [Test]
    public void EditorTest()
    {
        //Iterate (test01 (0));  
        Iterate(testA(), obj => {
            if (obj is YieldInstruction)
            {
                Debug.LogFormat("!!YI: " + obj.GetType().Name);
            }
            else if (obj is CustomYieldInstruction)
            {
                Debug.LogFormat("!!CYI: {0}, isIt? {1} ", obj.GetType().Name, obj is IEnumerator);
            }
        });
    }

    private void Iterate(IEnumerator _it, System.Action<object> action = null)
    {
        Stack<IEnumerator> stack = new Stack<IEnumerator>();
        var it = _it;
        while (it != null)
        {
            while (it.MoveNext())
            {
                if (action != null)
                    action(it.Current);
                if (it.Current is IEnumerator)
                {
                    stack.Push(it);
                    it = it.Current as IEnumerator;
                    continue;
                }
            }
            it = stack.Count > 0 ? stack.Pop() : null;
        }
    }
    float time = 0;
    private IEnumerator testA()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("-1-");
            yield return new WaitForSeconds(i);
        }
        Debug.Log("-2-");
        yield return null;
        Debug.Log("-3-");
        yield return new CC();
        Debug.Log("-4-");
        yield return new WaitUntil(delegate {
            time += 0.3f;
            Debug.Log(">Time: " + time);
            return time > 10;
        });
        Debug.Log("-5-");
        yield return new WaitWhile(delegate {
            time += 0.3f;
            Debug.Log(">Time: " + time);
            return time < 15;
        });
    }

    class CC : CustomYieldInstruction
    {
        int i = 0;
        public override bool keepWaiting
        {
            get
            {
                Debug.Log("CC > " + i);
                return i++ < 10;
            }
        }
    }

    private IEnumerator test01(int l)
    {
        Debug.Log("----test01-{{{");
        print(1, l);
        yield return test02(l + 1);
        print(2, l);
        yield return null;
        print(3, l);
        yield return test03(l + 1);
        print(4, l);
        Debug.Log("}}}test01-----");
    }

    private IEnumerator test02(int l)
    {
        Debug.Log("----test02-{{{");
        print(1, l);
        yield return test03(l + 1);
        print(2, l);
        yield return null;
        print(3, l);
        yield return test03(l + 1);
        print(4, l);
        Debug.Log("}}}test02-----");

    }

    private IEnumerator test03(int l)
    {
        Debug.Log("----test03-{{{");
        print(1, l);
        yield return null;
        Debug.Log("}}}test03-----");
    }

    private void print(int num, int level)
    {
        string rt = "";
        for (int i = 0; i < level; i++)
        {
            rt += "   ";
        }
        Debug.LogFormat("{0}{1}", rt, num);
    }
}