using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponBtn : MonoBehaviour
{
    public Image weaponImage;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI weaponDesc;
    public int weaponBtnId;
    public Sprite defaultWeaponSprite;

    private void Start(){
        if(WeaponManager.instance.activateWeapon == null){
            weaponImage.sprite = defaultWeaponSprite;
            weaponName.text = "Description";
            weaponDesc.text = "No description";
        }
    }

    public void PressWeaponButton(){
        WeaponManager.instance.activateWeapon = transform.GetComponent<Weapon>();
        weaponImage.sprite = WeaponManager.instance.weapons[weaponBtnId].wSprite;
        weaponName.text = WeaponManager.instance.weapons[weaponBtnId].wName;
        weaponDesc.text = WeaponManager.instance.weapons[weaponBtnId].wDescription;
    }
}
