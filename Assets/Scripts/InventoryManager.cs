using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> InterfaceField;
    public List<GenericClass.E_Loot> InventoryElements;

    void Start()
    {
        InventoryElements = new List<GenericClass.E_Loot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddLootToInventory(Sprite image, GenericClass.E_Loot lootType)
    {
        if(InventoryElements.Count < 3)
        {
            InventoryElements.Add(lootType);
            InterfaceField[InventoryElements.Count-1].GetComponent<Image>().sprite = image;
            InterfaceField[InventoryElements.Count-1].SetActive(true);
        }
    }
}
