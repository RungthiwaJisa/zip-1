using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private List<WeaponData> availableWeapons = new List<WeaponData>();

    [Header("UI Elements")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform weaponListContent;
    [SerializeField] private GameObject weaponItemPrefab;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private Button closeShopButton;

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
    }

    private void OnDestroy()
    {
        UIManager.OnUIShopButton -= OpenShop;
        UIManager.OnUIReturnButton -= CloseShop;

        if (closeShopButton != null)
        {
            closeShopButton.onClick.RemoveListener(CloseShop);
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
            PopulateWeaponList();
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
    }

    private void PopulateWeaponList()
    {
        foreach (Transform child in weaponListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var weaponData in availableWeapons)
        {
            GameObject weaponItem = Instantiate(weaponItemPrefab, weaponListContent);

            Image iconImage = weaponItem.GetComponentInChildren<Image>();
            TMP_Text nameText = weaponItem.GetComponentInChildren<TMP_Text>();
            Button button = weaponItem.GetComponent<Button>();

            if (iconImage != null && weaponData.weaponIcon != null)
            {
                iconImage.sprite = weaponData.weaponIcon;
            }

            if (nameText != null)
            {
                nameText.text = weaponData.weaponName;
            }

            if (button != null)
            {
                button.onClick.AddListener(() => ShowWeaponDetails(weaponData));
            }
        }
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
                stats = $"ความเสียหาย: {selectedWeaponInstance.damage}\n" +
                        $"ความจุ: {selectedWeaponInstance.capacity}\n" +
                        $"ระดับ: {selectedWeaponInstance.level}";
            }
            else
            {
                stats = $"ความเสียหาย: {weaponData.baseDamage}\n" +
                        $"ความจุ: {weaponData.baseCapacity}\n" +
                        $"ราคา: {weaponData.basePrice} ทอง";
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
            equipButton.GetComponentInChildren<TMP_Text>().text = isEquipped ? "กำลังใช้" : "ใช้";
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
            goldText.text = $"ทอง: {player.gold}";
        }
    }
}
