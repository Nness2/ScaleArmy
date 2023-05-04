using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLook : MonoBehaviour
{
    //private Camera _camera;

    private void Start()
    {
        //_camera = Camera.main;
    }
    private void LateUpdate()
    {
        //transform.LookAt(_camera.transform);
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

    }
}
