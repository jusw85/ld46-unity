using System;
using System.Collections;
using k;
using UnityEngine;
using Random = UnityEngine.Random;

public enum TileType {
    Village, Castle
}

public class Spawner : MonoBehaviour {

    public float initialDelay = 3.0f;
    public float spawnMin = 1.0f;
    public float spawnMax = 2.0f;

    public bool runForever = false;
    public int spawnInstances = 3;
    public int numPerSpawn = 1;

    private bool isStarted;
    private bool isRunning;

    public float walkSpeed;

    public GameObject objectToSpawn;
    public Animator anim;

    private void Update() {
        if (!isStarted) {
            initialDelay -= Time.deltaTime;
            if (initialDelay <= 0)
                isStarted = true;
            return;
        }
        if (!isRunning && (runForever || spawnInstances > 0)) {
            StartCoroutine(SpawnCoroutine());
        }
    }

    IEnumerator SpawnCoroutine() {
        isRunning = true;
        while (runForever || (spawnInstances - numPerSpawn) >= 0) {
            anim.SetBool(AnimatorParams.SPAWNING, true);
            yield return new WaitForSeconds(2f);
            
            spawnInstances -= numPerSpawn;
            for (int i = 0; i < numPerSpawn; i++)
            {
                GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                obj.GetComponent<EnemyMover>().WalkSpeed = walkSpeed;    
            }
            
            yield return new WaitForSeconds(1f);
            anim.SetBool(AnimatorParams.SPAWNING, false);
            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(Random.Range(spawnMin, spawnMax));
        }
        isRunning = false;
    }

}