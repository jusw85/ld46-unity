using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text text;

    public void SetHealth(int health)
    {
        text.text = "Crystal Health: " + health;
    }
}