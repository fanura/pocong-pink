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
        for (int zPos = backViewDistances;  zPos < initialGrassCount; zPos++)
        {
            var grass = Instantiate(grassPrefab);
            grass.transform.localPosition = new Vector3(0,0,zPos);
            grass.SetNisanPercentage(zPos < -1 ? 1 : 0);
            grass.Generate(horizontalSize);
            activeTerrainDict[zPos] = grass;
        }

        // create initial road 4 --- 15
        for (int zPos = initialGrassCount;  zPos < forwardViewDistances; zPos++)
        {
            
                SpawnRandomTerrain(zPos);

        }
    }

    private Terrain SpawnRandomTerrain(int zPos)
    {
        Terrain terrainCheck = null;
        int randomIndex;
        for (int z = -1; z >= -3; z--)
        {
            var checkPos = zPos + z;
            if(terrainCheck == null)
            {
                terrainCheck = activeTerrainDict[checkPos];
                continue;
            }
            else if(terrainCheck.GetType() != activeTerrainDict[checkPos].GetType())
            {
                randomIndex = Random.Range(0,terrainList.Count);
                return SpawnTerrain(terrainList[randomIndex], zPos);
            }
            else{
                continue;
            }
        }

        var CandidateTerrain = new List<Terrain>(terrainList);
        for (int i = 0; i < CandidateTerrain.Count; i++)
        {
            if(terrainCheck.GetType() == CandidateTerrain[i].GetType())
            {
                CandidateTerrain.Remove(CandidateTerrain[i]);
                break;
            }
        }

    randomIndex = Random.Range(0,CandidateTerrain.Count);
    return SpawnTerrain(CandidateTerrain[randomIndex], zPos);
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
        var destroyPos = travelDistance-1+backViewDistances;
        Destroy(activeTerrainDict[destroyPos].gameObject);
        activeTerrainDict.Remove(destroyPos);

       var spawnPosition = travelDistance - 1 + forwardViewDistances;
       SpawnRandomTerrain(spawnPosition);

        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistances);
       
    }
    
}
