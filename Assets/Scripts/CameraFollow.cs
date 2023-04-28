using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Référence au transform du personnage
    public Vector3 offset;     // Offset de position de la caméra par rapport au personnage
    public bool CamMovable;
    public GameObject maskCanvas;

    //Camera slide
    public float speed = 1.0f;
    private Vector3 lastMousePosition;

    //UI element qui passe du mask à l'ui joueur en caméra mode
    public GameObject[] ListElementOnMask;
    public GameObject CanvasJoueur;
    public GameObject CanvasMask;
    public PlayerController plyrControl_Script;

    //Camera Zoom
    //public float maxZoom = 5f;
    //public float minZoom = 1f;
    public float zoomSpeed = 10f;

    //Camera Zoom
    public float maxZoomFOW = 60;
    public float minZoomFOW = 40;

    private void Start()
    {
        CamMovable = false;
        plyrControl_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        if (CamMovable)
        {
            CameraSlide();
        }
        else
        {
            transform.position = target.position + offset;
        }
    }

    private void Update()
    {
        /*
        #region ZoomTouchScreen
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            transform.GetComponent<Camera>().orthographicSize += deltaMagnitudeDiff * zoomSpeed;

            //transform.GetComponent<Camera>().orthographicSize = Mathf.Clamp(transform.GetComponent<Camera>().orthographicSize, minZoom, maxZoom);
        }

        #endregion
        */
    }


    public void SwitchCameraMode()
    {
        CamMovable = !CamMovable;
        maskCanvas.SetActive(!maskCanvas.activeSelf);

        if (CamMovable)
        {
            foreach(GameObject obj in ListElementOnMask)
            {
                obj.transform.SetParent(CanvasMask.transform);
            }
        }
        else
        {
            foreach (GameObject obj in ListElementOnMask)
            {
                obj.transform.SetParent(CanvasJoueur.transform);
                plyrControl_Script.FeedActivate = false;
                plyrControl_Script.ManagerActivate = false;
            }
        }
    }


    public void CameraSlide()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            float deltaZ = Input.mousePosition.y - lastMousePosition.y;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - deltaZ * speed * Time.deltaTime);
            lastMousePosition = Input.mousePosition;
        }
    }

    public void Zoom(bool zoomIn)
    {
        StartCoroutine(ZoomCoroutine(zoomIn));
    }


    private IEnumerator ZoomCoroutine(bool zoomIn)
    {
        float targetZoom = zoomIn ? minZoomFOW : maxZoomFOW;
        float startZoom = Camera.main.fieldOfView;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * zoomSpeed;
            Camera.main.fieldOfView = Mathf.Lerp(startZoom, targetZoom, t);
            yield return null;
        }
    }

}
