using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {

    private static Dictionary<GameObject, List<GameObject>> PooledList;


    private void Start()
    {
        PooledList = new Dictionary<GameObject, List<GameObject>>();
    }

    public static GameObject GetPoolObject(GameObject spawneObject)
    {
        if (!PooledList.ContainsKey(spawneObject)) {
            PooledList.Add(spawneObject, new List<GameObject>());
            GameObject parent = new GameObject();

            parent.name = spawneObject.name + "_Pooler";

            PooledList[spawneObject].Add(parent);
        }

        List<GameObject> pooledObjects = PooledList[spawneObject];
        for (int i = 0; i < pooledObjects.Count; i++) {

            if (!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }

        GameObject tmp = Instantiate(spawneObject, pooledObjects[0].transform);
        pooledObjects.Add(tmp);

        return tmp;
    }


}
