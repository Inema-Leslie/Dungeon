using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PlayerProfile
{
    public string playerName = "";
    public bool hasBeatenGame = false;
}

public class PlayerProfileManager : MonoBehaviour
{
    public static PlayerProfileManager Instance { get; private set; }

    [Header("JSONBin.io Config")]
    [SerializeField] private string binId = "6a69e71cda38895dfe9f888f";
    [SerializeField] private string masterKey = "$2a$10$nsy4aWrgLY1AShEGcP3ESulwOwrwH3u7GaJquoB.NkC.htydA9x8G ";

    public PlayerProfile CurrentProfile { get; private set; } = new PlayerProfile();

    private string BaseUrl => $"https://api.jsonbin.io/v3/b/{binId}";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(FetchProfile());
    }

    public IEnumerator FetchProfile()
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"{BaseUrl}/latest"))
        {
            request.SetRequestHeader("X-Master-Key", masterKey);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                
                JsonBinWrapper wrapper = JsonUtility.FromJson<JsonBinWrapper>(json);
                CurrentProfile = wrapper.record;
                Debug.Log($"[PlayerProfileManager] Fetched profile: {CurrentProfile.playerName}, hasBeatenGame: {CurrentProfile.hasBeatenGame}");
            }
            else
            {
                Debug.LogWarning($"[PlayerProfileManager] Fetch failed: {request.error}. Using local default profile.");
            }
        }
    }

    public void SetPlayerName(string name)
    {
        CurrentProfile.playerName = name;
        StartCoroutine(SaveProfile());
    }

    public void MarkGameBeaten()
    {
        CurrentProfile.hasBeatenGame = true;
        StartCoroutine(SaveProfile());
    }

    private IEnumerator SaveProfile()
    {
        string json = JsonUtility.ToJson(CurrentProfile);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request = new UnityWebRequest(BaseUrl, "PUT"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("X-Master-Key", masterKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[PlayerProfileManager] Profile saved successfully.");
            }
            else
            {
                Debug.LogWarning($"[PlayerProfileManager] Save failed: {request.error}");
            }
        }
    }

    [Serializable]
    private class JsonBinWrapper
    {
        public PlayerProfile record;
    }
}