using System;
using Jusw85.Common;
using k;
using Prime31.ZestKit;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private int health;

    private HUDManager hudManager;
    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

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
        if (isEnding)
        {
            return;
        }

        if (other.tag.Equals(Tags.ENEMY))
        {
            // Debug.Log("Crystal: touched");
            hudManager?.SetHealth(--health);
            // rend.color = Color.white;
            // rend.ZKcolorTo(new Color(1.0f, 200/255f, 200/255f), 0.2f).setLoops(LoopType.PingPong).start();
            if (health <= 0)
            {
                StartCoroutine(CoroutineUtils.DelaySeconds(() =>
                {
                    rend.ZKalphaTo(0, 1).start();
                }, 2));
                GameObject.FindWithTag(Tags.MAIN_GAME)?.GetComponent<MainGame>()?.Lose();
            }

            Destroy(other.gameObject);
        }
    }

    private bool isEnding;

    public bool IsEnding
    {
        get => isEnding;
        set => isEnding = value;
    }
}