using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerWeapons playerWeapons;
    private void Awake(){
        transform.Find("Weapon1_0").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponMelee(PlayerWeapons.WeaponType.Sword))
                Debug.Log("Cannot unlock Sword");
        };
        transform.Find("Weapon1_1").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponMelee(PlayerWeapons.WeaponType.Hammer))
                Debug.Log("Cannot unlock Hammer");
        };
        transform.Find("Weapon1_2").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponMelee(PlayerWeapons.WeaponType.Scythe))
                Debug.Log("Cannot unlock Scythe");
        };
        transform.Find("Weapon2_0").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponRange(PlayerWeapons.WeaponType.Bow))
                Debug.Log("Cannot unlock Bow");
        };
        transform.Find("Weapon2_1").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponRange(PlayerWeapons.WeaponType.Gun))
                Debug.Log("Cannot unlock Gun");
        };
        transform.Find("Weapon2_2").GetComponent<Button_UI>().ClickFunc = () => {
            if(!playerWeapons.TryUnlockWeaponRange(PlayerWeapons.WeaponType.Rifle))
                Debug.Log("Cannot unlock Rifle");
        };
    }
    public void SetPlayerWeapons(PlayerWeapons playerWeapons){
        this.playerWeapons = playerWeapons;
    }
}
