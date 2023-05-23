using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBase : MonoBehaviour
{
    public GameObject TaskCanvas;
    public bool _done;
    public bool isNear;

    public GameObject ValidProof;
    public GameObject MonsterPrefab;
    private ArmyManager ArmyMng_Script;
    private TaskManager TaskMng_Script;
    public GameObject ModelToOutline;

    public GameObject ParentLocation;
    public List<GameObject> LocationsList;

    public List<GameObject> MonsterList;

    void Start()
    {
        MonsterList = new List<GameObject>();
        LocationsList = new List<GameObject>();
        Transform Locations = ParentLocation.GetComponentInChildren<Transform>();
        foreach(Transform loc in Locations)
        {
            LocationsList.Add(loc.gameObject);
        }
        isNear = false;
        _done = false;
        ArmyMng_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<ArmyManager>();
        TaskMng_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<TaskManager>();

        //GenerateDefence
        int mapLevel = TaskMng_Script.MapLevel;

        int nbrMonster = Random.Range(2, mapLevel+4);

        for (int i = 0; i < nbrMonster; i++)
        {
            int LevelMonster = Random.Range(1, mapLevel+1);

            string monsterName = "Enemy" + i;
            GameObject newMonster = null;

            newMonster = GameObject.Instantiate(MonsterPrefab, LocationsList[i].transform.position, Quaternion.identity);
            MonsterList.Add(newMonster);
            newMonster.GetComponent<EnemyBehavior>().ObjectifDefended = transform.gameObject; // Dis au monster qu'elle objectif il defent, permet de trouver la liste des montres du groupe dans le gameobject

            newMonster.GetComponent<Statistique>()._level = LevelMonster;
            ArmyMng_Script.updateMonsterColor(newMonster, newMonster.GetComponent<Statistique>()._level);

            //Update monster statistique with data saved
            newMonster.name = monsterName;
            //newMonster.GetComponent<Statistique>()._health = PlayerPrefs.GetFloat(monsterName);
            //newMonster.GetComponent<Statistique>().ChangeHealthValue((int)PlayerPrefs.GetFloat(monsterName));

            newMonster.GetComponent<Statistique>().damage += (LevelMonster-1)*10;
            newMonster.GetComponent<Statistique>()._health += (LevelMonster-1)*50;
            newMonster.GetComponent<Statistique>()._startHealth += (LevelMonster-1)*50;
            newMonster.GetComponent<Statistique>().attackSpeed += (LevelMonster-1)*0.1f;

            newMonster.transform.SetParent(transform);
            //newMonster.GetComponent<SoldierBehavior>()._zoneAttribute = (GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone");
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, ArmyMng_Script.transform.position);
        if (distance < 3)
        {
            if (!isNear)
            {
                ModelToOutline.GetComponent<Outline>().enabled = true;
                isNear = true;
            }
        }
        else
        {
            if (isNear)
            {
                ModelToOutline.GetComponent<Outline>().enabled = false;
                isNear = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (isNear)
        {
            TaskMng_Script.OpenActifTaskCanvas(gameObject);
        }
    }
}
