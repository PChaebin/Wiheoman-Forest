using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Image totalPanel;

    [SerializeField] private TextMeshProUGUI dangerFruitTartPriceText;
    [SerializeField] private TextMeshProUGUI flowerTeaPriceText;
    [SerializeField] private TextMeshProUGUI slimeJamBreadText;

    [SerializeField] private TextMeshProUGUI totalPriceText;


    public void CloseRestaurant()
    {
        dangerFruitTartPriceText.text = $"�ܰſ��� Ÿ��Ʈ: {CustomerManager.instance.dangerFruitTartTotalPrice}��";
        flowerTeaPriceText.text = $"����: {CustomerManager.instance.flowerTeaTotalPrice}��";
        slimeJamBreadText.text = $"�������� �Ļ�: {CustomerManager.instance.slimeJamBreadTotalPrice}��";

        totalPriceText.text = $"Total: {CustomerManager.instance.totalPrice}��";

        totalPanel.gameObject.SetActive(!totalPanel.gameObject.activeSelf);
    }
}
