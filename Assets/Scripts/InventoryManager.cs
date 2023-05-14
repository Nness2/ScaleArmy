using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> InterfaceField;
    public List<GenericClass.E_Loot> InventoryElements;

    //LootIMAGE
    public Sprite FlaskImg;
    public Sprite MeatImg;
    public Sprite BandageImg;

    //LootPrefab
    public GameObject FiolePrefab;
    public GameObject ViandePrefab;
    public GameObject BandagePrefab;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLootToInventory(GenericClass.E_Loot lootType)
    {
        Sprite UseImage = null;
        switch (lootType)
        {
            case GenericClass.E_Loot.Flask:
                UseImage = FlaskImg;
                break;
            case GenericClass.E_Loot.Meat:
                UseImage = MeatImg;
                break;
            case GenericClass.E_Loot.Bandage:
                UseImage = BandageImg;
                break;
        }

        for (int i = 0; i < InventoryElements.Count; i++)
        {
            if (InventoryElements[i] == GenericClass.E_Loot.None)
            {
                InventoryElements[i] = lootType;
                InterfaceField[i].GetComponent<Image>().sprite = UseImage;
                InterfaceField[i].GetComponent<Image>().enabled = true;
                break;
            }
        }
    }

    public GenericClass.E_Loot RemoveLootToInventory(GameObject Img)
    {
        GenericClass.E_Loot lootDeleted;
        int index = InterfaceField.IndexOf(Img);
        lootDeleted = InventoryElements[index];
        InventoryElements[index] = GenericClass.E_Loot.None;
        InterfaceField[index].GetComponent<Image>().enabled = false;
        return lootDeleted;
    }


    public GameObject InstantiateLoot(GenericClass.E_Loot loot)
    {
        GameObject choosedLoot = null;
        switch ((GenericClass.E_Loot)loot)
        {
            case GenericClass.E_Loot.Flask:
                choosedLoot = FiolePrefab;
                break;
            case GenericClass.E_Loot.Meat:
                choosedLoot = ViandePrefab;
                break;
            case GenericClass.E_Loot.Bandage:
                choosedLoot = BandagePrefab;
                break;
        }
        return choosedLoot;
    }
}
