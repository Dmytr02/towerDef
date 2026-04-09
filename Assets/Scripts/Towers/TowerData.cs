using UnityEngine;

[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/TowerData")]
public class TowerData : ScriptableObject {
	public string towerName;
	public GameObject prefab;
	public int cost;

	[Header("Stats")]
	public float damage;
	public float attackSpeed; 
	public float range;
	public float aoeRadius;

	[Header("Special Effects")]
	public float slowAmount = 0.4f;
	public float slowDuration = 2.5f;
}