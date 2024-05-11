using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SlimeController : EnemyBase
{
    [SerializeField]
    private float jumpInterval;
    private Coroutine jumpCoroutine;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float moveForce;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        jumpCoroutine = StartCoroutine(Jump());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void DoDeathHandle()
    {
        // ===============================
        // �������� ��������Ƿ� ���̻� ������ ���� �ʽ��ϴ�.
        // ===============================
        StopCoroutine(jumpCoroutine);
        Debug.Log("�������� ����߽��ϴ�.");
    }
    // ===============================
    // TODO : �ش� �������� ������ �ؼ� Ƣ������� ���ȿ�
    // �������� �÷��̾�� ��� �� ��� �������� ������ �ؾ� �մϴ�.
    // ������ �÷��̾�� �������� �ִ� �Լ��� ���� �÷��̾� ü�¿� �����ϴ� � ���ϵ� ǥ���� �����.
    // =============================== 
    /// <summary>
    ///     �������� jumpInterval ��ŭ�� �ֱ�� ���� Ƣ������� �Լ��Դϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Jump()
    {
        while (true)
        {
            yield return new WaitForSeconds(jumpInterval);
            if (enemyRigidbody == null)
            {
                Debug.LogError("SlimeController.AttackPlayer()���� ������ٵ� ã�� �� �����ϴ�.");
            }
            Debug.Log($"found player : {IsFoundPlayer()}");
            // ����
            enemyRigidbody.AddForce(
                new Vector3(0, jumpForce, 0) + GetPseudoDirection() * moveForce, 
                ForceMode.VelocityChange);
        }
    }
}
