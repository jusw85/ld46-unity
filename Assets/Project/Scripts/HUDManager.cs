using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text text;

    private int z = 0;

    public void SetText()
    {
        z++;
        text.text = z.ToString();
    }
}