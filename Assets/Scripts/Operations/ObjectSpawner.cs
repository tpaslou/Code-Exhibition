using Data;
using UnityEngine;


/// <summary>
/// Class responsible for spawning the 3D objects based on the ID received
/// </summary>
public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject MeshObject;

    private void OnEnable()
    {
        EventBroker.DataPropagation.AddListener(OnDataRecieved);
    }

    private void OnDataRecieved(ObjectData data)
    {

        Debug.Log("Received data");
        GameObject gameObject = Instantiate(MeshObject, new Vector3(data.x,data.z,data.y),
            Quaternion.identity); //Spawning in the designated position provided in json 
        gameObject.name = data.object_id.ToString();
        float scale = Mathf.Sqrt(data.object_area); // Assuming uniform scaling
        gameObject.transform.localScale = new Vector3(scale, scale, scale);
        gameObject.transform.position += new Vector3(0, 1, 0);// 1 meter over the floor movement
        ObjectBehaviour objectBehaviour = gameObject.GetComponent<ObjectBehaviour>();
        Vector3 centerPosition = new Vector3(data.x, data.z, data.y);
        objectBehaviour.GenerateMesh(data.object_id,data.contour_points,centerPosition);
        objectBehaviour.StartMovement(data.velocity);
    }

    

    private void OnDisable()
    {
        EventBroker.DataPropagation.RemoveListener(OnDataRecieved);

    }
}
