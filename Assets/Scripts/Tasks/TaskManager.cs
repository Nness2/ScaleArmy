using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class TaskManager : MonoBehaviour
{
    public PlayableDirector doorAnim;
    public GameObject ExitDoor;
    private GameObject Character;

    //TaskTotem
    public GameObject[] AllTaskTotem;
    public GameObject ActifTaskTotem;

    public int NbrTaskEnd;
    public int MapLevel;
    public Text RoomLevelText;

    void Start()
    {
        MapLevel = PlayerPrefs.GetInt("MapLevel")+1;
        RoomLevelText.text = "Room " + MapLevel;
        NbrTaskEnd = 0;
        Character = GameObject.FindGameObjectWithTag("LocalPlayer");
        AllTaskTotem = GameObject.FindGameObjectsWithTag("TaskTotem");
    }

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
            Debug.Log("test");
        }
        #endregion

        #region DetectProximityTask
        //Si Le button de tache n'est pas activé
        if (ActifTaskTotem == null)
        {
            //Vérifie si une des taches est à proximité
            foreach (GameObject obj in AllTaskTotem)
            {
                distance = Vector3.Distance(Character.transform.position, obj.transform.position);
                if (distance < 2f)
                {
                    //Si on n'a pas encore fait la tache, le bouton s'affiche
                    if (obj.GetComponent<TaskBase>()._done != true)
                    {
                        ActifTaskTotem = obj;
                        ActifTaskTotem.GetComponent<TaskBase>().ModelToOutline.GetComponent<Outline>().enabled = true;
                        break;
                    }
                }
            }
        }

        else
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
                ActifTaskTotem.GetComponent<TaskBase>().ModelToOutline.GetComponent<Outline>().enabled = false;
                ActifTaskTotem = null;

            }
        }

        #endregion
    }


    public void OpenActifTaskCanvas()
    {
        if (ActifTaskTotem != null)
            ActifTaskTotem.GetComponent<TaskBase>().TaskCanvas.SetActive(true);
    }

    public void CloseActifTaskCanvas()
    {
        if(ActifTaskTotem != null)
            ActifTaskTotem.GetComponent<TaskBase>().TaskCanvas.SetActive(false);
    }

}
