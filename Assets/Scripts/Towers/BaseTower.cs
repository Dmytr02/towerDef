using UnityEngine;

public abstract class BaseTower : MonoBehaviour {
	public TowerData data;
	protected float fireCountdown = 0f;
	protected Transform target;

	protected virtual void Update() {
		if (fireCountdown <= 0f) {
			Shoot();
			Debug.Log("update");
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

		if (shortestDistance <= data.range) {
			target = nearestEnemy.transform;
		} else {
			target = null;
		}
	}

	protected abstract void Shoot();

	private void OnDrawGizmosSelected() {
		if (data == null) return;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position+Vector3.up*0.05f, data.range);
	}
}