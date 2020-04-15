using System;
using Prime31;
using UnityEngine;
using UnityEngine.SceneManagement;

public class _Test : MonoBehaviour
{
    private HUDManager hudManager;

    private void Start()
    {
        Scene hudScene = SceneManager.GetSceneByName("HUD");

        if (Application.isEditor && hudScene.isLoaded)
        {
            InitHUD();
        }
        else
        {
            enabled = false;
            AsyncOperation op = SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Additive);
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
                if (!cam.gameObject.scene.name.Equals("DynamicPlatforming"))
                {
                    cam.gameObject.SetActive(false);
                }
            }
            
            GameObject obj = GameObject.FindWithTag("HUDManager");
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