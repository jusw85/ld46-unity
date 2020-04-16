using System;
using k;
using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Test : MonoBehaviour
{
    private HUDManager hudManager;

    private void Start()
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
                if (!cam.gameObject.scene.name.Equals(Scenes.DYNAMIC_PLATFORMING))
                {
                    cam.gameObject.SetActive(false);
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