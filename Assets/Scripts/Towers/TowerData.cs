using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/TowerData")]
public class TowerData : ScriptableObject {
	public string towerName;
	public Mesh mesh;
	public int cost;
	public int recoverCost;
	public Vector2Int size;
	public TowerData nextLvl;

	[Header("Stats")]
	public float damage;
	public float attackSpeed; 
	public float range;
	public float aoeRadius;
	public Projectile projectilePrefab;
	public float projectileSpeed = 10f;
	public float arcHeight = 2f;

	[Header("Special Effects")]
	public float slowAmount = 0.4f;
	public float slowDuration = 2.5f;
}