using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    int damage = 0;
    public GameObject DeadVFX;
    public float speed;
    public LayerMask hitMask;

    public void Setup(Vector3 position, Vector3 direction, int damage)
    {
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
        this.damage = damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
         if(IsInLayerMask(other.gameObject, hitMask))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.Hurt(damage);
            }

            Player player = other.GetComponent<Player>();

            if(player != null)
            {
                player.DowngradeDice();
            }



            EndProjectile();

        }
    }

    void EndProjectile()
    {
        Instantiate(DeadVFX, transform.position, Quaternion.identity);

        gameObject.SetActive(false);

        //Destroy(gameObject);
    }

    public static bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }



}
