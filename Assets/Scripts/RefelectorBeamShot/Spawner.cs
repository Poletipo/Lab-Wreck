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

    public Transform Player;
    public GameObject SpawnedObject;
    public float IntervalSpawnTime = 5;
    public Camera cam;
    public int ValidSpawnPointCount = 3;
    Bounds spawnBounds;
    NavMeshAgent navMeshAgent;


    // Start is called before the first frame update
    void Start()
    {
        spawnBounds = GetComponent<BoxCollider>().bounds;
        navMeshAgent = GetComponent<NavMeshAgent>();
        StartAutomaticSpawn();
    }

    private async void StartAutomaticSpawn()
    {

        possibleSpawningPoints = new List<SpawnPointValue>();

        while (Application.isPlaying) {

            possibleSpawningPoints.Clear();

            for (int i = 0; i < transform.childCount; i++) {

                Transform child = transform.GetChild(i);

                if (IsOutOfScreen(child)) {

                    float distance = Vector3.Distance(child.position, Player.position);

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

            Instantiate(SpawnedObject, spawnPosition, Quaternion.identity);
            await Task.Delay((int)(IntervalSpawnTime * 1000));
        }

    }

    // Update is called once per frame
    void Update()
    {

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
            Gizmos.color = Color.grey;
            Gizmos.DrawSphere(transform.GetChild(i).transform.position, .5f);
        }
        if (possibleSpawningPoints != null) {

            for (int i = 0; i < possibleSpawningPoints.Count; i++) {
                Gizmos.color = Color.green;
                //Debug.Log(possibleSpawningPoints[i].spawnPoint.name);
                Gizmos.DrawSphere(possibleSpawningPoints[i].spawnPoint.position, .7f);

            }

        }

    }



}
