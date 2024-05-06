using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    [SerializeField] private Projectile projectile;      //Type of Projectile
    [SerializeField] private Transform spawnProjectile;
    [SerializeField] private Transform circleRange;
    /// <summary>
    /// Turret
    /// </summary>
    private Transform target;
    [Header("Attribute")]
    public float range = 15f;
    public float fireRate = 1.0f;
    private float fireCountdown = 0f;
    public string enemyTag = "enemy";

    // Use this for initialization
    void Start () {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.1f);
        circleRange.localScale = Vector3.one * range * 2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (target == null)
            return;
        //////////// Bo dem nguoc turret fire
        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
        #region
        //fireCountdown -= Time.deltaTime;
        //attackCounter -= Time.deltaTime;
        ////If our closest enemy in range and if its within our attackRange, set our target enemy to the closest enemy in range.
        //if (targetEnemy == null || targetEnemy.IsDead)
        //{
        //    Enemy closestEnemy = GetClosestEnemyInRange();
        //    if(closestEnemy != null && Vector2.Distance(transform.localPosition, closestEnemy.transform.position) <= attackRange)
        //    {
        //        targetEnemy = closestEnemy;
        //    }
        //}
        //else
        //{
        //    if(attackCounter <= 0f)
        //    {
        //        isAttacking = true;
        //        attackCounter = timeBetweenAttacks; //reset attack counter
        //    }
        //    else
        //    {
        //        isAttacking = false;
        //    }
        //    //If enemy gets out of attack range, then that enemy can no longer be targeted
        //    if (Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRange)
        //    {
        //        targetEnemy = null;
        //    }
        //}
        #endregion
    }
    //void FixedUpdate()
    //{
    //    if (isAttacking) { Attack(); }

    //}
    //public void Attack()
    //{
    //    isAttacking = false;
    //    Projectile newProjectile = Instantiate(projectile) as Projectile;
    //    newProjectile.transform.localPosition = transform.localPosition;

    //    if (newProjectile.ProjectileType == ProjectileType.arrow)
    //    {
    //        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
    //    }
    //    else if (newProjectile.ProjectileType == ProjectileType.fireball)
    //    {
    //        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Fireball);
    //    }
    //    else if (newProjectile.ProjectileType == ProjectileType.rock)
    //    {
    //        GameManager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
    //    }
    //    //If we have a target enemy, start a coroutine to shoot projectile to target enemy
    //    if (targetEnemy == null)
    //    {
    //        Destroy(newProjectile);
    //    }
    //    else
    //    {
    //        StartCoroutine(MoveProjectile(newProjectile));
    //    }
    //}
    void Shoot()
    {
        var dir = target.transform.position - spawnProjectile.position;
        var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                         //Angle of the projectile
        //projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);  //Rotation of projectile
        Projectile bulletGO = Instantiate(projectile, spawnProjectile.position, Quaternion.AngleAxis(angleDirection, Vector3.forward)) as Projectile;
        if (bulletGO != null)
        {
            bulletGO.seekTarget(target);
        }
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;                    // khoang cach vs enemy gan nhat
        GameObject nearestEnemy = null;                             // enemy gan nhat
        foreach (GameObject enemy in enemies)
        {
            float distanceEnemy = Vector3.Distance(transform.position, enemy.transform.position); // khoang cach cua tung enemy
            if (distanceEnemy <= shortestDistance)                  // kiem tra khoang cach cua tung enemy vs khong cach ngan nhat
            {
                shortestDistance = distanceEnemy;                   // gan khoang cach ngan nhat = khoang cach vs enemy
                nearestEnemy = enemy;                               // gan enemy gan nhat = enemy
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    public void OnSellectTower()
    {
        circleRange.gameObject.SetActive(true);
    }
    public void OnDeSellectedTower()
    {
        circleRange.gameObject.SetActive(false);
    }
    ///Move Projectile to Target Enemy
    //IEnumerator MoveProjectile(Projectile projectile)
    //{
    //    while(getTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
    //    {
    //        var dir = targetEnemy.transform.localPosition - transform.localPosition;
    //        var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                         //Angle of the projectile
    //        projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);  //Rotation of projectile
    //        projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime); //Move Projectile
    //        yield return null;
    //    }
    //    if (projectile != null || targetEnemy == null)
    //    {
    //        Destroy(projectile);
    //    }
    //}

    ///Get the current target's distance
    //private float getTargetDistance(Enemy enemy)
    //{
    //    if(enemy == null)
    //    {
    //        enemy = GetClosestEnemyInRange();
    //        if(enemy == null)
    //        {
    //            return 0f;
    //        }
    //    }
    //    return Mathf.Abs(Vector2.Distance(transform.localPosition, enemy.transform.localPosition));
    //}
    ///Get Enemies in Attack Range
    //private List<Enemy> GetEnemiesInRange()
    //{
    //    List<Enemy> enemiesInRange = new List<Enemy>();

    //    //Check if enemies are in range
    //    foreach(Enemy enemy in GameManager.Instance.EnemyList)
    //    {
    //        if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRange && !enemy.IsDead)
    //        {
    //            enemiesInRange.Add(enemy);
    //        }
    //    }
    //    return enemiesInRange;
    //}
    /////Get Closest Enemy - Foreach enemy in range, get the closest enemy
    //private Enemy GetClosestEnemyInRange()
    //{
    //    Enemy closestEnemy = null;
    //    float smallestDistance = float.PositiveInfinity; 

    //    foreach(Enemy enemy in GetEnemiesInRange())
    //    {
    //        if(Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
    //        {
    //            smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
    //            closestEnemy = enemy;
    //        }
    //    }
    //    return closestEnemy;
    //}
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
