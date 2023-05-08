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
        var nisanCount = Mathf.FloorToInt((float)size / 3);

        List<int>emptyPosition = new List<int>();
        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

            for (int i = 0; i <= nisanCount; i++)
        {
            // memilih posisi kosong secara random
            var randomIndex = Random.Range(0, emptyPosition.Count-1);
            var pos = emptyPosition[randomIndex];

            // posisi yang terpilih dihapus dari daftar posisi kosong
            emptyPosition.RemoveAt(randomIndex);

            SpawnRandomNisan(pos);
           

        }   

                // selalu ada nisan diujung
        SpawnRandomNisan(-limit -1);
        SpawnRandomNisan(limit +1);

    }     



        private void SpawnRandomNisan(int pos)
        {
            // set nisan
            var nisan = Instantiate(nisanPrefab, transform);
            nisan.transform.localPosition = new Vector3(pos,1.5f,2);
            
        }
}
