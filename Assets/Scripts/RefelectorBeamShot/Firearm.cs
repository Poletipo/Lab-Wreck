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
    public ParticleSystem MuzzleFlash;
    public AudioClip[] ShootSound;

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

    public int ReboundCount = 1;


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
            GameObject projectile = PoolManager.GetPoolObject(bullet);


            Vector3 bulletDirection = bulletAngle * Vector3.forward;

            StartCoroutine(projectile.GetComponent<ReflectorBullet>().Setup(muzzlePos.position, bulletDirection, ReboundCount));
            MuzzleFlash.Play();
            GameManager.Instance.CameraObject.GetComponent<CameraShake>().SetHigherTrauma(0.3f);
        }
        GameManager.Instance.AudioManager.PlayOneShot(ShootSound[UnityEngine.Random.Range(0, ShootSound.Length)], .5f);
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
