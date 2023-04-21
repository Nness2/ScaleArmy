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
}
