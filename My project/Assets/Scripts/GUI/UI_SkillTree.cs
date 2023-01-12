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
    [SerializeField]
    private WeaponUnlockPath[] weaponUnlockPathArray;
    [SerializeField]
    private WeaponUnlockPath2[] weaponUnlockPathArray2;
    [SerializeField] private Sprite lineSprite;
    [SerializeField] private Sprite lineSpriteGlow;

    public static UI_SkillTree instance;
    private PlayerWeapons playerWeapons;
    private List<WeaponBtnS1> weaponBtnListS1;
    private List<WeaponBtnS2> weaponBtnListS2;
    private TMPro.TextMeshProUGUI upgradePointsText;
    public Image weapon1, weapon2;
    public PlayerInputActions playerInput;
    private InputAction openSkillTree;
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
        instance = this;
        upgradePointsText = transform.Find("UpgradePoints").GetComponent<TMPro.TextMeshProUGUI>();
        playerInput = new PlayerInputActions();
        canvasGroup = GetComponent<CanvasGroup>();

        CloseWeaponTreeMenu();

        Tooltip.AddTooltip(transform.Find("Weapon1_0"), "Sword (Base Attack = 10)");
        Tooltip.AddTooltip(transform.Find("Weapon1_1"), "Hammer (Base Attack = 20)");
        Tooltip.AddTooltip(transform.Find("Weapon1_2"), "Scythe (Base Attack = 40)");
        Tooltip.AddTooltip(transform.Find("Weapon2_0"), "Bow (Base Attack = 7)");
        Tooltip.AddTooltip(transform.Find("Weapon2_1"), "Gun (Base Attack = 15)");
        Tooltip.AddTooltip(transform.Find("Weapon2_2"), "Rifle (Base Attack = 22)");
    }
    public void SetPlayerWeapons(PlayerWeapons playerWeapons)
    {
        this.playerWeapons = playerWeapons;
        weaponBtnListS1 = new List<WeaponBtnS1>();
        weaponBtnListS2 = new List<WeaponBtnS2>();
        weaponBtnListS1.Add(new WeaponBtnS1(transform.Find("Weapon1_1"), playerWeapons, PlayerWeapons.WeaponType.Hammer, skillLockedMaterial));
        weaponBtnListS1.Add(new WeaponBtnS1(transform.Find("Weapon1_2"), playerWeapons, PlayerWeapons.WeaponType.Scythe, skillLockedMaterial));
        weaponBtnListS2.Add(new WeaponBtnS2(transform.Find("Weapon2_1"), playerWeapons, PlayerWeapons.WeaponType.Gun, skillLockedMaterial));
        weaponBtnListS2.Add(new WeaponBtnS2(transform.Find("Weapon2_2"), playerWeapons, PlayerWeapons.WeaponType.Rifle, skillLockedMaterial));
        playerWeapons.OnWeaponUnlocked += PlayerWeapons_OnWeaponUnlocked;
        playerWeapons.OnUpgradePointsChanged += PlayerWeapons_OnUpgradePointsChanged;

        UpdateVisualsS1();
        UpdateVisualsS2();
        UpdateUpgradePoints();
    }

    private void PlayerWeapons_OnWeaponUnlocked(object sender, PlayerWeapons.OnWeaponUnlockedEventArgs e)
    {
        UpdateVisualsS1();
        UpdateVisualsS2();
    }

    private void PlayerWeapons_OnUpgradePointsChanged(object sender, System.EventArgs e)
    {
        UpdateUpgradePoints();
    }

    private void UpdateUpgradePoints()
    {
        upgradePointsText.SetText(playerWeapons.GetUpgradePoints().ToString());
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

    private void UpdateVisualsS1()
    {
        foreach (WeaponBtnS1 weaponBtn in weaponBtnListS1)
        {
            weaponBtn.UpdateVisualS1();
        }

        // Darken all links
        foreach (WeaponUnlockPath weaponUnlockPath in weaponUnlockPathArray)
        {
            foreach (Image linkImage in weaponUnlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5f, .5f, .5f);
                linkImage.sprite = lineSprite;
            }
        }

        foreach (WeaponUnlockPath weaponUnlockPath in weaponUnlockPathArray)
        {
            if (playerWeapons.IsWeaponUnlocked(weaponUnlockPath.weaponType) || playerWeapons.CanUnlockMelee(weaponUnlockPath.weaponType))
            {
                foreach (Image linkImage in weaponUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                    linkImage.sprite = lineSpriteGlow;
                }
            }
        }
    }

    private void UpdateVisualsS2()
    {
        foreach (WeaponBtnS2 weaponBtn in weaponBtnListS2)
        {
            weaponBtn.UpdateVisualS2();
        }

        // Darken all links
        foreach (WeaponUnlockPath2 weaponUnlockPath in weaponUnlockPathArray2)
        {
            foreach (Image linkImage in weaponUnlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5f, .5f, .5f);
                linkImage.sprite = lineSprite;
            }
        }

        foreach (WeaponUnlockPath2 weaponUnlockPath in weaponUnlockPathArray2)
        {
            if (playerWeapons.IsWeaponUnlocked(weaponUnlockPath.weaponType) || playerWeapons.CanUnlockRange(weaponUnlockPath.weaponType))
            {
                foreach (Image linkImage in weaponUnlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                    linkImage.sprite = lineSpriteGlow;
                }

            }
        }
    }

    public class WeaponBtnS1
    {
        private Transform transform;
        public Image weaponImage;
        private Image backgroundImage;
        private Image frameImage;


        private PlayerWeapons playerWeapons;
        private PlayerWeapons.WeaponType weaponType;
        private Material skillLockedMaterial;

        public WeaponBtnS1(Transform transform, PlayerWeapons playerWeapons, PlayerWeapons.WeaponType weaponType, Material skillLockedMaterial)
        {
            this.transform = transform;
            this.playerWeapons = playerWeapons;
            this.weaponType = weaponType;
            this.skillLockedMaterial = skillLockedMaterial;
            weaponImage = transform.Find("Image").GetComponent<Image>();
            backgroundImage = transform.Find("Background").GetComponent<Image>();
            frameImage = transform.Find("Frame").GetComponent<Image>();
            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerWeapons.IsWeaponUnlocked(weaponType))
                {
                    // Skill not yet unlocked
                    if (!playerWeapons.TryUnlockWeaponMelee(weaponType))
                    {
                        Tooltip_Warning.ShowTooltip_Static("Cannot unlock " + weaponType + "!");
                        AudioManager.instance.PlaySFX("UpgradeFail");
                    }
                }
            };
        }

        public void UpdateVisualS1()
        {
            if (playerWeapons.IsWeaponUnlocked(weaponType))
            {
                weaponImage.material = null;
                //success color
                backgroundImage.color = new Color(0.088f, 0.264f, 0.092f, 1f);
                frameImage.color = Color.yellow;
                AudioManager.instance.PlaySFX("Upgrade");
                UI_SkillTree.instance.weapon1.sprite = weaponImage.sprite;
            }
            else
            {

                if (playerWeapons.CanUnlockMelee(weaponType))
                {
                    weaponImage.material = skillLockedMaterial;
                    backgroundImage.color = new Color(0.047f, 0.149f, 0.145f, 1f);
                }
                else
                {
                    weaponImage.material = skillLockedMaterial;
                    backgroundImage.color = Color.black;
                }
            }
        }

    }

    public class WeaponBtnS2
    {
        private Transform transform;
        public Image weaponImage;
        private Image backgroundImage;
        private Image frameImage;

        private PlayerWeapons playerWeapons;
        private PlayerWeapons.WeaponType weaponType;
        private Material skillLockedMaterial;

        public WeaponBtnS2(Transform transform, PlayerWeapons playerWeapons, PlayerWeapons.WeaponType weaponType, Material skillLockedMaterial)
        {
            this.transform = transform;
            this.playerWeapons = playerWeapons;
            this.weaponType = weaponType;
            this.skillLockedMaterial = skillLockedMaterial;
            weaponImage = transform.Find("Image").GetComponent<Image>();
            backgroundImage = transform.Find("Background").GetComponent<Image>();
            frameImage = transform.Find("Frame").GetComponent<Image>();
            transform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                if (!playerWeapons.IsWeaponUnlocked(weaponType))
                {
                    // Skill not yet unlocked
                    if (!playerWeapons.TryUnlockWeaponRange(weaponType))
                    {
                        Tooltip_Warning.ShowTooltip_Static("Cannot unlock " + weaponType + "!");
                        AudioManager.instance.PlaySFX("UpgradeFail");
                    }
                }
            };
        }

        public void UpdateVisualS2()
        {
            if (playerWeapons.IsWeaponUnlocked(weaponType))
            {
                weaponImage.material = null;
                //success color
                backgroundImage.color = new Color(0.088f, 0.264f, 0.092f, 1f);
                frameImage.color = Color.yellow;
                AudioManager.instance.PlaySFX("Upgrade");
                UI_SkillTree.instance.weapon2.sprite = weaponImage.sprite;
            }
            else
            {
                if (playerWeapons.CanUnlockRange(weaponType))
                {
                    weaponImage.material = skillLockedMaterial;
                    backgroundImage.color = new Color(0.047f, 0.149f, 0.145f, 1f);
                }
                else
                {
                    weaponImage.material = skillLockedMaterial;
                    backgroundImage.color = Color.black;
                }
            }
        }
    }

    [System.Serializable]
    public class WeaponUnlockPath
    {
        public PlayerWeapons.WeaponType weaponType;
        public Image[] linkImageArray;
    }

    [System.Serializable]
    public class WeaponUnlockPath2
    {
        public PlayerWeapons.WeaponType weaponType;
        public Image[] linkImageArray;
    }
}
