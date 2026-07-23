using System;
using UnityEngine;

public static class GameEvents
{
    // ---Player events ---
    public static event Action<float, float> OnHealthChanged; // (current,max)
    public static event Action OnPlayerDied;
    
    // ---Progression events ---
    public static event Action<string> OnItemCollected; //shield or weapon
    public static event Action OnShieldCollected;
    public static event Action OnChainsBroken;
    public static event Action OnDoorOpened;
    public static event Action<string> OnEnemyDefeated;
    public static event Action<int> OnLevelCompleted;
    public static event Action<int> OnLevelUnlocked;

    // --- Game state ---
    public static event Action <string> OnGameStateChanged; //paused or gameover

    // ---Raise methods ---
    public static void RaiseHealthChanged(float current, float max) => OnHealthChanged?.Invoke(current, max);
    public static void RaisePlayerDied() => OnPlayerDied?.Invoke();
    public static void RaiseShieldCollected() => OnChainsBroken?.Invoke();
    public static void RaiseChainsBroken() => OnChainsBroken?.Invoke();
    public static void RaiseDoorOpened() => OnDoorOpened?.Invoke();
    public static void RaiseEnemyDefeated(string enemyType) => OnEnemyDefeated?.Invoke(enemyType);
    public static void RaiseLevelCompleted(int levelIndex) => OnLevelCompleted?.Invoke(levelIndex);
    public static void RaiseLevelUnlocked(int levelIndex) => OnLevelUnlocked?.Invoke(levelIndex);
    public static void RaiseGameStateChanged (string state) => OnGameStateChanged?.Invoke(state);
    public static void RaiseItemCollected(string itemId) => OnItemCollected?.Invoke(itemId);
}

