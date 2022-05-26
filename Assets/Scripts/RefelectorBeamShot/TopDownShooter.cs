using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TopDownShooter : MonoBehaviour {

    public float Speed = 5f;

    public float healthInterval = 1;
    public float healthIntervalTimer = 1;

    private bool isController = false;

    private bool _isDead;

    public bool IsDead {
        get { return _isDead; }
        set {
            _isDead = value;
            if (value) {
                _firearm.DeactivatePrincipalAction();
            }
        }
    }

    public GameObject AimFocus;
    public GameObject MoveFocus;
    public bool isMobile = true;

    public Transform Wheels;

    public Laser CanonLaser;
    public MobileJoystick joystick;
    public MobileJoystick AimJoystick;

    private Vector3 _controllerDir;
    private Vector3 _moveDirection;

    private Plane _plane = new Plane(Vector3.up, 0);

    private Rigidbody rb;
    private Health health;

    private Firearm _firearm;


    public Action OnMoneyChanged;

    [Header("Money")]
    [SerializeField]
    private int _moneyAmount = 100;

    public int MoneyAmount {
        get { return _moneyAmount; }
        set {
            _moneyAmount = Mathf.Clamp(value, 0, 1000000);
            OnMoneyChanged?.Invoke();
        }
    }



    // TEST 
    public CameraShake _cameraShake;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        _firearm = GetComponent<Firearm>();
        health.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        IsDead = true;
    }

    void Update()
    {
        if (!IsDead) {
            Move();
            Look();
            Shoot();
            CheckLaser();


            healthIntervalTimer += Time.deltaTime;

            if (healthIntervalTimer > healthInterval) {
                health.Heal(1);
                healthIntervalTimer = 0;
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.R)) {
                SceneManager.LoadScene(0);
            }
        }

    }

    private void CheckLaser()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            if (CanonLaser.LaserOn) {
                CanonLaser.TurnOff();
            }
            else {
                CanonLaser.TurnOn();
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            _cameraShake.AddTrauma(0.5f);
        }
        if (Input.GetKey(KeyCode.O)) {
            _cameraShake.AddTrauma(1f);
        }

    }

    private void Shoot()
    {
        if (isMobile) {

            if (AimJoystick.JoystickInUse) {
                _firearm.ActivatePrincipalAction();
            }
            else {
                _firearm.DeactivatePrincipalAction();
            }
        }
        else {

            if (Input.GetButtonDown("Fire1")) {
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    _firearm.ActivatePrincipalAction();
                }
            }
            if (Input.GetButtonUp("Fire1")) {
                _firearm.DeactivatePrincipalAction();
            }
        }
    }

    private void Move()
    {

        if (SystemInfo.deviceType == DeviceType.Handheld || isMobile) {
            Vector3 dir = joystick.GetDirection();

            _moveDirection.x = dir.x;
            _moveDirection.z = dir.y;
        }
        else {

            _moveDirection.x = Input.GetAxisRaw("Horizontal");
            _moveDirection.z = Input.GetAxisRaw("Vertical");
            _moveDirection.Normalize();
        }


        rb.velocity = _moveDirection * Speed;

        if (_moveDirection.magnitude > 0) {
            Wheels.rotation = Quaternion.Lerp(Wheels.rotation, Quaternion.LookRotation(_moveDirection, Vector3.up), 10 * Time.deltaTime);
        }

        MoveFocus.transform.position = Vector3.Lerp(MoveFocus.transform.position, (transform.position + _moveDirection * 5f), 8f * Time.deltaTime);
    }

    private void Look()
    {
        if (isMobile) {

            Vector3 mousePosition = AimJoystick.GetDirection();


            if (mousePosition.magnitude > 0) {

                float lookAtAngle = Mathf.Atan2(mousePosition.y, -mousePosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.up);

            }
            AimFocus.transform.position = transform.position + new Vector3(mousePosition.x, 0, mousePosition.y).normalized;


        }
        else {

            _controllerDir.x = -Input.GetAxisRaw("RightStickX");
            _controllerDir.y = Input.GetAxisRaw("RightStickY");

            if (_controllerDir.magnitude > 0) {
                isController = true;
            }
            else if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse Y") > 0) {
                isController = false;
            }

            AimFocus.transform.position = transform.position + _controllerDir.normalized * _controllerDir.magnitude;

            if (isController && _controllerDir.magnitude > 0) {
                Vector3 mousePosition = _controllerDir;


                float lookAtAngle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.up);
            }
            else if (!isController) {
                Vector3 worldMousePosition;
                float distance;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (_plane.Raycast(ray, out distance)) {
                    worldMousePosition = ray.GetPoint(distance);

                    Vector3 mousePosition = worldMousePosition - transform.position;

                    AimFocus.transform.position = transform.position +
                        new Vector3(mousePosition.x, 0, mousePosition.z).normalized * (Mathf.Clamp(Vector3.Distance(transform.position, worldMousePosition), 0, 5) / 5) * 3f;

                    float lookAtAngle = Mathf.Atan2(mousePosition.z, mousePosition.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.down);
                }
            }
        }
    }

    public bool Pay(int cost)
    {
        if (cost <= MoneyAmount) {
            MoneyAmount -= cost;
            return true;
        }
        return false;
    }


}
