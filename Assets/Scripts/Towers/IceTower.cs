using UnityEngine;

public class IceTower : BaseTower {
	protected override void Shoot() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, data.range*0.5f, LayerMask.GetMask("Enemy"));
		foreach (Collider collider in colliders) {
			if (collider.TryGetComponent(out EnemyHealth health)) {
				health.TakeDamage(data.damage);
			}
			if (collider.TryGetComponent(out WaypointManager move)) {
				move.ApplySlow(data.slowAmount, data.slowDuration);
			}
		}
	}
	
	public override string GetStats()
	{
		return $"slow straight: {data.slowAmount} {(data.nextLvl != null ? $"->{data.nextLvl.slowAmount}" : "")}\nrange: {data.range} {(data.nextLvl != null ? $"->{data.nextLvl.range}" : "")}";
	}
}