using Jusw85.Common;
using UnityEngine;

[RequireComponent(typeof(PlatformController))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float walkVelocity = 8;

    private PlatformController platformController;

    private void Awake()
    {
        platformController = GetComponent<PlatformController>();
    }

    private void Update()
    {
        
    }
}