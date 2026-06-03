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
	
	public override string GetStats(bool isUpgrade = true)
	{
		return base.GetStats(isUpgrade) + $"slow straight: {data.slowAmount} {(data.nextLvl != null&& isUpgrade ? $"->{data.nextLvl.slowAmount}" : "")}\nrange: {data.range} {(data.nextLvl != null&& isUpgrade ? $"->{data.nextLvl.range}" : "")}";
	}
}