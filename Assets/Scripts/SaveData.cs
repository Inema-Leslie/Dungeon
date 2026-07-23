using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int[] levelStatus;          
    public List<string> inventory = new List<string>();
    public float playerHealth = 100f;
    public bool hasShield = false;

    public float musicVolume = 0.8f;
    public float sfxVolume = 0.8f;
}