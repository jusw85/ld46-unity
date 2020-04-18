using k;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private int health;

    private HUDManager hudManager;

    private void Start()
    {
        MainGame game = GameObject.FindWithTag(Tags.MAIN_GAME)?.GetComponent<MainGame>();
        if (game.IsInitialised)
        {
            initHudManager(game);
        }
        else
        {
            game.InitialisedCallback += () => { initHudManager(game); };
        }
    }

    private void initHudManager(MainGame game)
    {
        hudManager = game.HudManager;
        hudManager?.SetHealth(health);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals(Tags.ENEMY))
        {
            Debug.Log("Crystal: touched");
            hudManager?.SetHealth(--health);
            Destroy(other.gameObject);
        }
    }
}