using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public class SceneGenerator : MonoBehaviour {

	[SerializeField] private int xSize = 50;
	[SerializeField] private int zSize = 50;
	private int[,] _grid;
	public static Vector2Int _gridSize;
	public static Transform m_transform;

	[SerializeField, SerializeReference] private List<CellOfGrid> possibleCells = new();
	[SerializeField] private List<CellOfGrid> PathCells = new();

	private Dictionary<CellOfGrid, List<(Vector3 positions, Quaternion quaternion)>> InstancesData = new();

	private static List<Vector2Int> _Path = new List<Vector2Int>();

	public static List<Vector3> Path { get { return _Path.Select(n => m_transform.localToWorldMatrix.MultiplyPoint((new Vector3((n.x + 0.5f) / (float)_gridSize.x - 0.5f, 1, (n.y + 0.5f) / (float)_gridSize.y - 0.5f)))).ToList(); } }
	public static Vector3 enemyScale;
	private void Awake() {
		_gridSize = new Vector2Int(xSize, zSize);
		m_transform = transform;
		_grid = new int[xSize, zSize];
		for (int x = 0; x < _grid.GetLength(0); x++) {
			for (int z = 0; z < _grid.GetLength(1); z++) {
				_grid[x, z] = Random.Range(1, 10);
			}
		}
		for (int x = -4; x < 4; x++) {
			for (int z = -4; z < 4; z++) {
				_grid[_grid.GetLength(0) / 2 + x, _grid.GetLength(1) / 2 + z] = -1;
			}
		}
		_grid[0, 0] = -1;
	}

	private void Start() {
		Vector2Int[] way = WayGenerator.GenerateWay(_grid, new Pair<int, int>(5, 5), new Pair<int, int>(46, 46), new List<Pair<int, int>>() { new Pair<int, int>(0, 1), new Pair<int, int>(1, 0), new Pair<int, int>(0, -1), new Pair<int, int>(-1, 0) }, 1).Select(n => new Vector2Int(n.First, n.Second)).ToArray();
		_Path = way.ToList();
		HashSet<Vector2Int>[] buckets = new HashSet<Vector2Int>[33].Select(_ => new HashSet<Vector2Int>()).ToArray();
		Dictionary<Vector2Int, uint> possibleValue = new Dictionary<Vector2Int, uint>();

		for (int x = 0; x < xSize; x++)
			for (int z = 0; z < zSize; z++) {
				buckets[32].Add(new Vector2Int(x, z));
			}

		for (int i = 0; i < way.Length; i++) {
			int mask = 0;
			if (i != way.Length - 1) {
				if (way[i] + Vector2.down == way[i + 1]) mask |= 1 << 0;
				if (way[i] + Vector2.up == way[i + 1]) mask |= 1 << 1;
				if (way[i] + Vector2.left == way[i + 1]) mask |= 1 << 2;
				if (way[i] + Vector2.right == way[i + 1]) mask |= 1 << 3;
			}

			if (i != 0) {
				if (way[i] + Vector2.down == way[i - 1]) mask |= 1 << 0;
				if (way[i] + Vector2.up == way[i - 1]) mask |= 1 << 1;
				if (way[i] + Vector2.left == way[i - 1]) mask |= 1 << 2;
				if (way[i] + Vector2.right == way[i - 1]) mask |= 1 << 3;
			}


			InstancesData.TryAdd(PathCells[mask], new List<(Vector3, Quaternion)>());
			InstancesData[PathCells[mask]].Add((new Vector3((way[i].x + 0.5f) / (float)xSize - 0.5f, 1, (way[i].y + 0.5f) / (float)zSize - 0.5f), PathCells[mask].rotation));
			SetCell(ref buckets, ref possibleValue, PathCells[mask], way[i].x, way[i].y, 0);
			RemuveCell(ref buckets, ref possibleValue, way[i]);
		}


		while (true) {
			(Vector2Int pos, uint mask) current = PopCell(ref buckets, ref possibleValue);
			if (current.pos == new Vector2Int(-1, -1)) break;
			//Debug.Log($"{current.pos}");
			(CellOfGrid cell, Quaternion rotation) currentIndex = GetRandom(current.mask);


			InstancesData.TryAdd(currentIndex.cell, new List<(Vector3, Quaternion)>());
			InstancesData[currentIndex.cell].Add((new Vector3((current.pos.x + 0.5f) / (float)xSize - 0.5f, 1, (current.pos.y + 0.5f) / (float)zSize - 0.5f), currentIndex.rotation));
			SetCell(ref buckets, ref possibleValue, currentIndex.cell, current.pos.x, current.pos.y, Mathf.RoundToInt((currentIndex.rotation.eulerAngles.y - currentIndex.cell.rotation.eulerAngles.y) / 90));
		}
	}

	private void Update() {
		foreach (var data in InstancesData) {
			Matrix4x4[] matrix = data.Value.Select(n => {
				return transform.localToWorldMatrix * Matrix4x4.TRS(n.positions,
					(n.quaternion.Equals(new Quaternion(0, 0, 0, 0))) ? Quaternion.identity : n.quaternion,
					new Vector3(1 / (float)xSize, 0.02f, 1 / (float)zSize));
			}).ToArray();
			Graphics.DrawMeshInstanced(data.Key.mesh, 0, data.Key.mat, matrix);
		}

		enemyScale = transform.lossyScale * 0.01f;
	}

	IEnumerator Corutine(HashSet<Vector2Int>[] buckets, Dictionary<Vector2Int, uint> possibleValue) {
		yield return null;
	}
	private void SetCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue, CellOfGrid cell, int x, int y, int rotation) {
		//Debug.Log($"rotation - {rotation}");
		rotation = (rotation + 2) % 4;
		uint num = (cell.sides << 8 * rotation) | (cell.sides >> 32 - 8 * rotation);

		SetCell(ref buckets, ref possibleValue, (num & 0xFF000000) | 0x00FFFFFF, new Vector2Int(x - 1, y));
		SetCell(ref buckets, ref possibleValue, (num & 0x00FF0000) | 0xFF00FFFF, new Vector2Int(x, y - 1));
		SetCell(ref buckets, ref possibleValue, (num & 0x0000FF00) | 0xFFFF00FF, new Vector2Int(x + 1, y));
		SetCell(ref buckets, ref possibleValue, (num & 0x000000FF) | 0xFFFFFF00, new Vector2Int(x, y + 1));
	}

	private Vector2Int GetCell(HashSet<Vector2Int>[] buckets) {
		for (int i = 0; i < buckets.Length; i++) {
			if (buckets[i].Count != 0) {
				return buckets[i].First();
			}
		}
		return new Vector2Int(-1, -1);
	}

	private (Vector2Int pos, uint mask) PopCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue) {
		(Vector2Int pos, uint mask) result = (new Vector2Int(-1, -1), 0);
		for (int i = 0; i < buckets.Length; i++) {
			if (buckets[i].Count != 0) {
				result.pos = buckets[i].First();
				buckets[i].Remove(result.pos);
				break;
			}
		}
		result.mask = possibleValue.GetValueOrDefault(result.pos, uint.MaxValue);
		possibleValue.Remove(result.pos);
		return result;
	}
	private void SetCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue, uint num, Vector2Int cell) {
		if (cell.x >= xSize || cell.y >= zSize || cell.x < 0 || cell.y < 0) return;
		for (int i = 0; i < buckets.Length; i++)
			if (buckets[i].Contains(cell)) {
				buckets[i].Remove(cell);
				if (!possibleValue.TryAdd(cell, num)) possibleValue[cell] &= num;
				buckets[PopCount(possibleValue[cell])].Add(cell);
			}
	}

	private void RemuveCell(ref HashSet<Vector2Int>[] buckets, ref Dictionary<Vector2Int, uint> possibleValue, Vector2Int cell) {
		for (int i = 0; i < buckets.Length; i++) {
			buckets[i].Remove(cell);
			possibleValue.Remove(cell);
		}
	}

	public static int PopCount(uint i) {
		i = i - ((i >> 1) & 0x55555555);
		i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
		return (int)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
	}

	private (CellOfGrid, Quaternion) GetRandom(uint mask) {
		float totalWeight = 0;
		foreach (var cell in possibleCells) {
			for (int r = 0; r < 4; r++) {
				if (CheckMask(cell, mask, r)) totalWeight += cell.chance;
			}
		}

		float value = Random.Range(0, totalWeight);
		float currentSum = 0;

		foreach (var cell in possibleCells) {
			for (int r = 0; r < 4; r++) {
				if (CheckMask(cell, mask, r)) {
					currentSum += cell.chance;
					if (currentSum >= value)
						return (cell, Quaternion.Euler(0, cell.rotation.eulerAngles.y + 90 * r, 0));
				}
			}
		}
		return (possibleCells[0], Quaternion.identity);
	}

	private bool CheckMask(CellOfGrid cell, uint environmentMask, int rotation) {
		uint rotatedEnv = (environmentMask >> 8 * rotation) | (environmentMask << (32 - 8 * rotation));

		bool match = ((((cell.num >> 24) & (rotatedEnv >> 24)) & 0xFF) != 0) &&
					 ((((cell.num >> 16) & (rotatedEnv >> 16)) & 0xFF) != 0) &&
					 ((((cell.num >> 8) & (rotatedEnv >> 8)) & 0xFF) != 0) &&
					 (((cell.num & rotatedEnv) & 0xFF) != 0);

		return match;
	}
}