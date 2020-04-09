using UnityEngine;

namespace Jusw85.Common
{
    public class Physics2DManager : MonoBehaviour
    {
        private void Awake()
        {
            Physics2D.autoSimulation = false;
            Physics2D.queriesStartInColliders = false;
            Physics2D.autoSyncTransforms = false;
        }

        private void Update()
        {
            Physics2D.Simulate(Mathf.Epsilon);
        }
    }
}