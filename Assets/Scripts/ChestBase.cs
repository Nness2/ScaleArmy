using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChestBase : MonoBehaviour
{
    public GameObject MonsterPrefab;
    private ArmyManager ArmyMng_Script;
    private TaskManager TaskMng_Script;

    public GameObject ParentLocation;
    public List<GameObject> LocationsList;

    public GameObject ModelToOutline;
    public bool isNear;
    public bool LootIsOut;

    public PlayableDirector lootAnim;

    //lootPrefab 
    public GameObject FiolePrefab;
    public GameObject ViandePrefab;
    public InventoryManager InvMng_Script;


    void Start()
    {
        InvMng_Script = GameObject.FindGameObjectWithTag("GameManager").GetComponent<InventoryManager>();
        isNear = false;
        LootIsOut = false;
        LocationsList = new List<GameObject>();
        Transform Locations = ParentLocation.GetComponentInChildren<Transform>();
        foreach(Transform loc in Locations)
        {
            LocationsList.Add(loc.gameObject);
        }


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
        if(distance < 3)
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
        if (isNear && !LootIsOut)
        {
            GameObject choosedLoot = RandomLoot();
            choosedLoot = Instantiate(choosedLoot, transform.position, transform.rotation);
            choosedLoot.transform.SetParent(lootAnim.transform);
            StartCoroutine(EndAnimation(choosedLoot));
            lootAnim.Play();
            LootIsOut = true;
        }
    }

    IEnumerator EndAnimation(GameObject choosedLoot)
    {
        yield return new WaitForSeconds(1);
        InvMng_Script.AddLootToInventory(choosedLoot.GetComponent<LootClass>()._image, choosedLoot.GetComponent<LootClass>()._lootType);

        Destroy(choosedLoot);
    }


    private GameObject RandomLoot()
    {
        int randLoot = Random.Range(0, 2);
        GameObject choosedLoot = null;
        switch (randLoot)
        {
            case 0:
                choosedLoot = FiolePrefab;
                break;
            case 1:
                choosedLoot = ViandePrefab;
                break;
        }
        return choosedLoot;
    }
}
