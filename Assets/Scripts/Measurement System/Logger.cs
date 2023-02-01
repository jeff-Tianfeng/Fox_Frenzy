using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles logging the player data, which includes angle and distance over time
/// </summary>
public class Logger : MonoBehaviour
{
    public GameController gameController;

    private const float logInterval = 0.1f;

    private string playerDataPath;
    private string playerDataFileName;
    private string playerID;

    private PlayData playData;
    private PlaySession sessionInProgress;
    private FoxSearch searchInProgress;

    private Coroutine activeCoroutine;


    void Awake()
    {
        //get player ID
#if UNITY_EDITOR
        playerID = "Editor";
#else
        string[] args = Environment.GetCommandLineArgs();
        if (args.Length >= 2) 
            playerID = args[1];
        else
            playerID = "ID_Not_Given";
#endif

        playerDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/KL2C/Player Data/" + playerID + "/";
        playerDataFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".json";

        //create data file directory in case it doesnt exist
        Directory.CreateDirectory(playerDataPath);

        playData = LoadPlayData();

        SavePlayData(playData);
    }

    void OnEnable()
    {
        StartSession();
    }

    void OnDisable()
    {
        EndSession();
    }

    public void StartSession()
    {
        sessionInProgress = new PlaySession();
    }

    public void EndSession()
    {
        EndSearch();

        playData.AddSession(sessionInProgress);

        SavePlayData(playData);
    }

    public void StartNewSearch()
    {
        searchInProgress = new FoxSearch();

        activeCoroutine = StartCoroutine(LogPlayer());
    }

    public void EndSearch()
    {
        StopCoroutine(activeCoroutine);

        if (searchInProgress != null) sessionInProgress.AddSearch(searchInProgress);
        searchInProgress = null;
    }

    IEnumerator LogPlayer()
    {
        while (true)
        {
            float angle = gameController.GetPlayerAngleToFox();
            float distance = gameController.GetPlayerDistanceToFox();

            searchInProgress.AddDataEntry(angle, distance);

            yield return new WaitForSeconds(logInterval);
        }

    }

    /// <summary>
    /// Loads the player's data from a file
    ///
    /// <para>If the player does not already have a file, a new PlayData object is created</para>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private PlayData LoadPlayData()
    {
        PlayData data;

        if (File.Exists(playerDataPath + playerDataFileName))
        {
            using (StreamReader reader = new StreamReader(playerDataPath + playerDataFileName))
            {
                data = (PlayData)JsonUtility.FromJson(reader.ReadToEnd(), typeof(PlayData));
            }
        }
        else
        {
            data = new PlayData();
        }

        return data;
    }

    private void SavePlayData(PlayData data)
    {
        using (StreamWriter writer = new StreamWriter(playerDataPath + playerDataFileName))
        {
            writer.Write(JsonUtility.ToJson(data));
        }
    }

    #region JSON Serialisable objects for storing player data

    /// <summary>
    /// Playdata contains data of all play sessions on a particular day
    /// </summary>
    [Serializable]
    class PlayData
    {
        public string date;
        public List<PlaySession> sessions = new List<PlaySession>();

        public PlayData()
        {
            date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        public void AddSession(PlaySession session)
        {
            sessions.Add(session);
        }
    }

    /// <summary>
    /// PlaySession describes one session - i.e. every time the player presses "play" or "restart"
    /// </summary>
    [Serializable]
    class PlaySession
    {
        public string time;
        public List<FoxSearch> searches = new List<FoxSearch>();

        public PlaySession()
        {
            time = DateTime.Now.ToString("HH:mm:ss");
        }

        public void AddSearch(FoxSearch search)
        {
            searches.Add(search);
        }
    }

    /// <summary>
    /// FoxSearch describes one search - every time the player goes from the starting position
    ///
    /// <para>A FoxSearch may not end with the fox being found - a search also ends if the player wanders too far</para>
    /// </summary>
    [Serializable]
    class FoxSearch
    {
        public List<DataEntry> data = new List<DataEntry>();

        public void AddDataEntry(float angle, float distance)
        {
            data.Add(new DataEntry() { angle = angle, distance = distance });
        }
    }

    /// <summary>
    /// DataEntry contains data on one particular point in time.
    /// </summary>
    [Serializable]
    struct DataEntry
    {
        public float angle;
        public float distance;
    }
    #endregion
}
