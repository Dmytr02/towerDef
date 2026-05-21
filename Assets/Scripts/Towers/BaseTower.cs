using UnityEngine;

public abstract class BaseTower : MonoBehaviour {
	public TowerData data;
	protected float fireCountdown = 0f;
	protected Transform target;
	public MeshFilter TowerMeshFilter;

	protected virtual void Update() {
		if (fireCountdown <= 0f) {
			Shoot();
			fireCountdown = data.attackSpeed;
		}

		fireCountdown -= Time.deltaTime;
	}

	protected virtual void UpdateTarget() {
		GameObject[] enemies = EnemyManager.Instance.enemies.ToArray();
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies) {
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance) {
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (shortestDistance <= data.range*0.5f) {
			target = nearestEnemy.transform;
		} else {
			target = null;
		}
	}

	protected void CreateProjectile() {
		if (target == null || data.projectilePrefab == null) return;

		Vector3 spawnPos = transform.position + Vector3.up * (transform.lossyScale.y * 0.5f);

		Projectile proj = Instantiate(data.projectilePrefab, spawnPos, Quaternion.identity);
		
		if (proj != null) {
			proj.Seek(target, data, transform.lossyScale);
		}
	}

	protected abstract void Shoot();

	public virtual string GetStats()
	{
		return "";
		
	}

	private void OnDrawGizmosSelected() {
		if (data == null) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position+Vector3.up*0.05f, data.range);
	}
}