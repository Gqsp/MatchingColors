using TMPro;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI deathCounterText;

    private void Start()
    {
        PlayerDeathHandler.OnPlayerDeath += UpdateDeathCounter;
        UpdateDeathCounter();
    }
    
    private void OnDestroy()
    {
        PlayerDeathHandler.OnPlayerDeath -= UpdateDeathCounter;
    }
    
    private void UpdateDeathCounter()
    {
        deathCounterText.text = GameData.deaths.ToString();
    }
}
