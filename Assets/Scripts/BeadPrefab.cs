
using UnityEngine;

public class BeadPrefab : MonoBehaviour
{

	class PoolObject
	{
		public Transform transform;
		public bool inUse;
		public PoolObject(Transform t) { transform = t; }
		public void Use() { inUse = true; }
		public void Dispose() { inUse = false; }
	}

	[System.Serializable]
	public struct XSpawnRange
	{
		public float minX;
		public float maxX;
	}

	[System.Serializable]
	public struct YSpawnRange
	{
		public float minY;
		public float maxY;
	}

	[System.Serializable]
	public struct ZSpawnRange
	{
		public float minZ;
		public float maxZ;
	}

	public GameObject Prefab;
	public int poolSize;
	public float shiftSpeed;
	public float spawnRate;

	public XSpawnRange xSpawnRange;
	public YSpawnRange ySpawnRange;
	public ZSpawnRange zSpawnRange;
	public Vector3 defaultSpawnPos;
	public bool spawnImmediate;


	public Vector3 immediateSpawnPos;

	float spawnTimer;
	PoolObject[] poolObjects;
	GameManager p_game;

	void Awake()
	{
		Configure();
	}

	void Start()
	{
		p_game = GameManager.instance;
		
	}


	void Update()
	{
			spawnTimer += Time.deltaTime;
		if (spawnTimer > spawnRate)
		{		
			Spawn();
			spawnTimer = 0;
		}
	}

	void Configure()
	{
		//spawning pool objects
		poolObjects = new PoolObject[poolSize];
		for (int i = 0; i < poolObjects.Length; i++)
		{
			GameObject go = Instantiate(Prefab) as GameObject;
			Transform t = go.transform;
			t.SetParent(transform);
			t.position = Vector3.one * 1000;
			poolObjects[i] = new PoolObject(t);
		}

	
	}

	void Spawn()
	{
		//moving pool objects into place
		Transform t = GetPoolObject();
		if (t == null) return;
		Vector3 pos = Vector3.zero;
		pos.y = Random.Range(ySpawnRange.minY, ySpawnRange.maxY);
		pos.x = Random.Range(xSpawnRange.minX, xSpawnRange.maxX);
		pos.z = Random.Range(zSpawnRange.minZ, zSpawnRange.maxZ);
		t.position = pos;
	}


	Transform GetPoolObject()
	{
		//retrieving first available pool object
		for (int i = 0; i < poolObjects.Length; i++)
		{
			if (!poolObjects[i].inUse)
			{
				poolObjects[i].Use();
				return poolObjects[i].transform;
			}
		}
		return null;
	}

}


