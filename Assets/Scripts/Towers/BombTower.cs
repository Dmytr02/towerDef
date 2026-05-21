using UnityEngine;

public class BombTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
	public override string GetStats()
	{
		return $"damage: {data.damage} {(data.nextLvl != null ? $"->{data.nextLvl.damage}" : "")}\nrange: {data.range} {(data.nextLvl != null ? $"->{data.nextLvl.range}" : "")}\nattack speed: {data.attackSpeed} {(data.nextLvl != null ? $"->{data.nextLvl.attackSpeed}" : "")}\nexplosive radius: {data.aoeRadius} {(data.nextLvl != null ? $"->{data.nextLvl.aoeRadius}" : "")}";
	}
}