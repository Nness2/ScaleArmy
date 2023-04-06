using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public GameObject ZonesBackMiddle;
    public GameObject ZonesBackLeft;
    public GameObject ZonesBackRight;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowZones(bool show)
    {
        if (show)
        {
            ZonesBackMiddle.GetComponent<MeshRenderer>().enabled = true;
            ZonesBackLeft.GetComponent<MeshRenderer>().enabled = true;
            ZonesBackRight.GetComponent<MeshRenderer>().enabled = true;
        }

        else
        {
            ZonesBackMiddle.GetComponent<MeshRenderer>().enabled = false;
            ZonesBackLeft.GetComponent<MeshRenderer>().enabled = false;
            ZonesBackRight.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
