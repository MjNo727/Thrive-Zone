using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;
    public Weapon[] weapons;
    public WeaponBtn[] weaponBtns;
    public Image[] WeaponSlots;
    public Weapon activateWeapon;
    public Sprite defaultFrame, activateFrame;

    public int totalPoints = 6;
    public int remainingPoints;
    public TextMeshProUGUI pointsText;

    public bool isReset;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        remainingPoints = totalPoints; // TODO: change back to remainingPoints = level
        DisplayUpgradePoints();
        UpdateWeaponImage();
    }

    private void DisplayUpgradePoints()
    {
        pointsText.text = remainingPoints + "/" + totalPoints;
    }

    private void UpdateWeaponImage()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].isUpgraded)
            {
                weapons[i].GetComponent<Image>().color = new Vector4(1, 1, 1, 1);
                weapons[i].transform.GetChild(0).GetComponent<Image>().sprite = activateFrame;
            }
            else
            {
                weapons[i].GetComponent<Image>().color = new Vector4(0.39f, 0.39f, 0.39f, 1);
                weapons[i].transform.GetChild(0).GetComponent<Image>().sprite = defaultFrame;
            }
        }
    }

    public void UpgradeButton()
    {
        if (activateWeapon == null)
        {
            AudioManager.instance.PlaySFX("UpgradeFail");
            return;
        }

        if (!activateWeapon.isUpgraded && remainingPoints >= 1)
        {
            for (int i = 0; i < activateWeapon.previousWeapons.Length; i++)
            {
                if (activateWeapon.previousWeapons[i].isUpgraded)
                {
                    AudioManager.instance.PlaySFX("Upgrade");
                    activateWeapon.isUpgraded = true;
                    char c = activateWeapon.name[6];
                    if(c.Equals('1')) WeaponSlots[0].sprite = activateWeapon.wSprite;
                    else WeaponSlots[1].sprite = activateWeapon.wSprite;
                    remainingPoints -= 1;
                }
                else
                {
                    AudioManager.instance.PlaySFX("UpgradeFail");
                }
            }
        }
        else if (remainingPoints <= 0)
        {
            AudioManager.instance.PlaySFX("UpgradeFail");
        }

        UpdateWeaponImage();
        DisplayUpgradePoints();
    }

    public void ResetButton()
    {
        isReset = true;
        activateWeapon = null;
        remainingPoints = totalPoints;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].name.Substring(weapons[i].name.IndexOf("_") + 1).TrimStart() != "0")
            {
                weapons[i].isUpgraded = false;
            }
        }

        DisplayUpgradePoints();
        UpdateWeaponImage();
    }
}
