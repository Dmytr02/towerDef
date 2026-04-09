using UnityEngine;

public abstract class BaseTower : MonoBehaviour {
	public TowerData data;
	protected float fireCountdown = 0f;
	protected Transform target;

	protected virtual void Update() {
		UpdateTarget();

		if (target == null) return;

		if (fireCountdown <= 0f) {
			Shoot();
			fireCountdown = 1f / data.attackSpeed;
		}

		fireCountdown -= Time.deltaTime;
	}

	protected virtual void UpdateTarget() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies) {
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance) {
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= data.range) {
			target = nearestEnemy.transform;
		} else {
			target = null;
		}
	}

	protected abstract void Shoot();

	private void OnDrawGizmosSelected() {
		if (data == null) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, data.range);
	}
}