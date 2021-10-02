using UnityEngine;

public class DesertRider : MonoBehaviour {
    public Transform[] wheels;

    public Transform steering;

    public Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxisRaw("Vertical") > 0) {

            for (int i = 0; i < wheels.Length; i++) {
                rb.AddForceAtPosition(transform.forward, wheels[i].position, ForceMode.Force);
            }

        }

        if (Input.GetAxisRaw("Horizontal") != 0) {

            Vector3 direction = transform.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0));

            rb.AddForceAtPosition(-direction * 0.2f, steering.position, ForceMode.Force);

        }


    }
}
