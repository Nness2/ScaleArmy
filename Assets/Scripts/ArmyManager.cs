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

    public List<GameObject> Army;

    // Monsters prefab
    //public GameObject waterMonsterPrefab;
    public GameObject MonsterPrefab;
    //public GameObject ZonesFrontLeft;
    //public GameObject ZonesFrontRight;

    public bool OverPopulate;
    public bool WaitOverPopulate;

    //MonsterColor
    public Material BlueMat;
    public Material YellowMat;
    public Material RedMat;
    public Material GreenMat;
    public Material PurpleMat;


    void Start()
    {
        OverPopulate = false;
        WaitOverPopulate = false;
        Army = new List<GameObject>();
        
        if (Army.Count == 0)//If No monster in army give 3 basic monster, it's the case for the first level map
        {
            PlayerPrefs.DeleteAll();
            for (int i = 1; i <= 3; i++)
            {
                PlayerPrefs.SetInt("nbr", PlayerPrefs.GetInt("nbr") + 1); // Incremente le nbr de monster
                string monsterName = "Monster" + PlayerPrefs.GetInt("nbr");

                PlayerPrefs.SetFloat(monsterName, 100);

                GameObject newMonster = GameObject.Instantiate(MonsterPrefab, GetPositionOfZone((GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone")), Quaternion.identity);
                ChangeMonsterMat(newMonster, GreenMat);
                newMonster.name = monsterName;
                newMonster.tag = "MyMonster";
                PlayerPrefs.SetInt(monsterName + "level", newMonster.GetComponent<Statistique>()._level);
                newMonster.GetComponent<Statistique>()._health = PlayerPrefs.GetFloat(monsterName);
                newMonster.GetComponent<Statistique>().setEnemyBarToGreen();
                newMonster.GetComponent<SoldierBehavior>().enabled = true;
                newMonster.GetComponent<EnemyBehavior>().enabled = false;
                newMonster.transform.SetParent(GetComponent<PlayerController>().ArmyParent);
                Army.Add(newMonster);
            }
        }

        else //if some monster is in the army, instantiate them with data saved
        {
            for (int i = 0; i < PlayerPrefs.GetInt("nbr") + 1; i++)
            {
                string monsterName = "Monster" + i;
                if (PlayerPrefs.GetFloat(monsterName) > 0)
                {
                    GameObject newMonster = null;
                    newMonster = GameObject.Instantiate(MonsterPrefab, (GetPositionOfZone((GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone")) + new Vector3(i / 10, 0, -i / 10)), Quaternion.identity);
                    newMonster.GetComponent<Statistique>()._level = PlayerPrefs.GetInt(monsterName + "level");
                    updateMonsterColor(newMonster, newMonster.GetComponent<Statistique>()._level);

                    //Update monster statistique with data saved
                    newMonster.name = monsterName;
                    newMonster.tag = "MyMonster";
                    newMonster.GetComponent<Statistique>()._health = PlayerPrefs.GetFloat(monsterName);
                    newMonster.GetComponent<Statistique>().setEnemyBarToGreen();
                    newMonster.transform.SetParent(GetComponent<PlayerController>().ArmyParent);
                    newMonster.GetComponent<SoldierBehavior>().enabled = true;
                    newMonster.GetComponent<EnemyBehavior>().enabled = false;
                    newMonster.GetComponent<SoldierBehavior>()._zoneAttribute = (GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName+"zone");
                    Army.Add(newMonster);
                }
            }
        }
    }

    void Update()
    {
        if(Army.Count > 10)
        {
            OverPopulate = true;
            if (!WaitOverPopulate)
            {
                int randomMonster = Random.Range(0, Army.Count);
                StartCoroutine(OverPopulateAutoRegulation(Army[randomMonster]));
                WaitOverPopulate = true;
            }
        }
        else
        {
            OverPopulate = false;
        }
    }

    IEnumerator OverPopulateAutoRegulation(GameObject monster)
    {
        yield return new WaitForSeconds(5); // wait for 1 second
        float minDistance = Mathf.Infinity;
        GameObject target = null;
        foreach (GameObject obj in Army)
        {
            if(obj != monster.transform.gameObject)
            {
                float distance = Vector3.Distance(monster.transform.position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    target = obj;
                }
            }
        }
        Vector3 direction = target.transform.position - monster.transform.position;
        monster.transform.rotation = Quaternion.LookRotation(direction);
        target.GetComponent<Statistique>().TakeDamage(33, GenericClass.E_Action.Wait);
        monster.GetComponent<AnimationManager>()._animator.SetBool("Attack", true);
        yield return new WaitForSeconds(1); // wait for 1 second
        monster.GetComponent<AnimationManager>()._animator.SetBool("Attack", false);
        WaitOverPopulate = false;
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

    public Vector3 GetPositionOfZone(GenericClass.E_Zone zone)
    {
        Vector3 zonePose = Vector3.zero;

        switch (zone)
        {
            case GenericClass.E_Zone.Back:
                zonePose = ZonesBackMiddle.transform.position;
                break;

            case GenericClass.E_Zone.Left:
                zonePose = ZonesLeft.transform.position;
                break;

            case GenericClass.E_Zone.Right:
                zonePose = ZonesRight.transform.position;
                break;
        }

        return zonePose; 
    }

    public void ChangeMonsterMat(GameObject mtr, Material mat)
    {
        GameObject MatObject = mtr.transform.GetChild(0).gameObject;
        Material[] materials = MatObject.GetComponent<SkinnedMeshRenderer>().materials;

        materials[0] = mat;


        MatObject.GetComponent<SkinnedMeshRenderer>().materials = materials;
    }

    public void updateMonsterColor(GameObject mtr,int level)
    {
        switch (level)
        {
            case 1:
                ChangeMonsterMat(mtr, GreenMat);
                break;
            case 2:
                ChangeMonsterMat(mtr, BlueMat);
                break;
            case 3:
                ChangeMonsterMat(mtr, YellowMat);
                break;
            case 4:
                ChangeMonsterMat(mtr, RedMat);
                break;
            case 5:
                ChangeMonsterMat(mtr, RedMat);
                break;
        }
    }
}
