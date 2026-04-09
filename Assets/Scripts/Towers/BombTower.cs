using UnityEngine;

public class BombTower : BaseTower {
	protected override void Shoot() {
		Collider[] colliders = Physics.OverlapSphere(target.position, data.aoeRadius);
		foreach (Collider collider in colliders) {
			if (collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
				enemy.TakeDamage(data.damage);
			
		}
		Debug.Log("Bomb blowed!");
	}
}