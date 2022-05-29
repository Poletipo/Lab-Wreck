using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCount : MonoBehaviour {
    public float RefreshTime = 1;
    private float _refreshTimer = 0;

    public TextMeshProUGUI CatCount;

    // Update is called once per frame
    void Update()
    {
        _refreshTimer += Time.deltaTime;

        if (_refreshTimer >= RefreshTime) {
            CatCount.text = GameObject.FindGameObjectsWithTag("Enemy").Length.ToString("000");
            _refreshTimer = 0;
        }

    }
}
