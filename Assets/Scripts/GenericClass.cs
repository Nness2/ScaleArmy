using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericClass : MonoBehaviour
{

    [System.Serializable]
    public enum E_Action
    {
        Attack,
        Follow,
        Wait
    }

    [System.Serializable]
    public enum E_Zone
    {
        BackMiddle,
        BackLeft,
        BackRight,
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
