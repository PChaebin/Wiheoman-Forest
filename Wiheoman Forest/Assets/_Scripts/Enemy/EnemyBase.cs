using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

/// <summary>
///     �ش� �� ĳ������ ����� ��Ÿ���ϴ�.
/// </summary>
public enum ERank
{
    empty,
    normal,
    elite,
    boss
}

/// <summary>
///     �ش� ���ʹ��� ���¸� ��Ÿ���ϴ�.
/// </summary>
[Serializable]
public struct EnemyStatus
{
    public float health;
    public int guardGauge;
    public ERank rank;
}

public abstract class EnemyBase : MonoBehaviour
{
    static protected GameObject playerGameObject = null;
    [SerializeField]
    protected float range = 0;
    [SerializeField]
    protected EnemyStatus stat;
    protected Rigidbody enemyRigidbody;
    protected bool isDead = false;
    
    /// <summary>
    ///     �ش� ���� �����ϰ� �����ϴ� �÷��̾��� ������ ���ʹ̿��� �˷��ݴϴ�.
    /// </summary>
    /// <param name="player"></param>
    static public void SetPlayer(GameObject player)
    {
        playerGameObject = player;
    }
    /// <summary>
    ///     �ش� �� ĳ���Ͱ� �÷��̾ �߰��ߴ��� ���θ� �ľ��մϴ�.
    /// </summary>
    /// <returns></returns>
    public bool IsFoundPlayer()
    {
        if (playerGameObject == null) return false;
        return (range * range) > (transform.position - playerGameObject.transform.position).sqrMagnitude;
    }
    /// <summary>
    ///     ���ʹ� ��ġ���� ����Ͽ� �÷��̾ �ִ� �������� �ٶ󺸴� ���͸� �����ϰ� �������ݴϴ�.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPseudoDirection()
    {
        if (IsFoundPlayer() == false) return Vector3.zero;
        float dx = playerGameObject.transform.position.x - transform.position.x;
        return (dx < 0) ? new Vector3(-1, 0, 0) : new Vector3(1, 0, 0);
    }
    /// <summary>
    ///     ������ ���� ��츦 �����մϴ�.
    /// </summary>
    public void BeAttacked(float damage, Vector3 knockBackDirection, float force)
    {
        // ===============================
        // ������ ���� ��� �ش� �Լ��� ȣ���մϴ�.
        // ���࿡ �߰����� ���Ƿ� ����, �����̳� ���� ���� �� ���� ��Ŀ������ ���������� ���, �ش� �Լ��� �����ϼ���.
        // ���� ���� ������ �ݶ��̴��� Ȱ���Ͽ� OnTriggerEnter / OnCollisionEnter ������ Ȱ���Ұ��ε� �̵��� ����� �������� Ȯ������ ���� �Ӵ���
        // ������ �÷��̾��� ������ �ݶ��̴��� � �±׸� ������ �ִ����� ���� �׳� �Լ��� ��������ϴ�.
        // ��������� ���߿� �ش� ������ ���ǵȴٸ� �ش� �Լ��� ȣ���ϼ���.
        // ===============================

        stat.health -= damage;
        if (stat.health <= 0.0f && (isDead == false))
        {
            DoDeathHandle();
        }
        if (enemyRigidbody != null)
        {
            enemyRigidbody.AddForce(knockBackDirection.normalized * force, ForceMode.Impulse);
        }
    }

    /// <summary>
    ///     �ش� �� ĳ������ ����� ó���ϴ� �Լ��Դϴ�. �ݵ�� �������ּ���. ����� �ڷ�ƾ ���� �� ���� ��� ��� ���� �ֽ��ϴ�.
    /// </summary>
    protected abstract void DoDeathHandle();
}
