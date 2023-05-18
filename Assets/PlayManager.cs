using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayManager : MonoBehaviour
{
    [SerializeField] Congky congky;
    [SerializeField] grass grassPrefab;
    [SerializeField] road roadPrefab;
    [SerializeField] int initialGrassCount = 5;
    [SerializeField] int horizontalSize;
    [SerializeField] int backViewDistances = -5;
    [SerializeField] int forwardViewDistances = 15;
    [SerializeField, Range(0,1)] float nisanProbability;

    private List<Terrain> terrainList;
    Dictionary<int, Terrain> activeTerrainDict = new Dictionary<int, Terrain>(20);

    [SerializeField] private int travelDistance;

    public UnityEvent <int, int> OnUpdateTerrainLimit;

    private void Start() {

        terrainList = new List<Terrain>
        {
            grassPrefab,
            roadPrefab
        };


        // create initial grass -4 --- 4
         for (int zPos = backViewDistances; zPos < initialGrassCount; zPos++)
        {
            var terrain =  Instantiate(terrainList[0]);

            terrain.transform.position = new Vector3(0,0,zPos);

            if(terrain is grass grass)
                grass.SetNisanPercentage(zPos < -1 ? 1 : 0);

            terrain.Generate(horizontalSize);

            activeTerrainDict[zPos] = terrain;
        }

        // create initial road 4 --- 15
    for (int zPos = initialGrassCount; zPos < forwardViewDistances; zPos++)
        {
            SpawnRandomTerrain(zPos);
       
        }
        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistances);

    }

     private Terrain SpawnRandomTerrain(int zPos)
    {
        Terrain comparatorTerrain = null;
        int randomIndex;

        for (int z = -1; z >= -3; z--)
        {
            var checkPos = zPos + z;
        
            if(comparatorTerrain == null)
            {
                comparatorTerrain = activeTerrainDict[checkPos];
                continue;
            }
            else if(comparatorTerrain.GetType() != activeTerrainDict[checkPos].GetType())
            {
                randomIndex = Random.Range(0, terrainList.Count);
                return SpawnTerrain(terrainList[randomIndex],zPos);
            }
            else
            {
                continue;
            }
        }

        var candidateTerrain = new List<Terrain>(terrainList);
        for (int i = 0; i < candidateTerrain.Count; i++)
        {
            if(comparatorTerrain.GetType() == candidateTerrain[i].GetType())
            {
                candidateTerrain.Remove(candidateTerrain[i]);
                break;
            }
        }

        randomIndex = Random.Range(0,candidateTerrain.Count);
        return SpawnTerrain(candidateTerrain[randomIndex], zPos);
    }

    public Terrain SpawnTerrain(Terrain terrain, int zPos)
    {
        terrain = Instantiate(terrain);
        terrain.transform.position = new Vector3(0,0,zPos);
        terrain.Generate(horizontalSize);
        activeTerrainDict[zPos] = terrain;
        return terrain;
    }

    public void UpdateTravelDistance(Vector3 targetPosition)
    {
        if(targetPosition.z > travelDistance)
        {
            travelDistance = Mathf.CeilToInt(targetPosition.z);
            UpdateTerrain();
            
        }
    }

        public void UpdateTerrain()
    {
        var destroyPos = travelDistance - 1 + backViewDistances;
        Destroy(activeTerrainDict[destroyPos].gameObject);
        activeTerrainDict.Remove(destroyPos);

        var spawnPosition = travelDistance - 1 + forwardViewDistances;
        SpawnRandomTerrain(spawnPosition);

        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistances);
    }
    
}
