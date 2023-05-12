using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOpen : MonoBehaviour
{

    private TaskManager TskMng_Script;
    void Start()
    {
        TskMng_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TaskManager>();
    }

    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        TskMng_Script.OpenActifTaskCanvas();
    }
}
