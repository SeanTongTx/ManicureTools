using System;
using System.Collections;
using UnityEngine;

public class Testtest : MonoBehaviour
{
    CoroutineIter iter;
    void Start()
    {
        CoroutineManager.Instance.StartCoroutine(testx0());
        //StartCoroutine(test0());
    }

    void Update()
    {
        if (!CoroutineManager.Instance.doUpdate(Time.deltaTime))
        {
            Debug.LogError("=======EEEEEEEE=======");
        }
    }

    private IEnumerator testx0()
    {
        Debug.Log("======={{{{ =======");
        yield return null;
        var handler = CoroutineManager.Instance.StartCoroutine(testx1());
        yield return new WaitForSeconds(3f);
        CoroutineManager.Instance.StopCoroutine(handler);
        Debug.Log("======= }}}} =======");
        yield return new WaitForSeconds(1f);
        Debug.Log("======= End =======");
    }

    private IEnumerator testx1()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            Debug.Log(">>> " + (i++));
        }
    }

    private IEnumerator test0()
    {
        yield return test1();
        Debug.Log("000000000000");
        yield return test2();
        Debug.Log("000000000000");
    }

    private IEnumerator test1()
    {
        Debug.Log("111111  : " + Time.time);
        yield return new WaitForSeconds(1f);
        Debug.Log("111111  : " + Time.time);
    }

    private IEnumerator test2()
    {
        Debug.Log("222222  : " + Time.time);
        yield return new WaitForSeconds(2f);
        Debug.Log("22222  : " + Time.time);
    }

    private IEnumerator testA()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(i);
            Debug.Log("-1-   " + CoroutineManager.time);
        }
        Debug.Log("-2-");
        yield return null;
        Debug.Log("-3-");
        yield return new CC();
        Debug.Log("-4-  " + CoroutineManager.time);
        yield return new WaitUntil(delegate {
            return CoroutineManager.time > 5;
        });
        Debug.Log("-5-  " + CoroutineManager.time);
        yield return new WaitWhile(delegate {
            return CoroutineManager.time < 8;
        });
        Debug.Log("-6-  " + CoroutineManager.time);

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
}