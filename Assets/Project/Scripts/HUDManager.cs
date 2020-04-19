using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text timeText;

    public void SetHealth(int health)
    {
        healthText.text = "Crystal Health: " + health;
    }
    
    public void SetTime(int time)
    {
        timeText.text = "Time: " + time;
    }
}