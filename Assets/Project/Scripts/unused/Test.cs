using System;
using UnityEngine;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    [SerializeField] private UnityEvent destroyedOther;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // always called before Update and LateUpdate
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("OnTriggerEnter! " + Time.frameCount);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // Debug.Log("OnTriggerStay!" + Time.frameCount);
    }
}