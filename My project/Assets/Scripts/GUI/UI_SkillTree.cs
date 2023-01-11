using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utils;

public class UI_SkillTree : MonoBehaviour
{
    [SerializeField]
    private Material skillLockedMaterial;
    private PlayerWeapons playerWeapons;
    private List<WeaponBtnS1> weaponBtnList1;
    public PlayerInputActions playerInput;
    public InputAction openSkillTree;
    private CanvasGroup canvasGroup;
    public static bool isOpened = false;
    private void OnEnable()
    {
        openSkillTree = playerInput.UI.WeaponTree;
        openSkillTree.Enable();
        openSkillTree.performed += WeaponTree;
    }
    void OnDisable()
    {
        openSkillTree.Disable();
    }
    private void Awake()
    {
        playerInput = new PlayerInputActions();
        canvasGroup = GetComponent<CanvasGroup>();

        CloseWeaponTreeMenu();

        Tooltip.AddTooltip(transform.Find("Weapon1_0"), "Sword (Base Attack = 5)");
        Tooltip.AddTooltip(transform.Find("Weapon1_1"), "Hammer (Base Attack = 10)");
        Tooltip.AddTooltip(transform.Find("Weapon1_2"), "Scythe (Base Attack = 20)");
        Tooltip.AddTooltip(transform.Find("Weapon2_0"), "Bow (Base Attack = 5)");
        Tooltip.AddTooltip(transform.Find("Weapon2_1"), "Gun (Base Attack = 10)");
        Tooltip.AddTooltip(transform.Find("Weapon2_2"), "Rifle (Base Attack = 20)");

        transform.Find("Weapon1_0").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Tooltip_Warning.ShowTooltip_Static("Already obtained!");
            AudioManager.instance.PlaySFX("UpgradeFail");
        };
        transform.Find("Weapon1_1").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerWeapons.TryUnlockWeaponMelee(PlayerWeapons.WeaponType.Hammer))
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot upgrade!");
                AudioManager.instance.PlaySFX("UpgradeFail");
            }
        };
        transform.Find("Weapon1_2").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerWeapons.TryUnlockWeaponMelee(PlayerWeapons.WeaponType.Scythe))
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot upgrade!");
                AudioManager.instance.PlaySFX("UpgradeFail");
            }
        };
        transform.Find("Weapon2_0").GetComponent<Button_UI>().ClickFunc = () =>
        {
            Tooltip_Warning.ShowTooltip_Static("Already obtained!");
            AudioManager.instance.PlaySFX("UpgradeFail");
        };
        transform.Find("Weapon2_1").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerWeapons.TryUnlockWeaponRange(PlayerWeapons.WeaponType.Gun))
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot upgrade!");
                AudioManager.instance.PlaySFX("UpgradeFail");
            }
        };
        transform.Find("Weapon2_2").GetComponent<Button_UI>().ClickFunc = () =>
        {
            if (!playerWeapons.TryUnlockWeaponRange(PlayerWeapons.WeaponType.Rifle))
            {
                Tooltip_Warning.ShowTooltip_Static("Cannot upgrade!");
                AudioManager.instance.PlaySFX("UpgradeFail");
            }
        };
    }
    public void SetPlayerWeapons(PlayerWeapons playerWeapons)
    {
        this.playerWeapons = playerWeapons;
        weaponBtnList1 = new List<WeaponBtnS1>();
        weaponBtnList1.Add(new WeaponBtnS1(transform.Find("Weapon1_1"), playerWeapons, PlayerWeapons.WeaponType.Hammer, skillLockedMaterial));
        weaponBtnList1.Add(new WeaponBtnS1(transform.Find("Weapon1_2"), playerWeapons, PlayerWeapons.WeaponType.Scythe, skillLockedMaterial));
        playerWeapons.OnWeaponUnlocked += PlayerWeapons_OnWeaponUnlocked;
        UpdateVisuals();
    }

    private void PlayerWeapons_OnWeaponUnlocked(object sender, PlayerWeapons.OnWeaponUnlockedEventArgs e)
    {
        UpdateVisuals();
    }

    private void WeaponTree(InputAction.CallbackContext context)
    {
        isOpened = !isOpened;
        if (isOpened)
        {
            OpenWeaponTreeMenu();
        }
        else
        {
            CloseWeaponTreeMenu();
        }
    }

    void OpenWeaponTreeMenu()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
    }

    public void CloseWeaponTreeMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        isOpened = false;
    }

    private void UpdateVisuals()
    {
        foreach (WeaponBtnS1 weaponBtnS1 in weaponBtnList1)
        {
            weaponBtnS1.UpdateVisual();
        }
    }

    private class WeaponBtnS1
    {
        private Transform transform;
        private Image image;
        private Image backgroundImage;
        private PlayerWeapons playerWeapons;
        private PlayerWeapons.WeaponType weaponType;
        private Material skillLockedMaterial;

        public WeaponBtnS1(Transform transform, PlayerWeapons playerWeapons, PlayerWeapons.WeaponType weaponType, Material skillLockedMaterial)
        {
            this.transform = transform;
            this.playerWeapons = playerWeapons;
            this.weaponType = weaponType;
            this.skillLockedMaterial = skillLockedMaterial;
            image = transform.Find("Image").GetComponent<Image>();
            backgroundImage = transform.Find("Background").GetComponent<Image>();
            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerWeapons.IsWeaponUnlocked(weaponType))
                {
                    // Skill not yet unlocked
                    if (!playerWeapons.TryUnlockWeaponMelee(weaponType))
                    {
                        Tooltip_Warning.ShowTooltip_Static("Cannot unlock " + weaponType + "!");
                    }
                }
            };
        }

        public void UpdateVisual()
        {
            if (playerWeapons.IsWeaponUnlocked(weaponType))
            {
                image.material = null;
                //success color
                backgroundImage.color = new Color(0.088f, 0.264f, 0.092f, 1f);
                AudioManager.instance.PlaySFX("Upgrade");
            }
            else
            {
                if (playerWeapons.CanUnlockMelee(weaponType))
                {
                    backgroundImage.color = new Color(0.047f, 0.149f, 0.145f, 1f);
                }
                else
                {
                    image.material = skillLockedMaterial;
                    backgroundImage.color = Color.black;
                }
            }
        }
    }

}
