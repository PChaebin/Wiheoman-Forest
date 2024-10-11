using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Item
{
    public int itemID;
    public string itemName;
    public string itemDescription;
    public string itemAmount;
}

[System.Serializable]
public class ItemDataList
{
    public List<Item> Item;
}

[System.Serializable]
public class ItemDescription
{
    public int id;
    public string description;
}

[System.Serializable]
public class InventoryItemList
{
    public List<ItemDescription> itemDescription;
}

public class DroppedItemController : MonoBehaviour
{
    [SerializeField] private ItemAttribute itemAttribute;

    private ItemDataList itemDataList;
    private InventoryItemList inventoryList;

    private int ID;
    private string Description;

    private string itemTestPath;
    private string playerInventoryItemPath;

    void Start()
    {
        ReadJsonFile();        
    }

    private void ReadJsonFile()
    {
        itemTestPath = Application.dataPath + "/Resources/Json Files/ItemTest.json";
        playerInventoryItemPath = Application.dataPath + "/Resources/Json Files/PlayerInventoryItems.json";

        if (File.Exists(itemTestPath))
        {
            string itemJson = File.ReadAllText(itemTestPath);
            itemDataList = JsonUtility.FromJson<ItemDataList>(itemJson);
        }
        else
        {
            Debug.Log("ItemTest.json�� ã�� �� ��");
            return;
        }

        if (File.Exists(playerInventoryItemPath))
        {
            string inventoryJson = File.ReadAllText(playerInventoryItemPath);
            inventoryList = JsonUtility.FromJson<InventoryItemList>(inventoryJson);
        }
        else
        {
            // ������ ���� ��� ���ο� ����Ʈ�� ����
            inventoryList = new InventoryItemList();
        }
    }

    private void UpdateJson()
    {
        Item matchedItem = null;
        Debug.Log("�� �ȵǳ� ��������");
        foreach (var item in itemDataList.Item)
        {
            Debug.Log("�ݺ���");
            if (itemAttribute.ItemID == item.itemID)
            {
                Debug.Log("if��");
                matchedItem = item;
                break;
            }
        }

        if(matchedItem != null)
        {
            ID = matchedItem.itemID;
            Debug.Log("ItemId : " + ID);
            Debug.Log("ItemId2 : " + matchedItem.itemID);

            if (itemAttribute.ItemID == ID)
            {
                Description = matchedItem.itemDescription;
                Debug.Log("ItemDescription : " + Description);
                Debug.Log("ItemDescription2 : " + matchedItem.itemDescription);

                ItemDescription newItem = new ItemDescription
                {
                    id = ID,
                    description = Description
                };

                inventoryList.itemDescription.Add(newItem);

                string updateInventoryJson = JsonUtility.ToJson(inventoryList, true);
                File.WriteAllText(playerInventoryItemPath, updateInventoryJson);
                Debug.Log("PlayerInventoryItems.json ���� ������Ʈ");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("�÷��̾�� �浹!");

#warning ���߿� �÷��̾� �κ��丮 �����ؼ� �װ��� ������ �߰��� ��
        UpdateJson();
        Debug.Log($"���⼭ �������� �����߽��ϴ�");

        Destroy(gameObject);
    }
}
