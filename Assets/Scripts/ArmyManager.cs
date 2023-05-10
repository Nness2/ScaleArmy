using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyManager : MonoBehaviour
{
    //référence aux différentes zones attaché au personnage
    public GameObject ZonesBackMiddle;
    public GameObject ZonesLeft;
    public GameObject ZonesRight;

    //Material permettant de changer le mat des zones quand on les overlays avec un montre dans la main
    public Material GreenZone;
    public Material WhiteZone;

    public int nbrMonsterStart = 3;
    public int maxArmyUnit = 5; //Nombre de monstre maximum dans l'armé
    public List<GameObject> Army; //La liste des unités dans l'armé

    public GameObject MonsterPrefab;

    //Permet la gestion du nombre d'unité
    public bool OverPopulate;
    public bool WaitOverPopulate;
    public float regulationDelay = 5;

    //MonsterColor
    public Material BlueMat;
    public Material YellowMat;
    public Material RedMat;
    public Material GreenMat;
    public Material PurpleMat;
    public Material BlackMat;


    void Start()
    {
        LoadStartMapData();

        #region Vérifie si une session est toujour en court, attribue les hp en consequence
        if (PlayerPrefs.GetFloat("Teddy") <= 0) // Si le joueur a 0 hp, c'est que la session est fini ou n'a pas commencé
        {
            GetComponent<Statistique>().ChangeHealthValue((int)GetComponent<Statistique>()._startHealth);
        }
        else
        {
            GetComponent<Statistique>().ChangeHealthValue((int)PlayerPrefs.GetFloat("Teddy"));
        }
        #endregion

        #region Compte le nombre d'unité restant dans l'armé au démarage de niveau
        int ArmyUnits = 0;
        for (int i = 0; i < PlayerPrefs.GetInt("nbr") + 1; i++)
        {
            if(PlayerPrefs.GetFloat("Monster" + i) > 0)
            {
                ArmyUnits++;
            }
        }
        #endregion

        //S'il ne reste plus d'unité c'est que la run est terminé et on attribue "nbrMonsterStart" monstre pour un nouveau départ
        if (ArmyUnits == 0)
        {
            attributeBeginningMonster();
        }
        //S'il reste des unités on reconstitue l'armé du niveau précedant 
        else 
        {
            attributeSavedMonster();
        }

        // initialisation des variables
        OverPopulate = false;
        WaitOverPopulate = false;
    }

    void Update()
    {
        #region Comptage du nombre d'unité dans l'armé, si trop élevé on lance les script permettant la régulation
        if (Army.Count > maxArmyUnit)
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
        #endregion
    }

    IEnumerator OverPopulateAutoRegulation(GameObject monster)
    {
        #region Régulation du nombre d'unity, toute les regulationDelay on régule 
        yield return new WaitForSeconds(regulationDelay); // wait for 1 second
        float minDistance = Mathf.Infinity;
        GameObject target = null;
        foreach (GameObject obj in Army)
        {
            if(monster == null)
            {
                break;
            }
            if(obj != null)
            {
                if (obj != monster.transform.gameObject)
                {
                    float distance = Vector3.Distance(monster.transform.position, obj.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = obj;
                    }
                }
            }
        }
        if (monster != null && target != null)
        {
            Vector3 direction = target.transform.position - monster.transform.position;
            monster.transform.rotation = Quaternion.LookRotation(direction);
            target.GetComponent<Statistique>().TakeDamage(monster.GetComponent<Statistique>().damage, GenericClass.E_Action.Wait);
            monster.GetComponent<AnimationManager>()._animator.SetBool("Attack", true);
            yield return new WaitForSeconds(1); // wait for 1 second
            monster.GetComponent<AnimationManager>()._animator.SetBool("Attack", false);
            WaitOverPopulate = false;
        }
        #endregion
    }


    public void ShowZones(bool show)
    {
        if (show)
        {
            ZonesBackMiddle.GetComponent<MeshRenderer>().enabled = true;
            ZonesLeft.GetComponent<MeshRenderer>().enabled = true;
            ZonesRight.GetComponent<MeshRenderer>().enabled = true;
        }

        else
        {
            ZonesBackMiddle.GetComponent<MeshRenderer>().enabled = false;
            ZonesLeft.GetComponent<MeshRenderer>().enabled = false;
            ZonesRight.GetComponent<MeshRenderer>().enabled = false;
        }
    }



    //Make AllAZone white to prepare the next green one
    public void ResetAllZone()
    {
        ZonesBackMiddle.GetComponent<MeshRenderer>().material = WhiteZone;
        ZonesLeft.GetComponent<MeshRenderer>().material = WhiteZone;
        ZonesRight.GetComponent<MeshRenderer>().material = WhiteZone;
    }

    //Trouve la postion sur de la zone correspondente pour faire spawn le montre au bonne endroit
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



    public void updateMonsterColor(GameObject mtr,int level)
    {
        switch (level)
        {
            case 1:
                ChangeMonsterMat(mtr, YellowMat);
                break;
            case 2:
                ChangeMonsterMat(mtr, GreenMat);
                break;
            case 3:
                ChangeMonsterMat(mtr, BlueMat);
                break;
            case 4:
                ChangeMonsterMat(mtr, RedMat);
                break;
            case 5:
                ChangeMonsterMat(mtr, BlackMat);
                break;
            case 6:
                ChangeMonsterMat(mtr, PurpleMat);
                break;
        }
    }

    public void ChangeMonsterMat(GameObject mtr, Material mat)
    {
        GameObject MatObject = mtr.transform.GetChild(0).gameObject;
        Material[] materials = MatObject.GetComponent<SkinnedMeshRenderer>().materials;

        materials[0] = mat;

        MatObject.GetComponent<SkinnedMeshRenderer>().materials = materials;
    }



    //give 3 basic monster, it's the case for the first level map
    private void attributeBeginningMonster()
    {
        PlayerPrefs.DeleteAll();
        for (int i = 1; i <= nbrMonsterStart; i++)
        {
            PlayerPrefs.SetInt("nbr", PlayerPrefs.GetInt("nbr") + 1); // Incremente le nbr de monster
            string monsterName = "Monster" + i;


            GameObject newMonster = GameObject.Instantiate(MonsterPrefab, GetPositionOfZone((GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone")), Quaternion.identity);

            PlayerPrefs.SetFloat(monsterName, newMonster.GetComponent<Statistique>()._startHealth);
            ChangeMonsterMat(newMonster, YellowMat); // On attribue la couleur associé au level
            newMonster.name = monsterName; // On attribue le nom
            newMonster.tag = "MyMonster"; // On attribue le tag correspondant au soldat

            //On sauvegarde dans le fichier les informations utile pour le prochain niveau
            PlayerPrefs.SetInt(monsterName + "level", newMonster.GetComponent<Statistique>()._level);
            PlayerPrefs.SetInt(monsterName + "damage", newMonster.GetComponent<Statistique>().damage);
            PlayerPrefs.SetFloat(monsterName + "AtkSpeed", newMonster.GetComponent<Statistique>().attackSpeed);

            newMonster.GetComponent<Statistique>().ChangeHealthValue((int)PlayerPrefs.GetFloat(monsterName)); // On update la barre de vie
            newMonster.GetComponent<Statistique>().setEnemyBarToGreen(); //On update la couleur de la barre de vie, vert = soldat

            //On active le script Soldier et désactive Enemy
            newMonster.GetComponent<SoldierBehavior>().enabled = true;
            newMonster.GetComponent<EnemyBehavior>().enabled = false;

            newMonster.transform.SetParent(GetComponent<PlayerController>().ArmyParent); // On ajoute le monstre dans le parent contenant tout les soldats
            Army.Add(newMonster); // On ajoute le monstre à notre liste Army contenant toute nos unités
            SaveStartMapData(); //On sauvegarde les datas de l'armé dans le fichier
        }
    }

    //on reconstitue l'armé du niveau précedant 
    private void attributeSavedMonster()
    {
        //On parcourt la liste des unités (il peut y avoir des unités morte dans la liste, on tri dans la condition suivante
        for (int i = 0; i < PlayerPrefs.GetInt("nbr") + 1; i++)
        {
            string monsterName = "Monster" + i;
            if (PlayerPrefs.GetFloat(monsterName) > 0) // Pour sélectionne les unités avec des hp 
            {
                GameObject newMonster = null;
                newMonster = GameObject.Instantiate(MonsterPrefab, (GetPositionOfZone((GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone")) + new Vector3(i / 10, 0, -i / 10)), Quaternion.identity);

                newMonster.GetComponent<Statistique>()._level = PlayerPrefs.GetInt(monsterName + "level"); // On réccupére son level
                updateMonsterColor(newMonster, newMonster.GetComponent<Statistique>()._level); // On update sa couleur par-rapport au level

                //Update monster statistique with data saved
                newMonster.name = monsterName;
                newMonster.tag = "MyMonster";
                newMonster.GetComponent<Statistique>()._startHealth = 100 + (50 * (newMonster.GetComponent<Statistique>()._level - 1)); // -1 psq level 1 déjà 100hp
                newMonster.GetComponent<Statistique>().ChangeHealthValue((int)PlayerPrefs.GetFloat(monsterName));
                newMonster.GetComponent<Statistique>().damage = PlayerPrefs.GetInt(monsterName + "damage");
                newMonster.GetComponent<Statistique>().attackSpeed = PlayerPrefs.GetFloat(monsterName + "AtkSpeed");
                newMonster.GetComponent<Statistique>().setEnemyBarToGreen(); // On update la couleur de la barre de vie correspondant à Soldier
                newMonster.transform.SetParent(GetComponent<PlayerController>().ArmyParent); // On ajoute le monstre dans le parent contenant tout les soldats

                //On active le script Soldier et désactive Enemy
                newMonster.GetComponent<SoldierBehavior>().enabled = true;
                newMonster.GetComponent<EnemyBehavior>().enabled = false;

                newMonster.GetComponent<SoldierBehavior>()._zoneAttribute = (GenericClass.E_Zone)PlayerPrefs.GetInt(monsterName + "zone"); // On réccupére la zone correspondante
                Army.Add(newMonster); // On ajoute le monstre à notre liste Army contenant toute nos unités
            }
        }
    }



    //Permet de sauvegarder les datas du jeu à la fin du niveau
    public void SaveStartMapData()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("Mapnbr") + 1; i++)
        {
            PlayerPrefs.DeleteKey("MapMonster");
            PlayerPrefs.DeleteKey("MapMonster" + i + "level");
            PlayerPrefs.DeleteKey("MapMonster" + i + "damage");
            PlayerPrefs.DeleteKey("MapMonster" + i + "AtkSpeed");
            PlayerPrefs.DeleteKey("MapMonster" + i + "State");
            PlayerPrefs.DeleteKey("MapMonster" + i + "Zone");
        }

        PlayerPrefs.SetFloat("MapTeddy", PlayerPrefs.GetFloat("Teddy"));
        PlayerPrefs.SetInt("Mapnbr", PlayerPrefs.GetInt("nbr"));

        for (int i = 0; i < PlayerPrefs.GetInt("nbr") + 1; i++)
        {
            PlayerPrefs.SetFloat("MapMonster" + i, PlayerPrefs.GetFloat("Monster" + i));
            PlayerPrefs.SetInt("MapMonster" + i + "level", PlayerPrefs.GetInt("Monster" + i + "level"));
            PlayerPrefs.SetInt("MapMonster" + i + "damage", PlayerPrefs.GetInt("Monster" + i + "damage"));
            PlayerPrefs.SetFloat("MapMonster" + i + "AtkSpeed", PlayerPrefs.GetFloat("Monster" + i + "AtkSpeed"));
            PlayerPrefs.SetInt("MapMonster" + i + "State", PlayerPrefs.GetInt("Monster" + i + "State"));
            PlayerPrefs.SetInt("MapMonster" + i + "Zone", PlayerPrefs.GetInt("Monster" + i + "Zone"));
        }
    }

    //Permet de load les datas au début d'un nouveau niveau, Permet d'éviter que le joueur accumule des monstres en relançant le niveau en boucle
    public void LoadStartMapData()
    {

        PlayerPrefs.SetFloat("Teddy", PlayerPrefs.GetFloat("MapTeddy"));
        PlayerPrefs.SetInt("nbr", PlayerPrefs.GetInt("Mapnbr"));

        for (int i = 0; i < PlayerPrefs.GetInt("nbr") + 1; i++)
        {
            PlayerPrefs.SetFloat("Monster" + i, PlayerPrefs.GetFloat("MapMonster" + i));
            PlayerPrefs.SetInt("Monster" + i + "level", PlayerPrefs.GetInt("MapMonster" + i + "level"));
            PlayerPrefs.SetInt("Monster" + i + "damage", PlayerPrefs.GetInt("MapMonster" + i + "damage"));
            PlayerPrefs.SetFloat("Monster" + i + "AtkSpeed", PlayerPrefs.GetFloat("MapMonster" + i + "AtkSpeed"));
            PlayerPrefs.SetInt("Monster" + i + "State", PlayerPrefs.GetInt("MapMonster" + i + "State"));
            PlayerPrefs.SetInt("Monster" + i + "Zone", PlayerPrefs.GetInt("MapMonster" + i + "Zone"));
        }
    }
}

