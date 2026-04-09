using UnityEngine;

public class IceTower : BaseTower {
	protected override void Shoot() {
		Collider[] colliders = Physics.OverlapSphere(target.position, data.aoeRadius);
		foreach (Collider collider in colliders) {
			if (collider.CompareTag("Enemy")) {
				EnemyHealth eh = collider.GetComponent<EnemyHealth>();
				if (eh != null) {
					eh.TakeDamage(data.damage);
					/*EnemyMovement movement = collider.GetComponent<EnemyMovement>();
					if (movement != null) {
						movement.ApplySlow(data.slowAmount, data.slowDuration);
					}*/
				}
			}
		}
	}
}