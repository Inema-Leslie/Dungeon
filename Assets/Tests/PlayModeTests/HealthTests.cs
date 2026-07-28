using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class HealthTests
{
    private GameObject testObject;
    private Health health;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        testObject = new GameObject("TestHealth");
        health = testObject.AddComponent<Health>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.DestroyImmediate(testObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TakeDamage_ReducesCurrentHealthByExactAmount()
    {
        float startingHealth = health.CurrentHealth;

        health.TakeDamage(30f);

        Assert.AreEqual(startingHealth - 30f, health.CurrentHealth,
            "CurrentHealth should decrease by exactly the damage amount.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator TakeDamage_CannotReduceHealthBelowZero()
    {
        health.TakeDamage(9999f);

        Assert.AreEqual(0f, health.CurrentHealth,
            "CurrentHealth should clamp at 0, never go negative.");
        yield return null;
    }
}