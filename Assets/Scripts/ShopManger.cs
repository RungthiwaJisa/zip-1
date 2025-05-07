using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<WeaponData> availableWeapons = new List<WeaponData>();
    [SerializeField] private GameObject lobbyPage;
    [SerializeField] private UIManager uIManager;

    [Header("UI Elements")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Button closeShopButton;
    [SerializeField] private Button gun1;
    [SerializeField] private Button gun2;
    [SerializeField] private Button gun3;


    // Weapon detail panel
    [SerializeField] private GameObject weaponDetailPanel;
    [SerializeField] private Image weaponImage;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponStatsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button equipButton;

    private WeaponData selectedWeapon;
    private Weapons selectedWeaponInstance;

    private void Start()
    {
        lobbyPage.SetActive(true);

        UIManager.OnUIShopButton += OpenShop;
        UIManager.OnUIReturnButton += CloseShop;

        if (closeShopButton != null)
        {
            closeShopButton.onClick.AddListener(CloseShop);
        }

        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        if (weaponDetailPanel != null)
        {
            weaponDetailPanel.SetActive(false);
        }

        // เพิ่มการเชื่อมต่อปุ่มอาวุธ
        SetupWeaponButtons();
    }

    // เพิ่มฟังก์ชันใหม่สำหรับตั้งค่าปุ่มอาวุธ
    private void SetupWeaponButtons()
    {
        if (gun1 != null && availableWeapons.Count >= 1)
        {
            gun1.onClick.AddListener(() => ShowWeaponDetails(availableWeapons[0]));
        }

        if (gun2 != null && availableWeapons.Count >= 2)
        {
            gun2.onClick.AddListener(() => ShowWeaponDetails(availableWeapons[1]));
        }

        if (gun3 != null && availableWeapons.Count >= 3)
        {
            gun3.onClick.AddListener(() => ShowWeaponDetails(availableWeapons[2]));
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            ShowWeaponDetails(availableWeapons[0]);
            UpdateGoldText();
        }
    }

    public void CloseShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }

        if (weaponDetailPanel != null)
        {
            weaponDetailPanel.SetActive(false);
        }

        uIManager.LobbyWindow();
    }

    private void ShowWeaponDetails(WeaponData weaponData)
    {
        selectedWeapon = weaponData;

        selectedWeaponInstance = null;
        bool isOwned = false;
        foreach (var weapon in player.ownedWeapons)
        {
            if (weapon.weaponName == weaponData.weaponName)
            {
                isOwned = true;
                selectedWeaponInstance = weapon;
                break;
            }
        }

        if (weaponDetailPanel != null)
        {
            weaponDetailPanel.SetActive(true);
        }

        if (weaponImage != null && weaponData.weaponIcon != null)
        {
            weaponImage.sprite = weaponData.weaponIcon;
        }

        if (weaponNameText != null)
        {
            weaponNameText.text = weaponData.weaponName;
        }

        if (weaponStatsText != null)
        {
            string stats;
            if (isOwned && selectedWeaponInstance != null)
            {
                stats = $"Damages: {selectedWeaponInstance.damage}\n" +
                        $"Capasity: {selectedWeaponInstance.capacity}\n" +
                        $"Level: {selectedWeaponInstance.level}";
            }
            else
            {
                stats = $"Damages: {weaponData.baseDamage}\n" +
                        $"Capasity: {weaponData.baseCapacity}\n" +
                        $"Prices: {weaponData.basePrice} golds";
            }
            weaponStatsText.text = stats;
        }

        if (buyButton != null)
        {
            buyButton.gameObject.SetActive(!isOwned);
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => BuyWeapon(weaponData));
            buyButton.interactable = player.gold >= weaponData.basePrice;
        }

        if (upgradeButton != null)
        {
            upgradeButton.gameObject.SetActive(isOwned);
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => UpgradeWeapon());

            if (isOwned && selectedWeaponInstance != null)
            {
                int upgradePrice = selectedWeaponInstance.GetUpgradePrice();
                upgradeButton.interactable = player.gold >= upgradePrice;
            }
        }

        if (equipButton != null)
        {
            equipButton.gameObject.SetActive(isOwned);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => EquipWeapon());

            bool isEquipped = player.currentWeapon == selectedWeaponInstance;
            equipButton.interactable = !isEquipped;
            equipButton.GetComponentInChildren<TMP_Text>().text = isEquipped ? "Alredy Use" : "Use";
        }
    }

    private void BuyWeapon(WeaponData weaponData)
    {
        if (player.gold >= weaponData.basePrice)
        {
            GameObject weaponObj = new GameObject(weaponData.weaponName);
            Weapons newWeapon = weaponObj.AddComponent<Weapons>();
            newWeapon.weaponData = weaponData;
            newWeapon.weaponName = weaponData.weaponName;
            newWeapon.damage = weaponData.baseDamage;
            newWeapon.capacity = weaponData.baseCapacity;
            newWeapon.price = weaponData.basePrice;
            newWeapon.weaponIcon = weaponData.weaponIcon;

            player.gold -= weaponData.basePrice;
            player.BuyWeapon(newWeapon);

            UpdateGoldText();
            ShowWeaponDetails(weaponData);

            Debug.Log($"Bought {weaponData.weaponName} for {weaponData.basePrice} gold");
        }
    }

    private void UpgradeWeapon()
    {
        if (selectedWeaponInstance != null)
        {
            int upgradePrice = selectedWeaponInstance.GetUpgradePrice();

            if (player.gold >= upgradePrice)
            {
                player.gold -= upgradePrice;
                player.UpgradeWeapon(selectedWeaponInstance);

                UpdateGoldText();
                ShowWeaponDetails(selectedWeapon);
            }
        }
    }

    private void EquipWeapon()
    {
        if (selectedWeaponInstance != null)
        {
            player.SelectWeapon(selectedWeaponInstance);
            ShowWeaponDetails(selectedWeapon);
        }
    }

    private void UpdateGoldText()
    {
        if (goldText != null)
        {
            goldText.text = $"Golds: {player.gold}";
        }
    }
}
