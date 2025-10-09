using TMPro;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI timeAliveText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI damageText;

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

    public void AddDamage()
    {
        damage += 1;
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
}
