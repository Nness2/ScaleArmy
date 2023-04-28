using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    public GameObject ZonesBackMiddle;
    public GameObject ZonesLeft;
    public GameObject ZonesRight;


    public Material GreenZone;
    public Material WhiteZone;
    //public GameObject ZonesFrontLeft;
    //public GameObject ZonesFrontRight;

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
            ZonesLeft.GetComponent<MeshRenderer>().enabled = true;
            ZonesRight.GetComponent<MeshRenderer>().enabled = true;
            //ZonesLeft.GetComponent<MeshRenderer>().enabled = true;
            //ZonesRight.GetComponent<MeshRenderer>().enabled = true;
        }

        else
        {
            ZonesBackMiddle.GetComponent<MeshRenderer>().enabled = false;
            ZonesLeft.GetComponent<MeshRenderer>().enabled = false;
            ZonesRight.GetComponent<MeshRenderer>().enabled = false;
            //ZonesLeft.GetComponent<MeshRenderer>().enabled = false;
            //ZonesRight.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    //Make AllAZone white to prepare the next green one
    public void ResetAllZone()
    {
        ZonesBackMiddle.GetComponent<MeshRenderer>().material = WhiteZone;
        ZonesLeft.GetComponent<MeshRenderer>().material = WhiteZone;
        ZonesRight.GetComponent<MeshRenderer>().material = WhiteZone;
    }
}
