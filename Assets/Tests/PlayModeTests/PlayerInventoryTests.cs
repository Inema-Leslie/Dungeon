using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerInventoryTests
{
    private GameObject testObject;
    private PlayerInventory inventory;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        if (PlayerInventory.Instance != null)
            Object.DestroyImmediate(PlayerInventory.Instance.gameObject);

        testObject = new GameObject("TestPlayerInventory");
        inventory = testObject.AddComponent<PlayerInventory>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.DestroyImmediate(testObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator CollectingItem_AddsItToInventory()
    {
        GameEvents.RaiseItemCollected("Weapon");
        yield return null;

        Assert.IsTrue(inventory.HasItem("Weapon"),
            "Inventory should report having the item after it's collected.");
    }

    [UnityTest]
    public IEnumerator CollectingSameItemTwice_DoesNotAddDuplicate()
    {
        GameEvents.RaiseItemCollected("Weapon");
        GameEvents.RaiseItemCollected("Weapon");
        yield return null;

        int count = inventory.CollectedItems.FindAll(item => item == "Weapon").Count;

        Assert.AreEqual(1, count,
            "Inventory should not contain duplicate entries for the same item.");
    }
}