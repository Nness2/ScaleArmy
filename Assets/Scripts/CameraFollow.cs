using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // R�f�rence au transform du personnage
    public Vector3 offset;     // Offset de position de la cam�ra par rapport au personnage

    private void LateUpdate()
    {
        // Met � jour la position de la cam�ra
        transform.position = target.position + offset;

        // Garde l'orientation de la cam�ra
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
