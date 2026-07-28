using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ArrowPoolTests
{
    private GameObject poolObject;
    private ArrowPool arrowPool;
    private GameObject fakeArrowPrefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        if (ArrowPool.Instance != null)
            Object.DestroyImmediate(ArrowPool.Instance.gameObject);

        fakeArrowPrefab = new GameObject("FakeArrowPrefab");
        fakeArrowPrefab.AddComponent<Arrow>();

        
        poolObject = new GameObject("TestArrowPool");
        poolObject.SetActive(false);
        arrowPool = poolObject.AddComponent<ArrowPool>();

        typeof(ArrowPool).GetField("arrowPrefab", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(arrowPool, fakeArrowPrefab);
        typeof(ArrowPool).GetField("poolSize", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(arrowPool, 2);

        poolObject.SetActive(true); 
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.DestroyImmediate(poolObject);
        Object.DestroyImmediate(fakeArrowPrefab);
        yield return null;
    }

    [UnityTest]
    public IEnumerator GetArrow_ReturnsAnActiveArrow()
    {
        Arrow arrow = arrowPool.GetArrow();

        Assert.IsNotNull(arrow, "GetArrow should never return null when the pool has arrows available.");
        Assert.IsTrue(arrow.gameObject.activeSelf, "An arrow retrieved from the pool should be active.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator ReturnArrow_DeactivatesAndAllowsReuse()
    {
        Arrow first = arrowPool.GetArrow();
        arrowPool.ReturnArrow(first);

        Assert.IsFalse(first.gameObject.activeSelf,
            "Returning an arrow to the pool should deactivate it.");

       
        arrowPool.GetArrow();
        Arrow reused = arrowPool.GetArrow();

        Assert.AreSame(first, reused,
            "The pool should reuse the previously returned arrow rather than creating a new instance.");
        yield return null;
    }
}