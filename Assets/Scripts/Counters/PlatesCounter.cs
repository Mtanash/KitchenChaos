using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateObjectSO;

    public event EventHandler OnPlatSpawned;
    public event EventHandler OnPlatRemoved;

    private float spawnPlatesTimer;
    private readonly float spawnPlatesTimerMax = 4f;

    private int spawnedPlatesCount;
    private readonly int spawnedPlatesCountMax = 4;

    private void Update()
    {
        spawnPlatesTimer += Time.deltaTime;

        if (spawnPlatesTimer > spawnPlatesTimerMax )
        {
            spawnPlatesTimer = 0;

            if (spawnedPlatesCount < spawnedPlatesCountMax )
            {
                spawnedPlatesCount++;

                // spawn the visual
                // fire event
                OnPlatSpawned?.Invoke(this, new EventArgs());
            }
        }
    }

    public override void Interact(Player player)
    {
        if (spawnedPlatesCount > 0 && !player.HasKitchenObject())
        {
            spawnedPlatesCount--;
            spawnPlatesTimer = 0f;
            OnPlatRemoved?.Invoke(this, new EventArgs());
            KitchenObject.SpawnKitchenObject(plateObjectSO, player);
        }
    }

    public override void InteractAlternate(Player player)
    {
        return;
    }
}
