using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter {
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no KitchenObject on the counter
            if (player.HasKitchenObject()) {
                //Player is carrying KitchenObject
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else {
                //Player is not carrying KitchenObject
            }
        }
        else {
            //There is a KitchenObject on the counter
            if (player.HasKitchenObject()) {
                //Player is carrying KitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    //Player is holding a plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else {
                    //Player is holding sth else
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) {
                        //Player is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else {
                //Player is not carrying KitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}