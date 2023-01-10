using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons
{
    public event EventHandler<OnWeaponUnlockedEventArgs> OnWeaponUnlocked;
    public class OnWeaponUnlockedEventArgs : EventArgs
    {
        public WeaponType weaponType;
    }
    public enum WeaponType
    {
        Sword,
        Hammer,
        Scythe,
        Bow,
        Gun,
        Rifle
    }

    private List<WeaponType> unlockedWeaponTypeList;
    public PlayerWeapons()
    {
        unlockedWeaponTypeList = new List<WeaponType>();
    }
    private void UnlockWeapon(WeaponType weaponType)
    {
        if (!IsWeaponUnlocked(weaponType))
        {
            unlockedWeaponTypeList.Add(weaponType);
            OnWeaponUnlocked?.Invoke(this, new OnWeaponUnlockedEventArgs { weaponType = weaponType });
        }
    }

    public bool IsWeaponUnlocked(WeaponType weaponType)
    {
        return unlockedWeaponTypeList.Contains(weaponType);
    }

    public WeaponType GetWeaponRequirementMelee(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Hammer: return WeaponType.Sword;
            case WeaponType.Scythe: return WeaponType.Hammer;
        }
        return WeaponType.Sword;
    }

    public WeaponType GetWeaponRequirementRange(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Gun: return WeaponType.Bow;
            case WeaponType.Rifle: return WeaponType.Gun;
        }
        return WeaponType.Bow;
    }

    public bool TryUnlockWeaponMelee(WeaponType weaponType)
    {
        WeaponType weaponRequirement = GetWeaponRequirementMelee(weaponType);
        if (weaponRequirement != WeaponType.Sword)
        {
            if (IsWeaponUnlocked(weaponRequirement))
            {
                UnlockWeapon(weaponType);
                return true;
            }
            else return false;
        }
        else
        {
            UnlockWeapon(weaponType);
            return true;
        }
    }

    public bool TryUnlockWeaponRange(WeaponType weaponType)
    {
        WeaponType weaponRequirement = GetWeaponRequirementRange(weaponType);
        if (weaponRequirement != WeaponType.Bow)
        {
            if (IsWeaponUnlocked(weaponRequirement))
            {
                UnlockWeapon(weaponType);
                return true;
            }
            else return false;
        }
        else
        {
            UnlockWeapon(weaponType);
            return true;
        }
    }
}
