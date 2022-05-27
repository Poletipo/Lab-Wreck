using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour {

    struct SpawnPointValue {
        public Transform spawnPoint;
        public float distance;
    }

    List<SpawnPointValue> possibleSpawningPoints;

    [Header("Spawner Parameters")]
    private Transform Player;
    public GameObject SpawnedObject;
    public float IntervalSpawnTime = 5;
    private float _intervalSpawnTimer = 0;
    public Camera cam;
    public int ValidSpawnPointCount = 3;
    public bool isSpawning = true;

    [Header("Cats Parameters")]
    public AnimationCurve SpawnRateCurve;
    public int HealthStart = 15;
    public int HealthTimeMuliplier = 60;
    public int HealthAddition = 1;
    public int EstimateTimeGame = 600;
    private float _currentGameTime = 0;

    Bounds spawnBounds;

    private NavMeshPath path;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameManager.Instance.Player.transform;
        Player.GetComponent<Health>().OnDeath += OnPlayerDeath;
        spawnBounds = GetComponent<BoxCollider>().bounds;
        path = new NavMeshPath();
        possibleSpawningPoints = new List<SpawnPointValue>();
    }

    private void OnPlayerDeath()
    {
        isSpawning = false;
    }

    private void SpawnObject()
    {

        possibleSpawningPoints.Clear();

        for (int i = 0; i < transform.childCount; i++) {

            Transform child = transform.GetChild(i);

            if (IsOutOfScreen(child)) {

                NavMesh.CalculatePath(child.position, Player.position, NavMesh.AllAreas, path);

                float distance = 0.0f;
                for (int j = 0; j < path.corners.Length - 1; ++j) {
                    distance += Vector3.Distance(path.corners[j], path.corners[j + 1]);
                }

                int index = 0;
                bool isSmaller = false;

                for (int j = 0; j < possibleSpawningPoints.Count; j++) {

                    index = j;
                    if (distance < possibleSpawningPoints[j].distance) {
                        isSmaller = true;
                        break;
                    }
                }

                SpawnPointValue spawnPointValue = new SpawnPointValue();
                spawnPointValue.spawnPoint = child;
                spawnPointValue.distance = distance;

                if (isSmaller) {
                    possibleSpawningPoints.Insert(index, spawnPointValue);
                }
                else {
                    possibleSpawningPoints.Add(spawnPointValue);
                }

                if (possibleSpawningPoints.Count > ValidSpawnPointCount)
                    possibleSpawningPoints.RemoveAt(ValidSpawnPointCount);

            }
        }

        Vector3 spawnPosition;

        if (possibleSpawningPoints.Count > 0) {

            int index = Random.Range(0, possibleSpawningPoints.Count);

            spawnPosition = possibleSpawningPoints[index].spawnPoint.position;
        }
        else {
            spawnPosition = RandomPointInBounds(spawnBounds);
        }

        GameObject cat = PoolManager.GetPoolObject(SpawnedObject);
        int catHp = HealthStart + Mathf.FloorToInt(_currentGameTime / HealthTimeMuliplier) * HealthAddition;
        cat.GetComponent<ZombieEnemy>().Setup(spawnPosition, Quaternion.identity, catHp);

    }

    // Update is called once per frame
    void Update()
    {
        _currentGameTime += Time.deltaTime;
        if (isSpawning) {
            GameManager.Instance.GameUI.UpdateTimer(_currentGameTime);
        }

        IntervalSpawnTime = SpawnRateCurve.Evaluate(_currentGameTime / EstimateTimeGame);

        _intervalSpawnTimer += Time.deltaTime;
        if (_intervalSpawnTimer >= IntervalSpawnTime) {

            if (isSpawning) {
                SpawnObject();
                _intervalSpawnTimer = 0;
            }

        }


    }

    public bool IsOutOfScreen(Transform target)
    {
        bool isOutOfScreen = false;
        Vector3 viewPos = cam.WorldToViewportPoint(target.position);


        if (viewPos.x < 0 || viewPos.x > 1 ||
            viewPos.y < 0 || viewPos.y > 1) {
            isOutOfScreen = true;
        }

        return isOutOfScreen;
    }


    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    private void OnDrawGizmos()
    {
        int spawnPointCount = transform.childCount;
        for (int i = 0; i < spawnPointCount; i++) {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.GetChild(i).transform.position, .5f);
        }
        if (possibleSpawningPoints != null) {

            for (int i = 0; i < possibleSpawningPoints.Count; i++) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(possibleSpawningPoints[i].spawnPoint.position, .7f);

            }

        }

    }



}
