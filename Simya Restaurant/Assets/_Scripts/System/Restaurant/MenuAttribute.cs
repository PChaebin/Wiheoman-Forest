using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Menu", menuName = "Add/Menu")]
public class MenuAttribute : ScriptableObject
{
    [Header("�޴� ID (�ߺ� X)")]
    [SerializeField] private int mMenuID;
    public int ItemID
    {
        get
        {
            return mMenuID;
        }
    }

    [Header("�޴� �̸�")]
    [SerializeField] private string mMenuName;
    public string ItemName
    {
        get
        {
            return mMenuName;
        }
    }

    [Header("�޴� �̹���")]
    [SerializeField] private Sprite mMenuImage;
    public Sprite ItemImage
    {
        get
        {
            return mMenuImage;
        }
    }
}
