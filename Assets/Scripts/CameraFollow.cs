using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // Référence au transform du personnage
    public Vector3 offset;     // Offset de position de la caméra par rapport au personnage

    private void LateUpdate()
    {
        // Met à jour la position de la caméra
        transform.position = target.position + offset;

        // Garde l'orientation de la caméra
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
