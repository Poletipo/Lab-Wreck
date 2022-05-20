using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilSpawner : MonoBehaviour {
    private static Dictionary<GameObject, List<GameObject>> StencilList;


    private void Start()
    {
        StencilList = new Dictionary<GameObject, List<GameObject>>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public static void SpawnStencil(GameObject spawnObject, float spawnDistance, Vector3 position, Quaternion rotation)
    {
        bool validSpawn = true;

        if (StencilList.ContainsKey(spawnObject)) {

            float sqrSpawnDistance = spawnDistance * spawnDistance;

            for (int i = 1; i < StencilList[spawnObject].Count; i++) {
                Vector3 distanceVector = position - StencilList[spawnObject][i].transform.position;

                float distance = Vector3.SqrMagnitude(distanceVector);

                if (distance < sqrSpawnDistance) {
                    validSpawn = false;
                    break;
                }
            }
        }
        else {
            StencilList.Add(spawnObject, new List<GameObject>());
            StencilList[spawnObject].Add(new GameObject(spawnObject.name + "_Stencil"));
        }

        if (validSpawn) {

            GameObject tmp = Instantiate(spawnObject, position, rotation);
            tmp.transform.parent = StencilList[spawnObject][0].transform;
            StencilList[spawnObject].Add(tmp);
        }
    }



}
