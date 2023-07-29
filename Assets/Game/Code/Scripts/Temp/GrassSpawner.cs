using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassSpawner : MonoBehaviour {

    [SerializeField]
    GameObject _grassBlade;

    [SerializeField]
    int _spawnCount = 1;
    [SerializeField]
    int _radius = 5;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _spawnCount; i++) {

            Vector3 pos = Random.insideUnitCircle * _radius;
            pos.z = pos.y;
            pos.y = 0;

            Vector3 foward = new Vector3();

            foward.x = Random.Range(0f, 1f);
            foward.z = Random.Range(0f, 1f);

            Instantiate(_grassBlade, pos, Quaternion.LookRotation(foward));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
