using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private readonly List<GameObject> spawnedPlates = new();

    private void Start()
    {
        platesCounter.OnPlatSpawned += PlatesCounter_OnPlatSpawned;
        platesCounter.OnPlatRemoved += PlatesCounter_OnPlatRemoved;
    }

    private void PlatesCounter_OnPlatRemoved(object sender, System.EventArgs e)
    {
        GameObject lastPlate = spawnedPlates[spawnedPlates.Count - 1];
        spawnedPlates.Remove(lastPlate);
        Destroy(lastPlate);
    }

    private void PlatesCounter_OnPlatSpawned(object sender, System.EventArgs e)
    {
        Transform spawnedPalte = Instantiate(plateVisualPrefab, counterTopPoint);
        spawnedPalte.transform.localPosition = new (0, 0 + (float)(spawnedPlates.Count * .1), 0);
        spawnedPlates.Add(spawnedPalte.gameObject);
    }
}
