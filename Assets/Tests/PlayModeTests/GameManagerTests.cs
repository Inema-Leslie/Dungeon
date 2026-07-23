using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameManagerTests
{
    private GameObject testObject;
    private GameManager gameManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        if (GameManager.Instance != null)
            Object.DestroyImmediate(GameManager.Instance.gameObject);

        testObject = new GameObject("TestGameManager");
        gameManager = testObject.AddComponent<GameManager>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.DestroyImmediate(testObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Level1_IsUnlockedByDefault()
    {
        Assert.IsTrue(gameManager.IsLevelUnlocked(0),
            "Level 1 (index 0) should always be unlocked from the start.");
        yield return null;
    }

    [UnityTest]
    public IEnumerator Level2_IsLockedUntilLevel1Completed()
    {
        Assert.IsFalse(gameManager.IsLevelUnlocked(1),
            "Level 2 (index 1) should be locked before Level 1 is completed.");

        gameManager.CompleteLevel(0);

        Assert.IsTrue(gameManager.IsLevelUnlocked(1),
            "Level 2 (index 1) should unlock immediately after Level 1 is completed.");
        yield return null;
    }
}