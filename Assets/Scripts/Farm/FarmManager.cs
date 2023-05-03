using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    private bool MouseDown;
    private GameObject Tofeed;


    void Start()
    {
        MouseDown = false;
        Tofeed = null;
    }

    void Update()
    {

        if (gameObject.transform.tag == "MyMonster")// && gameObject.GetComponent<SoldierBehavior>()._actionState == GenericClass.E_Action.Wait)
        {
            if (MouseDown)
            {

                //int groundLayer = LayerMask.NameToLayer("Default");
                #region permet de surelever le monster quand on clique dessus
                int layerMask = 1 << LayerMask.NameToLayer("Ground"); // ignore tous les layers sauf "Ground"

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 0.01f);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, layerMask))
                {
                    transform.position = new Vector3(hit.point.x, 3+1.5f, hit.point.z);
                    //GetComponent<AIMovement>().StopMoving();
                    GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Floating);
                    //Debug.Log(hit.transform.name);
                }
                #endregion

                #region permet de nourrir un monstre avec un autre quand on le lache dessus
                int layerMask3 = 1 << LayerMask.NameToLayer("Monster");  // ignore tous les layers sauf "Zone"
                Vector3 ray3 = transform.position;
                //Debug.DrawRay(ray2, -transform.up * 100, Color.green, 0.01f);
                RaycastHit hit3;
                if (Physics.Raycast(ray3, -transform.up * 100, out hit3, 100, layerMask3))
                {
                    //transform.position = new Vector3(hit.point.x, 3, hit.point.z);
                    Tofeed = hit3.transform.gameObject;
                    Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Eat);
                }
                else
                {
                    if (Tofeed != null)
                        Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
                    Tofeed = null;
                }
                #endregion


            }
        }
        //plyrControl_Script.ChangeActivity(); //ça update pas
    }

    void OnMouseDown()
    {

        MouseDown = true;
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        //armyManager_Script.ShowZones(true);

    }

    private void OnMouseUp()
    {

        GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Run);

        if (gameObject.transform.tag == "MyMonster")
        {
            MouseDown = false;
            transform.gameObject.layer = LayerMask.NameToLayer("Monster");


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
                Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);

            }
        }
    }
}
