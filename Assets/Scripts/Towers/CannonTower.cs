public class CannonTower : BaseTower {
	protected override void Shoot() {
		EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
		if (enemyHealth != null) {
			enemyHealth.TakeDamage(data.damage);
			print("Cannon shooted at " + target.name);
		}
	}
}