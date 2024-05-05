using UnityEngine;
using Data;
using System.Collections.Generic;

/// <summary>
/// This class is responsible for broadcasting randomly
/// one of the info included in the accompanied JSON files
/// </summary>
public class Broadcaster : MonoBehaviour
{
    int randomIndex;
    string[] fileSelector = { "Image00001", "Image00003", "Image00006" };
    Dictionary<string, ObjectData> dataCache = new Dictionary<string, ObjectData>();
    JSONOperations jsonOperations;


    void Start()
    {
        jsonOperations = new JSONOperations();
        InvokeRepeating("BroadcastData", 0, 5);
    }

    void BroadcastData()
    {
        randomIndex = Random.Range(0, 3);

        if (!dataCache.TryGetValue(fileSelector[randomIndex], out ObjectData value))
        {
            dataCache.Add(fileSelector[randomIndex], jsonOperations.GetJsonDataFromFile(fileSelector[randomIndex]));
        }

        EventBroker.CallDataPropagation(dataCache[fileSelector[randomIndex]]);

    }

}
