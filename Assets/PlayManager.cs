using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] grass grassPrefab;
    [SerializeField] road roadPrefab;
    [SerializeField] int initialGrassCount = 5;
    [SerializeField] int horizontalSize;
    [SerializeField] int backViewDistances = -5;
    [SerializeField] int forwardViewDistances = 15;
    [SerializeField, Range(0,1)] float nisanProbability;

    private List<Terrain> terrainList;
    Dictionary<int, Terrain> activeTerrainDict = new Dictionary<int, Terrain>(20);

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
            
            var terrain = SpawnRandomTerrain(zPos);

            
            terrain.Generate(horizontalSize);

            activeTerrainDict[zPos] = terrain;
        }
        SpawnRandomTerrain(0);
    }

    private Terrain SpawnRandomTerrain(int zPos)
    {
        Terrain terrainCheck = null;
        int randomIndex;
        Terrain terrain = null;
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
                terrain = Instantiate(terrainList[randomIndex]);
                terrain.transform.localPosition = new Vector3(0,0,zPos);
            return terrain;
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
    terrain = Instantiate(CandidateTerrain[randomIndex]);
    terrain.transform.position = new Vector3(0,0,zPos);
            return terrain;
    }
}
