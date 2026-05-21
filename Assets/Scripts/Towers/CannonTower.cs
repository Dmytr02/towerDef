public class CannonTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
	public override string GetStats()
	{
		return $"damage: {data.damage} {(data.nextLvl != null ? $"->{data.nextLvl.damage}" : "")}\nrange: {data.range} {(data.nextLvl != null ? $"->{data.nextLvl.range}" : "")}\nattack speed: {data.attackSpeed} {(data.nextLvl != null ? $"->{data.nextLvl.attackSpeed}" : "")}";
	}
}