using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformsManager : MonoBehaviour
{
    [Header("Platforms Spawning Probability")]
    public GameObject[] Platform;

    [Header("SETTINGS")]
    [SerializeField] private int numPlatformsToSpawn; 
    [SerializeField] private float platformWidth;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;
    [SerializeField] private float spawnOffSet;

    //[SerializeField] private float alphaMin;
    //[SerializeField] private float alphaMax;

    int prefabIndex;
    private List<GameObject> platforms = new List<GameObject>();
    private Transform playerTransform;

    private float lastSpawnPosX;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //Check if player is on the screen
        lastSpawnPosX = playerTransform.position.x + 1;

        SpawnPlatforms(numPlatformsToSpawn); //Spawn an amount of platforms (Index sets default to 1 at the beginning)
    }

    private void SpawnPlatforms(int numPlatforms)
    {
        for (int i = 0; i < numPlatforms; i++)
        {
            float height = Random.Range(minHeight, maxHeight);
            Vector3 spawnPos = new Vector3(lastSpawnPosX + platformWidth, height, 0f);

            GameObject newPlatform = Instantiate (Platform[prefabIndex], spawnPos, Quaternion.identity);

            //SetRandomAlpha(newPlatform);

            platforms.Add(newPlatform);

            lastSpawnPosX = newPlatform.transform.position.x;
        }
    }
    
    //     private void SetRandomAlpha(GameObject platform) //Set the alpha of each platform randomly
    // {
    //     SpriteRenderer[] spriteRenderers = platform.GetComponentsInChildren<SpriteRenderer>();
    //     float randomAlpha = Random.Range(alphaMin, alphaMax);

    //     foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    //     {
    //         Color color = spriteRenderer.color;
    //         color.a = randomAlpha;
    //         spriteRenderer.color = color;
    //     }
    // }
       
    private void Update()
    {
        if(playerTransform != null )
        {
            prefabIndex = Random.Range(0, Platform.Length); //Randomly spawn different types of platform after Start()
            
            if (playerTransform.position.x > lastSpawnPosX - spawnOffSet) SpawnPlatforms(1); // The 7.5 part used to be spawnOffSet

            if (platforms.Count > numPlatformsToSpawn)
            {
            Destroy(platforms[0]);
            platforms.RemoveAt(0);
            }
        }     
    }
}
