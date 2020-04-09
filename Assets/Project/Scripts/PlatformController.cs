using UnityEngine;

namespace Jusw85.Common
{
    /// <summary>
    /// Usage:
    /// - Set script execution after other scripts have updated frameInfo, but before Physics2D.Simulate
    /// </summary>
    [RequireComponent(typeof(Raycaster), typeof(Rigidbody2D))]
    public class PlatformController : MonoBehaviour
    {
        // set vertical accel
        // sets horizontal accel
        
        private Raycaster raycaster;
        private Rigidbody2D rb2d;

        private FrameInfo frameInfo;

        private void Awake()
        {
            raycaster = GetComponent<Raycaster>();
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // update rb2d.position based on frameinfo
            frameInfo = new FrameInfo();
        }

        private void OnValidate()
        {
        }

        public struct FrameInfo
        {
        }

        // add (isJumpingThisFrame) callback?
        
        public enum State
        {
            GROUNDED,
            JUMPING,
        }
    }
}
