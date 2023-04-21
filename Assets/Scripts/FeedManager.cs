using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedManager : MonoBehaviour
{
    private bool MouseDown;
    public GenericClass.E_Zone newZone;
    private GameObject Tofeed;
    public ArmyManager armyManager_Script;
    public PlayerController plyrControl_Script;



    void Start()
    {
        MouseDown = false;
        newZone = GetComponent<SoldierBehavior>()._zoneAttribute;
        armyManager_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<ArmyManager>();
        plyrControl_Script = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<PlayerController>();
        Tofeed = null;
    }

    void Update()
    {
        if (plyrControl_Script.ManagerActivate || plyrControl_Script.FeedActivate && !plyrControl_Script.CamFollow_Script.CamMovable)
        {
            if (gameObject.transform.tag == "MyMonster")// && gameObject.GetComponent<SoldierBehavior>()._actionState == GenericClass.E_Action.Wait)
            {
                if (MouseDown)
                {
                    GetComponent<SoldierBehavior>().enabled = false;

                    //int groundLayer = LayerMask.NameToLayer("Default");
                    #region permet de surelever le monster quand on clique dessus
                    int layerMask = 1 << LayerMask.NameToLayer("CircleLimite"); // ignore tous les layers sauf "Ground"

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 0.01f);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 100, layerMask))
                    {
                        transform.position = new Vector3(hit.point.x, 3, hit.point.z);
                        GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Floating);
                        //Debug.Log(hit.transform.name);
                    }
                    #endregion

                    if (plyrControl_Script.ManagerActivate)
                    {

                        #region permet déplacer un monster dans un zone différente
                        int layerMask2 = 1 << LayerMask.NameToLayer("Zone");
                        Vector3 ray2 = transform.position;
                        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 0.01f);
                        RaycastHit hit2;
                        if (Physics.Raycast(ray2, -transform.up * 100, out hit2, 100, layerMask2))
                        {
                            newZone = hit2.collider.gameObject.GetComponent<ZoneInfos>().zone;

                            if (hit2.collider.gameObject.GetComponent<ZoneInfos>().zone == GenericClass.E_Zone.Totem)
                            {
                                GetComponent<SoldierBehavior>().Totem = hit2.collider.gameObject;
                            }
                        }
                        else
                        {
                            newZone = GetComponent<SoldierBehavior>()._zoneAttribute;
                        }
                    }
                    #endregion


                    if (plyrControl_Script.ManagerActivate) // feedactivate 
                    {
                        #region permet de nourrir un monstre avec un autre quand on le lache dessus
                        int layerMask3 = 1 << LayerMask.NameToLayer("Monster");  // ignore tous les layers sauf "Zone"
                        Vector3 ray3 = transform.position;
                        //Debug.DrawRay(ray2, -transform.up * 100, Color.green, 0.01f);
                        RaycastHit hit3;
                        if (Physics.Raycast(ray3, -transform.up * 100, out hit3, 100, layerMask3))
                        {
                            //transform.position = new Vector3(hit.point.x, 3, hit.point.z);
                            Tofeed = hit3.transform.gameObject;
                        }
                        else
                        {
                            Tofeed = null;
                        }
                        #endregion
                    }
                }
                else
                {
                    GetComponent<SoldierBehavior>()._zoneAttribute = newZone;
                    GetComponent<SoldierBehavior>().enabled = true;
                }
            }
            //plyrControl_Script.ChangeActivity(); //ça update pas
        }
    }

    void OnMouseDown()
    {
        if (plyrControl_Script.ManagerActivate || plyrControl_Script.FeedActivate && !plyrControl_Script.CamFollow_Script.CamMovable)
        {
            if (gameObject.transform.tag == "MyMonster")// && gameObject.GetComponent<SoldierBehavior>()._actionState == GenericClass.E_Action.Wait)
            {
                MouseDown = true;
                transform.gameObject.layer = LayerMask.NameToLayer("Default");
                //armyManager_Script.ShowZones(true);
                plyrControl_Script.CircleLimite.SetActive(true);
            }
        }
    }

    private void OnMouseUp()
    {
        if (plyrControl_Script.ManagerActivate || plyrControl_Script.FeedActivate && !plyrControl_Script.CamFollow_Script.CamMovable)
        {
            GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Run);

            if (gameObject.transform.tag == "MyMonster")// && gameObject.GetComponent<SoldierBehavior>()._actionState == GenericClass.E_Action.Wait)
            {
                MouseDown = false;
                transform.gameObject.layer = LayerMask.NameToLayer("Monster");
                //armyManager_Script.ShowZones(false);
                GetComponent<SoldierBehavior>().SelfIdle = false;
                plyrControl_Script.CircleLimite.SetActive(false);


                if (plyrControl_Script.ManagerActivate)// feedactivate 
                {
                    if (Tofeed != null)
                    {
                        if (Tofeed.GetComponent<Statistique>()._health < Tofeed.GetComponent<Statistique>()._startHealth)
                        {
                            int newHealth = (int)(Tofeed.GetComponent<Statistique>()._health + GetComponent<Statistique>()._health);
                            if (newHealth > Tofeed.GetComponent<Statistique>()._startHealth)
                            {
                                Tofeed.GetComponent<Statistique>()._health = Tofeed.GetComponent<Statistique>()._startHealth;
                                Tofeed.GetComponent<Statistique>().ChangeHealthValue((int)(Tofeed.GetComponent<Statistique>()._startHealth));
                            }
                            else
                            {
                                Tofeed.GetComponent<Statistique>().ChangeHealthValue(newHealth);
                            }
                            Destroy(transform.gameObject, 0.1f);
                        }
                    }
                }
            }
        }
    }
}
