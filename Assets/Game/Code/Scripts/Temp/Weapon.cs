using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    public enum WeaponType {
        Firearm,
        Melee,
        Laser
    }

    public enum WeaponLenght {
        Short,
        Long
    }

    public string weaponName;
    public WeaponType type;
    public WeaponLenght lenght;


    public abstract void ActivatePrincipalAction();
    public abstract void StopPrincipalAction();
    public abstract void ActivateSecondaryAction();
    public abstract void StopSecondaryAction();
}
