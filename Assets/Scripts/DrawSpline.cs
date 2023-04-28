using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DrawSpline : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> points = new List<Vector3>();
    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };
    public bool readyToDraw;
    public GenericClass.E_Zone ZoneChoosed;
    public GameObject UiPannel;
    private PlayerController _player;
    public bool isMooving;

    private void Awake()
    {
        isMooving = false;
        _player = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
        lineRenderer = GetComponent<LineRenderer>();
        readyToDraw = false;
        //ZoneChoosed = GenericClass.E_Zone.Back;
    }

    private void Update()
    {
        if (readyToDraw && !_player.ManagerActivate)
        {

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                int layerMask = 1 << LayerMask.NameToLayer("Ground"); // ignore tous les layers sauf "Ground"

                if (Physics.Raycast(ray, out hitInfo, 100, layerMask))
                {
                    if (DistanceToLastPoint(hitInfo.point) >0.1f)
                    {
                        Vector3 pos = new Vector3(hitInfo.point.x, 0.5f, hitInfo.point.z);
                        points.Add(pos);

                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());


                    }
                }
            }
        }

        else if (Input.GetMouseButtonUp(0))
        {
            //OnNewPathCreated(points);

        }


    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
        {
            return Mathf.Infinity;
        }
        return Vector3.Distance(points.Last(), point);
    }


    public void CleanLineRenderer()
    {
        points.Clear();
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        UiPannel.SetActive(false);
    }
}