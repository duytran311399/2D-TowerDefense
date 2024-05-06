using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    rock,
    arrow,
    fireball
};

public class Projectile : MonoBehaviour {
    [SerializeField]
    private int attackStrength;

    [SerializeField]
    private ProjectileType projectileType;

    public Transform target;
    public float speed = 10f;
    public GameObject impactEffect;

    public int AttackStrength
    {
        get { return attackStrength; }
    }

    public ProjectileType ProjectileType
    {
        get { return projectileType; }
    }

    public void seekTarget(Transform _target)
    {
        target = _target;
    }
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;                           // khoang cach trong 1 khung
        if (dir.magnitude <= distanceThisFrame)                                     //neu do dai vector <= khoang cach vien dan trong 1 khung
        {
            HitTarget();
            //Destroy(target.gameObject);
            //Debug.Log("magnitude: " + dir.magnitude);
            return;
        }
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
    }
    /// <summary>
    /// //////// ban chung cai j do thi destroy bullet
    /// </summary>
    void HitTarget()
    {
        if(impactEffect != null)
        {
            GameObject effectInt = Instantiate(impactEffect, target.position, target.rotation);
            Destroy(effectInt, 2f);
        }
        Destroy(gameObject);
        var enemy = target.GetComponent<Enemy>();
        if(enemy != null && !enemy.IsDead)
        {
            enemy.EnemyHit(AttackStrength);
        }
    }
}
