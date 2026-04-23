using UnityEngine;

public class IceTower : BaseTower {
	protected override void Shoot() {
		Collider[] colliders = Physics.OverlapSphere(target.position, data.aoeRadius, LayerMask.GetMask("Enemy"));
		foreach (Collider collider in colliders) {
			if (collider.TryGetComponent<EnemyHealth>(out EnemyHealth health)) {
				health.TakeDamage(data.damage);
			}
			if (collider.TryGetComponent<WaypointManager>(out WaypointManager move)) {
				move.ApplySlow(data.slowAmount, data.slowDuration);
			}
		}
		Debug.Log("Ice Blast at " + target.position);
	}
}