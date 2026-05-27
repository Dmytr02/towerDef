using UnityEngine;

public class Projectile : MonoBehaviour {
	private Transform target;
	private TowerData data;

	private Vector3 startPosition;
	private Vector3 lastTargetPosition;
	private float progress = 0f;
	private float towerScale = 0f;

	public void Seek(Transform _target, TowerData _data, Vector3 towerScale) {
		target = _target;
		data = _data;
		startPosition = transform.position;
		if (target != null) lastTargetPosition = target.position;

		transform.localScale = Vector3.Scale(transform.localScale, towerScale);
		this.towerScale = towerScale.x;
	}

	void Update() {
		if (target != null) {
			lastTargetPosition = target.position;
		}

		float distanceTotal = Vector3.Distance(startPosition, lastTargetPosition);
		//print(distanceTotal/towerScale + " | " + ((data.projectileSpeed / distanceTotal) * Time.deltaTime*towerScale));
		if (distanceTotal <= 0.1f*towerScale) { HitTarget(); return; }

		progress += (data.projectileSpeed / distanceTotal) * Time.deltaTime*towerScale;
		
		if (progress >= 1f) {
			HitTarget();
			return;
		}

		Vector3 currentPos = Vector3.Lerp(startPosition, lastTargetPosition, progress);

		float arc = 4f * data.arcHeight * towerScale * progress * (1f - progress);
		currentPos.y += arc;

		transform.position = currentPos;

		transform.LookAt(currentPos + transform.forward);
	}

	void HitTarget() {
		if (data.aoeRadius > 0) {
			Explode();
		} else if (target != null) {
			ApplyEffect(target);
		}
		Destroy(gameObject);
	}

	void ApplyEffect(Transform enemy) {
		if (data.damage > 0 && enemy.TryGetComponent(out EnemyHealth health)) {
			health.TakeDamage(data.damage);
		}

		if (data.slowDuration > 0 && data.slowAmount != 1 && enemy.TryGetComponent(out WaypointManager move)) {
			if (data.slowAmount > 0) {
				move.ApplySlow(data.slowAmount, data.slowDuration);
			}
		}
	}

	void Explode() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, data.aoeRadius);
		foreach (Collider collider in colliders) {
			if (collider.CompareTag("Enemy")) {
				ApplyEffect(collider.transform);
			}
		}
	}
}