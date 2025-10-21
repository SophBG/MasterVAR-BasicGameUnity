using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("Match Stats")]
    public TextMeshProUGUI timeAliveText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI damageText;

    [Header("Gun Stats")]
    public TextMeshProUGUI gunNameText;
    public TextMeshProUGUI gunFireRateText;
    public TextMeshProUGUI gunDamageText;
    public TextMeshProUGUI gunAutoText;

    private string FormatTime(float timeInSeconds)
    {
        // Format time as HH:MM:SS
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public void UpdateText(float timeAlive, int kills, int damage)
    {
        timeAliveText.text = "Time: " + FormatTime(timeAlive);
        killsText.text = "Kills: " + kills;
        damageText.text = "Damage: " + damage;
    }

    public void DisplayWeapon(PlayerShooting.Weapon currentWeapon)
    {
        gunNameText.text = currentWeapon.name;
        gunFireRateText.text = "Fire Rate: " + currentWeapon.fireRate;
        gunDamageText.text = "Damage: " + currentWeapon.bulletDamage;
        if (currentWeapon.isAuto)
            gunAutoText.text = "Automatic";
        else
            gunAutoText.text = "Manual";
    }
}
