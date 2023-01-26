using System.Collections;
using UnityEngine;

public class Firearm : Weapon {

    public enum FireRateModes {
        Single = 0,
        Burst = 1,
        Auto = 2
    }

    public FireRateModes fireMode;

    public GameObject bullet;
    public Transform muzzlePos;

    public int nbBulletPerShot = 1;
    [Range(0, 90)]
    public float maxAngleOffset = 10;

    public float fireRateSpeed = 0.5f;

    private float fireRateTimer = 0;

    bool isPrincipalActivated = false;
    bool isSecondaryActivated = false;

    public int nbBurst = 1;
    int burstCount = 0;

    bool canShoot = true;
    bool isBurstFiring = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer -= Time.deltaTime;

        if (isPrincipalActivated && canShoot && fireRateTimer <= 0) {

            switch (fireMode) {
                case FireRateModes.Single:
                    ShootSingle();
                    canShoot = false;
                    break;
                case FireRateModes.Burst:
                    StartCoroutine(ShootBurst());
                    canShoot = false;
                    break;
                case FireRateModes.Auto:
                    ShootSingle();
                    break;
            }
        }
    }

    IEnumerator ShootBurst()
    {
        while (burstCount < nbBurst) {
            ShootSingle();
            yield return new WaitForSeconds(fireRateTimer);

            burstCount++;
        }

        burstCount = 0;
    }

    public void ShootSingle()
    {
        for (int i = 0; i < nbBulletPerShot; i++) {
            float angleOffset = Random.Range(-maxAngleOffset, maxAngleOffset);
            Quaternion bulletAngle = muzzlePos.rotation * Quaternion.Euler(0, 0, angleOffset);
            GameObject projectile = Instantiate(bullet, muzzlePos.position, bulletAngle);
            projectile.GetComponent<FacingController>().FacingSide = GetComponent<FacingController>().FacingSide;
        }

        fireRateTimer = fireRateSpeed;
    }

    // OVERWRITE
    public override void ActivatePrincipalAction()
    {
        isPrincipalActivated = true;
    }

    public override void StopPrincipalAction()
    {
        isPrincipalActivated = false;
        canShoot = true;
    }

    public override void ActivateSecondaryAction()
    {
    }

    public override void StopSecondaryAction()
    {
    }
}
