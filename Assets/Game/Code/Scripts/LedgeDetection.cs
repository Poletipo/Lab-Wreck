using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetection : MonoBehaviour {

    public struct LedgeInfo {
        public Vector3 wallHitPoint;
        public Vector3 groundHitPoint;
        public Vector3 ledgePosition;
    }

    [SerializeField] float wallDistance = 5;
    [SerializeField] float maxWallHeight = 5;
    [SerializeField] float gizmosSize = 1;
    [SerializeField] float maxWallAngle = 45;


    private bool ledgeDetected = false;
    private bool validWallInFront = false;
    private bool groundOnTop = false;
    private RaycastHit wallHit;
    private RaycastHit groundHit;
    private Vector3 groundRayStart;
    private Vector3 ledgePosition;
    private Ray ray;
    private bool wallInFront;
    private CapsuleCollider selfCollider;


    // Start is called before the first frame update
    void Start() {
        selfCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update() {

        ledgeDetected = false;
        if (CheckValidWall()) {
            if (CheckValidGround()) {

                if (CheckValidPosition()) {
                    ledgeDetected = true;
                    ledgePosition = wallHit.point;
                    ledgePosition.y = groundHit.point.y;
                }
            }
        }

    }

    public static bool LedgeTest(Vector3 startPosition, Vector3 direction, out LedgeInfo info, float maxDistance = 1) {

        info = new LedgeInfo {
            groundHitPoint = Vector3.zero,
            ledgePosition = Vector3.zero,
            wallHitPoint = Vector3.zero
        };


        return true;
    }

    private bool CheckValidPosition() {
        return true;
    }

    private bool CheckValidGround() {

        bool validGround = false;

        groundRayStart = transform.position + (transform.forward * (wallHit.distance + selfCollider.radius)) + Vector3.up * maxWallHeight;
        ray = new Ray(groundRayStart, Vector3.down * maxWallHeight);
        groundOnTop = Physics.Raycast(ray, out groundHit, maxWallHeight);


        if (groundOnTop && groundHit.point.y >= wallHit.point.y) {
            validGround = true;
        }
        float rayLength = validGround ? groundHit.distance : maxWallHeight;
        Debug.DrawRay(groundRayStart, Vector3.down * rayLength, validGround ? Color.green : Color.gray);

        return validGround;
    }

    private bool CheckValidWall() {
        validWallInFront = false;

        Vector3 rayOrigin = transform.position + selfCollider.center;

        ray = new Ray(rayOrigin, transform.forward);
        wallInFront = Physics.Raycast(ray, out wallHit, wallDistance);

        if (wallInFront) {

            float dotTest = Mathf.Cos(maxWallAngle * Mathf.Deg2Rad);

            float dotValue = Vector3.Dot(wallHit.normal, -transform.forward);

            if (dotValue >= dotTest) {
                validWallInFront = true;
            }
        }

        float rayLength = validWallInFront ? wallHit.distance : wallDistance;
        Debug.DrawRay(rayOrigin, transform.forward * rayLength, validWallInFront ? Color.green : Color.gray);

        return validWallInFront;
    }


    private void OnDrawGizmos() {

        if (Application.isPlaying) {

            if (validWallInFront) {
                Gizmos.DrawSphere(groundRayStart, gizmosSize);
            }

            if (ledgeDetected) {
                Gizmos.DrawCube(ledgePosition, Vector3.one * gizmosSize);
            }
        }

    }



}
