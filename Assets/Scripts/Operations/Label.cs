using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// This class is responsible for labels functionality.
/// It subscribes and invokes the event of destroying
/// the spawned 3D objects.
/// </summary>
public class Label : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshPro;

    string parentObjectName;
    GraphicRaycaster raycaster;

    private void OnEnable()
    {
        EventBroker.ObjectClick += OnObjectClick;
    }

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
        if (!raycaster)
            Debug.LogError("Error finding rayctaster on the Label object");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            foreach (RaycastResult result in results)
            {
                EventBroker.CallObjectClick(parentObjectName);
            }
        }
    }

    private void OnObjectClick(string obj)
    {
        if(obj.Equals(parentObjectName))
            Destroy(gameObject);
    }

    public void SetLabelData(string text,string data)
    {
        textMeshPro.text = text;
        parentObjectName = data;
    }

    private void OnDisable()
    {
        EventBroker.ObjectClick -= OnObjectClick;
    }
}
