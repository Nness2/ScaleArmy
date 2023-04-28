using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TaskManager : MonoBehaviour
{
    public PlayableDirector doorAnim;
    public GameObject ExitDoor;
    private GameObject Character;

    //TaskTotem
    public GameObject TaskButton;
    public GameObject[] AllTaskTotem;
    public GameObject ActifTaskTotem;
    public bool taskButtonFlag;

    public int NbrTaskEnd;


    // Start is called before the first frame update
    void Start()
    {
        NbrTaskEnd = 0;
        Character = GameObject.FindGameObjectWithTag("LocalPlayer");
        AllTaskTotem = GameObject.FindGameObjectsWithTag("TaskTotem");
        taskButtonFlag = !TaskButton.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        #region debug
        float distance = Vector3.Distance(Character.transform.position, ExitDoor.transform.position);
        if (distance < 6)
        {
            if (NbrTaskEnd >= 3)
            {
                doorAnim.Play();
            }
        }
        #endregion

        #region DetectProximityTask
        if (TaskButton.activeSelf == false)
        {
            if (TaskButton.activeSelf != taskButtonFlag)
            {
                foreach (GameObject obj in AllTaskTotem)
                {
                    distance = Vector3.Distance(Character.transform.position, obj.transform.position);
                    if (distance < 2f)
                    {
                        if(obj.GetComponent<TaskBase>()._done != true)
                        taskButtonFlag = TaskButton.activeSelf;
                        TaskButton.SetActive(true);
                        ActifTaskTotem = obj;
                        break;
                    }
                }
            }
        }

        else
        {
            if (TaskButton.activeSelf != taskButtonFlag)
            {
                bool isNear = false;
                foreach (GameObject obj in AllTaskTotem)
                {
                    distance = Vector3.Distance(Character.transform.position, obj.transform.position);
                    if (distance < 2f)
                    {
                        isNear = true;
                    }
                }
                if (!isNear)
                {
                    taskButtonFlag = TaskButton.activeSelf;
                    TaskButton.SetActive(false);
                }
            }
        }

        #endregion
    }


    public void OpenActifTaskCanvas()
    {
        Debug.Log(ActifTaskTotem.GetComponent<TaskBase>().TaskCanvas.gameObject.name);
        ActifTaskTotem.GetComponent<TaskBase>().TaskCanvas.SetActive(true);
    }

}
