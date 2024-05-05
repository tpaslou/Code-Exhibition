using UnityEngine;
/// <summary>
/// The Gameobject always looks at the main camera transform
/// </summary>
public class LookAt : MonoBehaviour
{
    Transform target;


    private void Start()
    {
        target = Camera.main.transform;
    }

    void Update()
    {
        Vector3 directionToTarget = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget);
    }
}
