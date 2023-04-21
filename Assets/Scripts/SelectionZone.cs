using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionZone : MonoBehaviour
{

    private Vector3 startSelectionPos;
    private Vector3 endSelectionPos;
    public List<GameObject> selectedObjects = new List<GameObject>();
    private Rect selectionBox;

    void Update()
    {

    }

    void SelectObjectsInZone(Vector3 start, Vector3 end)
    {
        selectedObjects.Clear();

        Collider[] colliders = Physics.OverlapBox(end, start);

        foreach (Collider collider in colliders)
        {
            selectedObjects.Add(collider.gameObject);
        }
    }

    Bounds GetSelectionBounds()
    {
        Vector3 selectionStart = Camera.main.ScreenToWorldPoint(startSelectionPos);
        Vector3 selectionEnd = Camera.main.ScreenToWorldPoint(endSelectionPos);
        Bounds selectionBounds = new Bounds(Vector3.zero, Vector3.zero);
        selectionBounds.SetMinMax(Vector3.Min(selectionStart, selectionEnd), Vector3.Max(selectionStart, selectionEnd));
        return selectionBounds;
    }

    void OnGUI()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startSelectionPos = Input.mousePosition;
            selectionBox = new Rect(startSelectionPos.x, Screen.height - startSelectionPos.y, 0, 0);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            selectionBox.width = currentMousePos.x - selectionBox.x;
            selectionBox.height = currentMousePos.y - selectionBox.y;
            SelectObjectsInZone(new Vector3(selectionBox.x, selectionBox.y, 0), new Vector3(selectionBox.width, selectionBox.height, 0));
            GUI.Box(selectionBox, "");
        }
    }
}