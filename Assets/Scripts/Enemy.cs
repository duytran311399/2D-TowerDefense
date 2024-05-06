using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    [SerializeField] private int rewardAmount;
    Animator anim;
    public float speed = 2f;

    private Transform taget;
    private int wavePointIndex = 0;
    private bool isDead;
    public bool IsDead
    {
        get { return isDead; }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        taget = Waypoints.points[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;
        Vector3 dir = taget.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, taget.position) <= 0.2f)
        {
            GetNextWayPoint();
        }
    }

    void GetNextWayPoint()
    {
        if (wavePointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject, 0.1f);
            return;
        }
        wavePointIndex++;
        taget = Waypoints.points[wavePointIndex];
    }

    public void EnemyHit(int hitPoints)
    {
        if (healthPoints - hitPoints > 0)
        {
            healthPoints -= hitPoints;
            anim.Play("Hurt");
            GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
        }
        else
        {
            anim.SetTrigger("didDie");
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        GameManager.Instance.TotalKilled += 1;
        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        GameManager.Instance.AddMoney(rewardAmount);
        Destroy(gameObject, 2f);
        gameObject.tag = "Untagged";
    }
}
