using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required when using Event data.

public class InvImageInput : MonoBehaviour, IPointerDownHandler
{
    private GameObject Gm_Script;
    private InventoryManager Inv_Script;
    public GenericClass.E_Loot Selected; // L'id de l'objet selectionné
    public GameObject FloatObj;

    void Start()
    {
        Gm_Script = GameObject.FindGameObjectWithTag("GameManager");
        Inv_Script = Gm_Script.GetComponent<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && Selected != GenericClass.E_Loot.None)
        {
            if(FloatObj != null)
            {
                if(FloatObj.GetComponent<FeedObject>().Tofeed == null)
                {
                    Inv_Script.AddLootToInventory(Selected);
                    Selected = GenericClass.E_Loot.None;
                    Destroy(FloatObj);
                }
                if (FloatObj.GetComponent<FeedObject>().Tofeed != null)
                {
                    GameObject Tofeed = FloatObj.GetComponent<FeedObject>().Tofeed;
                    Tofeed.GetComponent<AnimationManager>()._animator.SetInteger("State", (int)GenericClass.E_MonsterAnimState.Idle);
                    AttributeLoot(Selected);

                    Selected = GenericClass.E_Loot.None;
                    Destroy(FloatObj, 0.1f);

                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Selected = Inv_Script.RemoveLootToInventory(gameObject);
        FloatObj = Instantiate(Inv_Script.InstantiateLoot(Selected), Vector3.zero, Quaternion.identity);
        Rigidbody gameObjectsRigidBody = FloatObj.AddComponent<Rigidbody>();
        FloatObj.GetComponent<FeedObject>().Owned = true;

    }

    public void AttributeLoot(GenericClass.E_Loot loot)
    {
        GameObject Tofeed = FloatObj.GetComponent<FeedObject>().Tofeed;
        switch (loot)
        {
            case GenericClass.E_Loot.Flask:
                Tofeed.GetComponent<Statistique>().GetComponent<Statistique>().ChangeHealthValue((int)Tofeed.GetComponent<Statistique>()._startHealth);
                Tofeed.GetComponent<Statistique>().UpdateStatInfoPanel();
                break;
            case GenericClass.E_Loot.Meat:
                Tofeed.GetComponent<FeedManager>().MonsterLevelUp(Tofeed);
                break;
            case GenericClass.E_Loot.Bandage:
                Tofeed.GetComponent<Statistique>().GetComponent<Statistique>().ChangeHealthValue((int)Tofeed.GetComponent<Statistique>()._startHealth);
                Tofeed.GetComponent<Statistique>().UpdateStatInfoPanel();
                break;
        }
    }
}
