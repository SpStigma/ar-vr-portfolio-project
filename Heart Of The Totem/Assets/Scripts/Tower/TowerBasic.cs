using UnityEngine;

public class TowerBasic :TowerStats
{
    private Transform target;
    private float attackCooldown;

    public GameObject projectileToFire;
    public Transform firePoint;
    private GameObject terrain;

    void Start()
    {
        attackCooldown = 1f / attackSpeed;
        terrain = GameObject.FindGameObjectWithTag("Terrain");
    }

    void Update()
    {
        FindTarget();

        if (target != null)
        {
            attackCooldown -= Time.deltaTime;

            if (attackCooldown <= 0f)
            {
                ShootProjectile();
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                target = hit.transform;
                return;
            }
        }

        target = null;
    }

    public void ShootProjectile()
    {
        GameObject projectileInstance = Instantiate(projectileToFire, firePoint.position, Quaternion.identity);

        Vector3 terrainScale = terrain.transform.lossyScale;
        projectileInstance.transform.localScale = new Vector3(
            projectileInstance.transform.localScale.x * terrainScale.x,
            projectileInstance.transform.localScale.y * terrainScale.y,
            projectileInstance.transform.localScale.z * terrainScale.z
        );

        ProjectileBehavior projectile = projectileInstance.GetComponent<ProjectileBehavior>();

        if (projectile != null)
        {
            projectile.SetTarget(target, damage);
            AudioManager.instance.PlaySFX(0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}