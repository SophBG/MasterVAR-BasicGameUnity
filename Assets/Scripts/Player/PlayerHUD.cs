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

    private float timeAlive = 0f;
    private int kills = 0;
    private int damage = 0;

    public void Start()
    {
        UpdateText();
    }

    public void LateUpdate()
    {
        timeAlive += Time.deltaTime;
        UpdateText();
    }

    public void AddKills()
    {
        kills += 1;
        UpdateText();
    }

    public void AddDamage(int dam)
    {
        damage += dam;
        UpdateText();
    }

    private string FormatTime(float timeInSeconds)
    {
        // Format time as HH:MM:SS
        int hours = Mathf.FloorToInt(timeInSeconds / 3600f);
        int minutes = Mathf.FloorToInt((timeInSeconds % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);

        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    private void UpdateText()
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
