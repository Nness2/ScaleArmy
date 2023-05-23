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
        Wait,
    }

    [System.Serializable]
    public enum E_Zone
    {
        Back,
        Left,
        Right,
        Line,
        Totem,
        FrontLeft,
        FrontRight,
    }

    [System.Serializable]
    public enum E_MonsterAnimState
    {
        Idle,
        Run,
        Floating,
        Eat,
    }

    [System.Serializable]
    public enum E_Loot
    {
        None,
        Flask,
        Meat,
        Bandage,
    }


    public 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
}
