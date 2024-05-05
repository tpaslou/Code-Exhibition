using UnityEngine;
/// <summary>
/// This class is attached to the 3D spawned prefab for click behaviour
/// </summary>
public class ClickBehaviour : MonoBehaviour
{
    string objectName;
    BoxCollider collider;

    private void OnEnable()
    {
        EventBroker.ObjectClick += OnClick;
        objectName = gameObject.name.Replace("(Clone)", "");
        collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;

    }

    private void OnClick(string obj)
    {
        if (objectName.Equals(obj))
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EventBroker.ObjectClick -= OnClick;
    }

    private void OnMouseDown()
    {
        EventBroker.CallObjectClick(objectName);
    }
}
