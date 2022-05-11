using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    public Transform Wheels;

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

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1")) {
            _firearm.ActivatePrincipalAction();
        }

        if (Input.GetButtonUp("Fire1")) {
            _firearm.DeactivatePrincipalAction();
        }
    }

    private void Move()
    {

        _moveDirection.x = Input.GetAxisRaw("Horizontal");
        _moveDirection.z = Input.GetAxisRaw("Vertical");
        _moveDirection.Normalize();

        rb.velocity = _moveDirection * Speed;

        if (_moveDirection.magnitude > 0) {
            Wheels.rotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        }

    }

    private void Look()
    {
        _controllerDir.x = -Input.GetAxisRaw("RightStickX");
        _controllerDir.y = Input.GetAxisRaw("RightStickY");

        if (_controllerDir.magnitude > 0) {
            isController = true;
        }
        else if (Input.GetAxisRaw("Mouse X") > 0 || Input.GetAxisRaw("Mouse Y") > 0) {
            isController = false;
        }

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
                float lookAtAngle = Mathf.Atan2(mousePosition.z, mousePosition.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(lookAtAngle - 90, Vector3.down);
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
