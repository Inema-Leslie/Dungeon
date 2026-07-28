using System.Collections;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SaveManagerTests
{
    private GameObject testObject;
    private SaveManager saveManager;
    private string savePath;
    private string backupJson;
    private bool hadExistingSave;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        if (SaveManager.Instance != null)
            Object.DestroyImmediate(SaveManager.Instance.gameObject);

        testObject = new GameObject("TestSaveManager");
        saveManager = testObject.AddComponent<SaveManager>();

        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        hadExistingSave = File.Exists(savePath);
        if (hadExistingSave)
        {
            backupJson = File.ReadAllText(savePath);
        }

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (hadExistingSave)
        {
            File.WriteAllText(savePath, backupJson);
        }
        else if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        Object.DestroyImmediate(testObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator SaveGame_ThenLoadGame_ReturnsMatchingData()
    {
        SaveData original = new SaveData
        {
            levelStatus = new int[] { 2, 1, 0, 0, 0 },
            playerHealth = 42f,
            hasShield = true,
            currentLevel = 1
        };

        saveManager.SaveGame(original);
        SaveData loaded = saveManager.LoadGame();

        Assert.IsNotNull(loaded, "LoadGame should return data after a successful save.");
        Assert.AreEqual(original.playerHealth, loaded.playerHealth, "Player health should persist correctly.");
        Assert.AreEqual(original.hasShield, loaded.hasShield, "hasShield should persist correctly.");
        Assert.AreEqual(original.currentLevel, loaded.currentLevel, "currentLevel should persist correctly.");
        Assert.AreEqual(original.levelStatus[0], loaded.levelStatus[0], "levelStatus array should persist correctly.");

        yield return null;
    }
}