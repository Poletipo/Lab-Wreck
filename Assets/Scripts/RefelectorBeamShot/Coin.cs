using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int CoinValue = 50;
    public Rigidbody rb;
    public int TimeAlive = 60;
    private float _timeAliveTimer = 0;
    public AudioClip CoinShuffle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _timeAliveTimer += Time.deltaTime;
        if (_timeAliveTimer >= TimeAlive) {
            DestroyCoin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {

            other.GetComponent<TopDownShooter>().MoneyAmount += CoinValue;
            GameManager.Instance.AudioManager.PlayOneShot(CoinShuffle);

            DestroyCoin();
        }
    }

    public void Setup(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;

        rb.velocity = Random.onUnitSphere * 3;
        rb.angularVelocity = Random.onUnitSphere * 10;
        _timeAliveTimer = 0;

        gameObject.SetActive(true);
    }

    void DestroyCoin()
    {
        gameObject.SetActive(false);
    }



}
