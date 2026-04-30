public class CannonTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target == null) return;
		EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
		if (enemyHealth != null) {
			enemyHealth.TakeDamage(data.damage);
			print("Cannon shooted at " + target.name);
		}
	}
}