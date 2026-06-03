using UnityEngine;

public class BombTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
	public override string GetStats(bool isUpgrade = true)
	{
		return base.GetStats(isUpgrade) + $"damage: {data.damage} {((data.nextLvl != null && isUpgrade) ? $"->{data.nextLvl.damage}" : "")}\nrange: {data.range} {((data.nextLvl != null && isUpgrade) ? $"->{data.nextLvl.range}" : "")}\nattack speed: {data.attackSpeed} {(data.nextLvl != null&& isUpgrade ? $"->{data.nextLvl.attackSpeed}" : "")}\nexplosive radius: {data.aoeRadius} {(data.nextLvl != null&& isUpgrade ? $"->{data.nextLvl.aoeRadius}" : "")}";
	}
}