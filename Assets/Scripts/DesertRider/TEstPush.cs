using UnityEngine;

public class TEstPush : MonoBehaviour {


    Rigidbody rb;
    public Transform point;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            rb.AddForceAtPosition(transform.forward, point.position, ForceMode.Force);
        }
    }
}
