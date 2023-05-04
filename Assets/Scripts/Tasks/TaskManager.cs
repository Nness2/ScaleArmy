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
        if (Input.GetKey(KeyCode.O))
        {
            doorAnim.Play();
        }
        #endregion

        #region DetectProximityTask
        //Si Le button de tache n'est pas activé
        if (TaskButton.activeSelf == false)
        {
            //Si on a pas encore fait apparaitre le button
            if (TaskButton.activeSelf != taskButtonFlag)
            {
                //Vérifie si une des taches est à proximité
                foreach (GameObject obj in AllTaskTotem)
                {
                    distance = Vector3.Distance(Character.transform.position, obj.transform.position);
                    if (distance < 2f)
                    {
                        //Si on n'a pas encore fait la tache, le bouton s'affiche
                        if(obj.GetComponent<TaskBase>()._done != true)
                        {
                            taskButtonFlag = TaskButton.activeSelf;
                            TaskButton.SetActive(true);
                            ActifTaskTotem = obj;
                            break;
                        }
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
        ActifTaskTotem.GetComponent<TaskBase>().TaskCanvas.SetActive(true);
    }

}
