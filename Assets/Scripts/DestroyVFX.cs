using UnityEngine;

public class DestroyVFX : MonoBehaviour {

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
