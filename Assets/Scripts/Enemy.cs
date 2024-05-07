using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int healthPoints;
    private int healthStart;
    [SerializeField] private int rewardAmount;
    [SerializeField] private Image healthBar;
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
        healthStart = healthPoints;
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
        if (isDead)
            return;
        if (healthPoints - hitPoints > 0)
        {
            healthPoints -= hitPoints;
            healthBar.fillAmount = (float)healthPoints / healthStart;
            anim.Play("Hurt");
            SoundManager.Instance.Play(SoundManager.Instance.Hit);
        }
        else
        {
            healthBar.fillAmount = 0;
            anim.SetTrigger("didDie");
            Die();
        }
    }
    void Die()
    {
        isDead = true;
        SoundManager.Instance.Play(SoundManager.Instance.Death);
        GameManager.Instance.AddMoney(rewardAmount);
        GameManager.Instance.TotalKilledOnWave++;
        GameManager.Instance.TotalKillEnemy++;
        //Debug.Log("TotalKilled" + GameManager.Instance.TotalKilled);
        Destroy(gameObject, 2f);
        gameObject.tag = "Untagged";
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Finish")
        {
            GameManager.Instance.TotalEnemyEscaped++;
            GameManager.Instance.TotalEscapeOnWave++;
        }
    }
}
