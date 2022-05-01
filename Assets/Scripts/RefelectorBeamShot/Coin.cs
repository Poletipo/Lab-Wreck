using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int CoinValue = 50;
    public Rigidbody rb;
    public int TimeAlive = 60;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = Random.onUnitSphere * 3;
        rb.angularVelocity = Random.onUnitSphere * 10;
        Destroy(gameObject.transform.parent.gameObject, TimeAlive);
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {

            other.GetComponent<TopDownShooter>().MoneyAmount += CoinValue;

            Destroy(gameObject.transform.parent.gameObject);
        }
    }


}
