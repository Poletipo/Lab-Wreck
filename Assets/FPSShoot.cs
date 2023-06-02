using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSShoot : MonoBehaviour {

    public Action OnShoot;

    [SerializeField] Animator _animator;
    [SerializeField] Transform aimTransform;
    [SerializeField] GameObject _hitVFX;
    [SerializeField] ParticleSystem _muzzleFlash;
    [SerializeField] LayerMask mask;
    [SerializeField] float _force = 5;
    [SerializeField] float _distance = 50;
    [SerializeField] int _damage = 1;

    public void Shoot()
    {

        OnShoot?.Invoke();

        _animator.Play("Jim_Gun_Fire", 0, 0f);
        _muzzleFlash.Play();

        Ray ray = new Ray(aimTransform.position, aimTransform.forward);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, _distance, mask, QueryTriggerInteraction.Collide)) {
            return;
        }

        Damageable damageable = hit.collider.GetComponent<Damageable>();
        if (damageable != null) {

            Damageable.HitData data = new Damageable.HitData {
                position = hit.point,
                direction = aimTransform.forward,
                force = _force,
                damage = _damage
            };

            damageable.Hit(data);
        }
        Instantiate(_hitVFX, hit.point, Quaternion.LookRotation(hit.normal, Vector3.up));
    }


}
