using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class Firearm : MonoBehaviour {

    public enum FireRateModes {
        Single = 0,
        Burst = 1,
        Auto = 2
    }

    public FireRateModes fireMode;

    public GameObject bullet;
    public Transform muzzlePos;
    [Range(1, 1000)]
    public int nbBulletPerShot = 1;
    [Range(0, 90)]
    public float maxAngleOffset = 10;
    [Range(0, 5)]
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
        if (Input.GetButtonDown("Fire1")) {
            ActivatePrincipalAction();
        }

        if (Input.GetButtonUp("Fire1")) {
            DeactivatePrincipalAction();
        }

        fireRateTimer -= Time.deltaTime;

        if (isPrincipalActivated && canShoot && fireRateTimer <= 0) {

            switch (fireMode) {
                case FireRateModes.Single:
                    ShootSingle();
                    canShoot = false;
                    break;
                case FireRateModes.Burst:
                    if (!isBurstFiring) {
                        ShootBurst();
                        canShoot = false;
                    }
                    break;
                case FireRateModes.Auto:
                    ShootSingle();
                    break;
            }
        }
    }

    public async void ShootBurst()
    {
        isBurstFiring = true;
        while (burstCount < nbBurst) {
            ShootSingle();
            await Task.Delay((int)(fireRateTimer * 1000));
            burstCount++;
        }
        burstCount = 0;
        isBurstFiring = false;
    }

    public void ShootSingle()
    {
        for (int i = 0; i < nbBulletPerShot; i++) {
            float angleOffset = Random.Range(-maxAngleOffset, maxAngleOffset);
            Quaternion bulletAngle = muzzlePos.rotation * Quaternion.Euler(0, angleOffset, 0);
            GameObject projectile = Instantiate(bullet, muzzlePos.position, Quaternion.identity);



            Vector3 bulletDirection = bulletAngle * Vector3.forward;

            projectile.GetComponent<ReflectorBullet>().BulletSetup(bulletDirection);
        }

        fireRateTimer = fireRateSpeed;
    }

    // OVERWRITE
    public void ActivatePrincipalAction()
    {
        isPrincipalActivated = true;
    }

    public void DeactivatePrincipalAction()
    {
        isPrincipalActivated = false;
        canShoot = true;
    }
}
