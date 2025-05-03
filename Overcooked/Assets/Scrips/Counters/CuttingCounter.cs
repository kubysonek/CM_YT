using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {


    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    private int cuttingProgress;
    
    

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no KitchenObject on the counter
            if (player.HasKitchenObject()) {
                //Player is carrying KitchenObject
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //This KitchenObject can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
            }
            else {
                //Player is not carrying KitchenObject
            }
        }
        else {
            //There is a KitchenObject on the counter
            if (!player.HasKitchenObject()) {
                //Player is not carrying KitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
   
                //To hide ProgressBarUI - progress is reset on placing an item anyway so
                OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
            }
            else {
                //Player is carrying KitchenObject
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
            //There is a KitchenObject on the counter and it can be cut
            
            cuttingProgress++;
            OnCut?.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this,new IHasProgress.OnProgressChangedEventArgs {
                progressNormalized = (float) cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecpieSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecpieSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null) {
            return cuttingRecipeSO.output;
        }
        return null;
    }

    private CuttingRecipeSO GetCuttingRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }
        return null;
    }
}