using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    [Header("Customer State")]
    [SerializeField] private bool isWaiting = true;
    [SerializeField] private int happiness = 100;
    [SerializeField] private float tipPercentage;
    [SerializeField] private float tipModifier = 1f;

    [Space(10)][Header("Order")]
    [SerializeField] private GameObject orderBubble;
    [SerializeField] private TextMeshProUGUI orderText;
    [SerializeField] private Image orderImage;
    [SerializeField] private ItemAttribute[] menus;

    [Space(10)][Header("Food Rank")]
    [SerializeField] private EFoodRank foodRank;


    private void Start()
    {
        Enter();
        StartCoroutine(DecreaseHappinessOverTime());
    }

    private void Enter()
    {
        string[] entryLines = { "�ȳ��ϼ���~", "��.. ����� ������ �߳׿�.." };
        string random = entryLines[Random.Range(0, entryLines.Length)];
        orderText.text = random;

        float randomCount = Random.Range(2, 10);
        Invoke("OrderMenu", randomCount);
    }

    /// <summary>
    /// ��ȹ ������ �ٸ�, �켱 ���� ������ ���� �ູ���� 50 �̸��̸� �������� 50%�� ���� �޴� ������ �� 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseHappinessOverTime()
    {
        while (isWaiting && happiness > 0)
        {
            yield return new WaitForSeconds(1f);
            happiness -= 1;

            if (happiness <= 50)
            {
                tipModifier = 0.5f;
            }
        }
    }

    private void OrderMenu()
    {
        isWaiting = false;
        orderImage.gameObject.SetActive(true);

        ItemAttribute[] menuType = { menus[0], menus[1], menus[2] };
        int random = Random.Range(0, 3);

        orderText.text = menuType[random].ItemName + "(��)�� �ּ���!";
        orderImage.sprite = menuType[random].ItemImage;
    }

    public void ServeMenu()
    {
        switch (foodRank)
        {
            case EFoodRank.Good:
                tipPercentage = Random.Range(0.2f, 0.3f) * tipModifier;
                GiveTip();
                break;
            case EFoodRank.Standard:
                tipPercentage = 0f;
                GiveTip();
                break;
            case EFoodRank.Bad:
                tipPercentage = -0.5f * tipModifier;
                GiveTip();
                break;
            case EFoodRank.None:
                Debug.LogError("EFoodRank�� �������� �ʾҽ��ϴ�");
                break;
        }

        Invoke("Exit", 3f);
    }

    private void GiveTip()
    {
        float menuPrice = 3000f;
        float tip = menuPrice * tipPercentage;
        int clampTip = Mathf.FloorToInt(tip);
        float finalPrice = clampTip + menuPrice;

        orderText.text = "�����! +" + finalPrice;
    }

    private void Exit()
    {
        string exitLine = "";

        switch (foodRank)
        {
            case EFoodRank.Good:
                exitLine = "�ְ��� �Ļ翴���!";
                break;
            case EFoodRank.Standard:
                exitLine = "�����ϼ���~";
                break;
            case EFoodRank.Bad:
                exitLine = "���� �׷��ҰԿ�;;";
                break;
            case EFoodRank.None:
                Debug.LogError("EFoodRank�� �������� �ʾҽ��ϴ�");
                break;
        }
        orderText.text = exitLine;

        StartCoroutine(LeaveAfterServing());
    }

    private IEnumerator LeaveAfterServing()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("�մ��� �������ϴ�.");
        gameObject.SetActive(false);
    }
}
