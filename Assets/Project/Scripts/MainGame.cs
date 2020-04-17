using Jusw85.Common;
using k;
using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    private HUDManager hudManager;
    private SoundKit soundKit;

    private bool selfInit = true;

    public bool SelfInit
    {
        get => selfInit;
        set => selfInit = value;
    }

    private void Start()
    {
        soundKit = Toolbox.Instance.Get<SoundKit>();

        if (SelfInit)
        {
            Init();
        }
    }

    public void Init()
    {
        LoadHUD();
        soundKit.playBackgroundMusic(bgm, 1.0f);
    }

    private void LoadHUD()
    {
        Scene hudScene = SceneManager.GetSceneByName(Scenes.HUD);

        if (Application.isEditor && hudScene.isLoaded)
        {
            InitHUD();
        }
        else
        {
            enabled = false;
            AsyncOperation op = SceneManager.LoadSceneAsync(Scenes.HUD, LoadSceneMode.Additive);
            op.completed += operation =>
            {
                InitHUD();
                enabled = true;
            };
        }

        return;

        void InitHUD()
        {
            Camera[] cams = FindObjectsOfType<Camera>();
            foreach (Camera cam in cams)
            {
                if (cam.gameObject.scene.name.Equals(Scenes.HUD))
                {
                    cam.gameObject.SetActive(false);
                    break;
                }
            }

            GameObject obj = GameObject.FindWithTag(Tags.HUDMANAGER);
            if (obj != null)
            {
                hudManager = obj.GetComponent<HUDManager>();
            }
            else
            {
                Debug.LogError("HUDManager is null!");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            hudManager.SetText();
        }
    }
}