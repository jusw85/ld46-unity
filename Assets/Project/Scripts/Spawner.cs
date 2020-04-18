using System.Collections;
using UnityEngine;

public enum TileType {
    Village, Castle
}

public class Spawner : MonoBehaviour {

    // private PoolManager poolManager;
    // public GameObject[] spawnVillagePlatform;
    // public GameObject[] spawnCastlePlatform;
    // public GameObject[] spawnGrass;
    // public GameObject[] spawnMountain;

    public float initialDelay = 3.0f;
    public float spawnMin = 1.0f;
    public float spawnMax = 2.0f;

    public bool runForever = false;
    public int spawnInstances = 3;
    public int numPerSpawn = 1;

    private bool isStarted = false;
    private bool isRunning = false;

    public float walkSpeed;

    public GameObject objectToSpawn;
    
    // private void Start() {
        // poolManager = FindObjectOfType<PoolManager>();
        // poolManager.CreatePool(spawnGrass[0], 3);
        // poolManager.CreatePool(spawnMountain[0], 3);
        // foreach (GameObject o in spawnVillagePlatform) {
        //     poolManager.CreatePool(o, 7);
        // }
        // foreach (GameObject o in spawnCastlePlatform) {
        //     poolManager.CreatePool(o, 7);
        // }
    // }

    // Update is called once per frame
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
            spawnInstances -= numPerSpawn;
            for (int i = 0; i < numPerSpawn; i++)
            {
                GameObject obj = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                obj.GetComponent<EnemyMover>().WalkSpeed = walkSpeed;    
            }
            yield return new WaitForSeconds(Random.Range(spawnMin, spawnMax));
        }
        isRunning = false;
    }

    // public void SpawnPlatform(Vector2 r) {
    //     switch (t) {
    //         case TileType.Castle:
    //             SpawnCastlePlatform(r);
    //             break;
    //         case TileType.Village:
    //         default:
    //             SpawnVillagePlatform(r);
    //             break;
    //     }
    //     //GameObject obj = poolManager.ReuseObject(spawnVillagePlatform[Random.Range(0, spawnVillagePlatform.Length)], new Vector3(r.x, r.y, 0f), Quaternion.identity);
    //     //AdjustPos(obj);
    // }

}