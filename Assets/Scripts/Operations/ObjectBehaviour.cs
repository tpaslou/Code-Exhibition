using System.Collections.Generic;
using Data;
using UnityEngine;

/// <summary>
/// Monobehaviour responsible for the Task 1 objects functionality
/// </summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(Material))]
public class ObjectBehaviour : MonoBehaviour
{
    #region variables
    [SerializeField]
    List<Material> meshMaterials;

    bool isMoving = false;
    float velocity;
    int objectID;
    #endregion

    #region methods
    private void FixedUpdate()
    {
        if (isMoving)
        {
            Movement();
        }
    }

    public void GenerateMesh(int objectID, List<Point> contourPoints, Vector3 centerPosition)
    { 
       
        Debug.Log("Generating Mesh...");

        this.objectID = objectID;
        Mesh mesh = new Mesh()
        {
            name = "Mesh"+ objectID.ToString()
        };

        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[contourPoints.Count+1];
        vertices[0] = Vector3.zero /*centerPosition, 1st item on the idices array*/;

        //creating vertices from the provided contour points & starting point of the mesh
        for (int i = 0; i < contourPoints.Count; i++)
        {
            vertices[i+1] = new Vector3(contourPoints[i].x, 0 , contourPoints[i].y);
        }      

        //Creating triangles from the provided vertices, assuming the contour points form a closed loop
        int[] triangles = new int[(contourPoints.Count - 1) * 3];
        for (int i = 0; i < contourPoints.Count - 1; i++)
        {
            
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        //Creating the UVs for texture mapping
        Vector2[] uvs = new Vector2[contourPoints.Count+1];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.bounds = new Bounds(centerPosition, mesh.bounds.size);
       

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        switch(objectID)
        {
            case 1:
                meshRenderer.material = meshMaterials[0];
                break;
            case 3:
                meshRenderer.material = meshMaterials[1];
                break;
            case 6:
                meshRenderer.material = meshMaterials[2];
                break;
            default:
                Debug.LogWarning("Couldn't find material for Oject ID : " +objectID);
                break;
        }
    }
    
    public void StartMovement(float velocity)
    {
        isMoving = true;
        this.velocity = velocity;
    }

    void Movement() {

        float distanceToMove = velocity * Time.fixedDeltaTime;

        //Forward is Z axis in Unity
        transform.Translate(Vector3.forward * distanceToMove);

        if (transform.position.z >= 2f)
        {
            Debug.Log("Object " +objectID + " reached 2m in Unity, time to die :(");
            isMoving = false;
            EventBroker.CallObjectDestroyed(objectID);
            Destroy(gameObject);
        }
    }

    #endregion
}
