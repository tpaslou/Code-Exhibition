using System;
using UnityEngine;

namespace Data {
    /// <summary>
    /// In this class we can find functions responsible for our
    /// JSON files opperations.
    /// </summary>
    public class JSONOperations
    {

        public ObjectData GetJsonDataFromFile(string filePath)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(filePath);

            if(jsonFile == null)
            {
                Debug.LogError("ERROR reading JSON file.");
                return null;
            }

            try
            {
                ObjectData data = JsonUtility.FromJson<ObjectData>(jsonFile.text);
                Debug.Log("JSON deserialization succeeded.");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to deserialize JSON: " + e.Message);
                return null;
            }

            
        }

        public HTTPData DeserializeMockData(string message)
        {
            try
            {
                HTTPData data = JsonUtility.FromJson<HTTPData>(message);
                Debug.Log("JSON deserialization succeeded.");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to deserialize JSON: " + e.Message);
                return null;
            }
        }
        
    }

}