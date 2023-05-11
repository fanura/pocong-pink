using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class road : Terrain
{
    [SerializeField] Kunti kuntiPrefab;
    [SerializeField] float minKuntiSpawnInterval;
    [SerializeField] float maxKuntiSpawnInterval;

    float timer;
    Vector3 kuntiSpawnPosition;
    Quaternion KuntiRotation;

    private void Start(){

        if(Random.value > 0.5f)
        {
        kuntiSpawnPosition = new Vector3(
        horizontalSize/2 + 3,
        0.5f, 
        this.transform.position.z + 1);

        KuntiRotation = Quaternion.Euler(0,-90,0);
        }

        else
        {
        kuntiSpawnPosition = new Vector3(
        -(horizontalSize/2 + 3),
        0.5f, 
        this.transform.position.z + 1);
        KuntiRotation = Quaternion.Euler(0,90,0);
        }
    }

    private void Update() {
        if(timer <= 0)
        {
        timer = Random.Range(minKuntiSpawnInterval, maxKuntiSpawnInterval);

        var kunti = Instantiate(
            kuntiPrefab,
            kuntiSpawnPosition,
            KuntiRotation);

        kunti.SetUpDistanceLimit(horizontalSize + 6);
        return;
        }
        timer -= Time.deltaTime;
    }
    
}
