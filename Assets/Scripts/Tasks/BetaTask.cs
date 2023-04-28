using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetaTask : MonoBehaviour
{
    private int ValideNmb;
    private TaskManager TM_Script;
    void Start()
    {
        ValideNmb = 0;
        TM_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TaskManager>();
    }

    void Update()
    {
        if (ValideNmb >= 9)
        {
            GetComponent<TaskBase>()._done = true;
            GetComponent<TaskBase>().TaskCanvas.SetActive(false);
            GetComponent<TaskBase>().ValidProof.SetActive(true);
            TM_Script.NbrTaskEnd++;
            this.enabled = false;
        }
    }

    public void ValidateButton()
    {
        ValideNmb++;
    }
}
