using HTTPMocking;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Data;
using System.Collections.Generic;
/// <summary>
/// This class is responsible for calling HTTP communication
/// and creating the objects indicated in Task 2.
/// </summary>
public class DynamicObjects : MonoBehaviour
{
    #region variables
    [Header("Mocking Service")]
    [Tooltip("Set the desired mocking URI")]
    [SerializeField]
    string URI;
    [Header("Prefab Refferences")]
    [Space]
    [SerializeField]
    GameObject labelPrefab;
    [SerializedDictionary("Object Name","Prefab")]
    public SerializedDictionary<string,GameObject> NamePrefabDictionary;

    HTTPCommunication httpCommunication;
    JSONOperations jsonOperations;
    HTTPData httpData;
    Dictionary<string,bool> spawnedObjects = new Dictionary<string, bool>();
    #endregion

    #region Event Driven Methods
    void OnEnable()
    {
        httpCommunication = new HTTPCommunication(URI);
        jsonOperations = new JSONOperations();
        httpCommunication.OnHTTPSuccess += OnHTTPDataRecieved ;
        httpCommunication.OnHTTPError += OnHTTPError;
        EventBroker.ObjectDetroyed += OnObjectDestroyed;
        EventBroker.ObjectClick += OnObjectClicked;
    }

    private void OnObjectDestroyed(int obj)
    {
        int id = obj;
        httpCommunication.RequestData(id);
        
    }

    private void OnObjectClicked(string obj)
    {
        spawnedObjects[obj] = false;
    }

    private void OnHTTPError(string obj)
    {
        Debug.LogError(obj);
    }

    private void OnHTTPDataRecieved(string message)
    {
        Debug.Log("Succesfully recieved data from the mocking service");
        httpData = jsonOperations.DeserializeMockData(message);
        SpawnObjects();
    }

    void OnDisable()
    {
        httpCommunication.OnHTTPSuccess -= OnHTTPDataRecieved;
        httpCommunication.OnHTTPError -= OnHTTPError;
        EventBroker.ObjectDetroyed -= OnObjectDestroyed;
        EventBroker.ObjectClick -= OnObjectClicked;

    }
    #endregion

    #region Functionality Methods

    private void Start()
    {
        foreach(KeyValuePair<string,GameObject> pair in NamePrefabDictionary) {

            spawnedObjects.Add(pair.Key, false);
        }

    }

    void SpawnObjects()
    {

        if (!NamePrefabDictionary[httpData.model_name])
            Debug.LogError("Error finding prefab in the Dictionary");

        if (!spawnedObjects[httpData.model_name])
        {
            GameObject spawnedObject = Instantiate(NamePrefabDictionary[httpData.model_name]);
            spawnedObject.transform.SetPositionAndRotation(new Vector3(httpData.position.x, httpData.position.y,
                httpData.position.z), Quaternion.Euler(0,180,0) );
            spawnedObject.AddComponent<ClickBehaviour>();
            spawnedObjects[httpData.model_name] = true;
            AdaptObjectSize(spawnedObject, .5f);
            CreateObjectLabels(spawnedObject);
        }
    }
    

    void AdaptObjectSize(GameObject gameObject, float boxSize) //I am using a float instead of a Vector3 since it is 0.5x0.5x0.5
    { 
        Bounds bounds = new Bounds();

        foreach(Transform child in gameObject.transform) {

            SkinnedMeshRenderer renderer = child.GetComponent<SkinnedMeshRenderer>();

            if (renderer)
            {
                if(bounds.size == Vector3.zero)
                {
                    bounds = renderer.bounds;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }

        Vector3 modelSize = bounds.size;
        Vector3 scale = new Vector3(boxSize / modelSize.x, boxSize / modelSize.y, boxSize / modelSize.z);
        gameObject.transform.localScale = scale;
        
    }

    void CreateObjectLabels(GameObject parent)
    {

        //The distance is 0.5 since the transform box has size 0.5 x 0.5 x 0.5
        Vector3 position = parent.transform.position;

        //Left
        GameObject leftLabel = Instantiate(labelPrefab);
        leftLabel.transform.localPosition = new Vector3(position.x + 0.8f, position.y, position.z);
        leftLabel.GetComponent<Label>().SetLabelData("Attributes : \n" + string.Join("\n",httpData.attributes) , httpData.model_name );

        //Right
        GameObject rightLabel = Instantiate(labelPrefab);
        rightLabel.transform.localPosition = new Vector3(position.x -0.8f, position.y, position.z);
        rightLabel.GetComponent<Label>().SetLabelData("Position : \n" +
            "X : "  + httpData.position.x +
            "\n Y : " + httpData.position.y +
            "\n Z : " + httpData.position.z,
            httpData.model_name);

        //Up
        GameObject upLabel = Instantiate(labelPrefab);
        upLabel.transform.localPosition = new Vector3(position.x, position.y+0.8f, position.z);
        upLabel.GetComponent<Label>().SetLabelData("Name : \n" + httpData.model_name,httpData.model_name);

    }
    #endregion
}
