using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image totalPanel;
    [SerializeField] private TextMeshProUGUI totalPriceText;

    private float totalPrice;

    public void CloseRestaurant()
    {
        totalPrice = CustomerManager.instance.totalPrice;

        totalPriceText.text = $"Total: {totalPrice}��";
        print($"Total Price: {totalPrice}��");

        totalPanel.gameObject.SetActive(!totalPanel.gameObject.activeSelf);
    }
}
