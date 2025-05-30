using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter {

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenSO;
    
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int platesSpawnedAmountMax = 4;
    
    private void Update() {
        
        spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax) {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < platesSpawnedAmountMax) {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }

        }
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //Player is empty handed
            if (platesSpawnedAmount > 0) {
                //There is at least 1 plate here
                platesSpawnedAmount--;
                
                KitchenObject.SpawnKitchenObject(plateKitchenSO, player);
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
