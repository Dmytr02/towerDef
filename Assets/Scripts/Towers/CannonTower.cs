public class CannonTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
}