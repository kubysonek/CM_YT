using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ContainerCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;
    
    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            //Player is not carrying KitchenObject
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }


}
