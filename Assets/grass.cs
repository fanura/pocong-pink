using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grass : Terrain
{
    [SerializeField] GameObject nisanPrefab;
    [SerializeField] List <GameObject> nisanPrefablist;
    [SerializeField, Range(0,1)] float nisanProbability;

 public void SetNisanPercentage(float newProbability)
   {
    this.nisanProbability = Mathf.Clamp01(newProbability);
   }

    public override void Generate(int size)
    {
        base.Generate(size);

        var limit = Mathf.FloorToInt((float)size / 2);
        var nisanCount = Mathf.FloorToInt((float)size * nisanProbability);

        List<int> emptyPosition = new List<int>();
        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

        //
        for (int i = 0; i < nisanCount; i++)
        {
            //Debug.Log(i + string.Join(",",emptyPosition));
            var randomIndex = Random.Range(0,emptyPosition.Count);
            var pos = emptyPosition [randomIndex];

            //
            emptyPosition.RemoveAt(randomIndex);
       
            SpawnRandomNisan(pos);
        }

        //
        SpawnRandomNisan(-limit -1);
        SpawnRandomNisan(limit +1);
    }

    private void SpawnRandomNisan(int xPos)
    {
       //
            var randomIndex = Random.Range(0, nisanPrefablist.Count);
            var prefab = nisanPrefablist[randomIndex];

            //
            var nisan = Instantiate(
                prefab,
                new Vector3(xPos,0,this.transform.position.z), 
                Quaternion.identity, 
                transform);
    }
}