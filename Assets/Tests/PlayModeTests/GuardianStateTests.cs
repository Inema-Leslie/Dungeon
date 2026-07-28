using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GuardianStateTests
{
    private GameObject testObject;
    private GuardianBehaviour guardian;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        testObject = new GameObject("TestGuardian");
        guardian = testObject.AddComponent<GuardianBehaviour>(); // Health auto-added via RequireComponent
        yield return null; // let Start() run, entering GuardianBlockState
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.DestroyImmediate(testObject);
        yield return null;
    }

    private object GetCurrentState()
    {
        FieldInfo field = typeof(GuardianBehaviour).GetField("currentState", BindingFlags.NonPublic | BindingFlags.Instance);
        return field.GetValue(guardian);
    }

    [UnityTest]
    public IEnumerator TenHits_TransitionsFromBlockStateToDeadState()
    {
        Assert.AreEqual("GuardianBlockState", GetCurrentState().GetType().Name,
            "Guardian should start in GuardianBlockState.");

        Health guardianHealth = testObject.GetComponent<Health>();
        for (int i = 0; i < 10; i++)
        {
            guardianHealth.TakeDamage(1f);
        }
        yield return null;

        Assert.AreEqual("GuardianDeadState", GetCurrentState().GetType().Name,
            "Guardian should transition to GuardianDeadState after exactly 10 hits.");
    }
}