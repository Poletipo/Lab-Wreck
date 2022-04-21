using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject SpawnedObject;
    public float IntervalSpawnTime = 5;
    Bounds spawnBounds;


    // Start is called before the first frame update
    void Start()
    {
        spawnBounds = GetComponent<BoxCollider>().bounds;
        StartAutomaticSpawn();
    }

    private async void StartAutomaticSpawn()
    {
        // TODO: CHECK IF IN GAME

        while (Application.isPlaying) {

            Vector3 spawnPosition;

            if (transform.childCount > 0) {

                int index = Random.Range(0, transform.childCount);

                spawnPosition = transform.GetChild(index).transform.position;
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

            Gizmos.DrawSphere(transform.GetChild(i).transform.position, .5f);

        }
    }



}
