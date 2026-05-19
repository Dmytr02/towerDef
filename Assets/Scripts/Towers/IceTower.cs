using UnityEngine;

public class IceTower : BaseTower {
	protected override void Shoot() {
		UpdateTarget();
		if (target != null) {
			CreateProjectile();
		}
	}
}