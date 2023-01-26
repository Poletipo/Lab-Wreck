using UnityEngine;

public class Bullet : MonoBehaviour {
    public float speed = 10f;
    public float damage = 25f;
    int dir = 1;

    public Transform bulletHitVFX;


    // Start is called before the first frame update
    void Start()
    {
        dir = GetComponent<FacingController>().GetIntDirection();
        speed += Random.Range(-1.0f, 1.0f);
        Destroy(gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * dir * speed * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            Instantiate(bulletHitVFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }


}
