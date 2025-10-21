using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<float, int, int> UpdateHUDStats;
    public UnityEvent<float, int, int> GameOver;
    private float timeAlive = 0f;
    private int kills = 0;
    private int damage = 0;
    private bool isPlayerAlive;

    public void Start()
    {
        UpdateHUDText();
        isPlayerAlive = true;
    }

    public void LateUpdate()
    {
        if(isPlayerAlive) {
            timeAlive += Time.deltaTime;
            UpdateHUDText();
        }
    }

    public void AddKills()
    {
        kills += 1;
        UpdateHUDText();
    }

    public void AddDamage(int dam)
    {
        damage += dam;
        UpdateHUDText();
    }

    private void UpdateHUDText()
    {
        if (isPlayerAlive)
            UpdateHUDStats?.Invoke(timeAlive, kills, damage);
    }
    
    public void StopGame()
    {
        isPlayerAlive = false;
        GameOver?.Invoke(timeAlive, kills, damage);
    }
}
