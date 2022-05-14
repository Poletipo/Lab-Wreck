using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour {

    public LayerMask wallMask;
    public float MaxDistance = 10f;

    private RaycastHit hitInfo;
    private LineRenderer lr;
    private Ray ray;

    public bool LaserOn { get; private set; } = true;

    public void TurnOn()
    {
        lr.enabled = true;
        LaserOn = true;
    }

    public void TurnOff()
    {
        lr.enabled = false;
        LaserOn = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (LaserOn) {
            ray.origin = transform.position;
            ray.direction = transform.forward;

            if (Physics.Raycast(ray, out hitInfo, MaxDistance, wallMask, QueryTriggerInteraction.Collide)) {
                UpdateLaser(transform.position, hitInfo.point);
            }
            else {
                UpdateLaser(transform.position, transform.position + transform.forward * MaxDistance);
            }
        }
    }


    private void UpdateLaser(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

}
