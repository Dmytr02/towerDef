using UnityEngine;

public class BombTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
}