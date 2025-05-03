using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {


    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs {
        public State state;
    }
    public enum State {
        Idle,
        Frying,
        Fried,
        Burned,
    }
    
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private void Start() {
        state = State.Idle;
    }

    private void Update() {

        if (HasKitchenObject()) {
            switch (state) {
                case State.Idle:
                    break;
                case State.Frying:
                    fryingTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingProgressMax
                    });
                    
                    if (fryingTimer >= fryingRecipeSO.fryingProgressMax) {
                        //Fried

                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
                        
                        state = State.Fried;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        
                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    }
                    break;
                case State.Fried:
                    burningTimer += Time.deltaTime;
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = burningTimer / burningRecipeSO.burningProgressMax
                    });
                    
                    if (burningTimer >= burningRecipeSO.burningProgressMax) {
                        //Burned
                        
                        GetKitchenObject().DestroySelf();
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        state = State.Burned;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressNormalized = 0f
                        });
                    }
                    break;
                case State.Burned:
                    break;
            }
        }


        
    }

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            //There is no KitchenObject on the counter
            if (player.HasKitchenObject()) {
                //Player is carrying KitchenObject
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    //This KitchenObject can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecpieSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                    
                    state = State.Frying;
                    fryingTimer = 0f;
                    
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                        state = state
                    });
                    
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingProgressMax
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
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                    state = state
                });
                
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressNormalized = 0f
                });
                
            }
            else {
                //Player is carrying KitchenObject
            }
        }
    }
    
    
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecpieSOWithInput(inputKitchenObjectSO);
        return fryingRecipeSO != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
        FryingRecipeSO fryingRecipeSO = GetFryingRecpieSOWithInput(inputKitchenObjectSO);
        if (fryingRecipeSO != null) {
            return fryingRecipeSO.output;
        }
        return null;
    }

    private FryingRecipeSO GetFryingRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
            if (fryingRecipeSO.input == inputKitchenObjectSO) {
                return fryingRecipeSO;
            }
        }
        return null;
    }
    
    private BurningRecipeSO GetBurningRecpieSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
            if (burningRecipeSO.input == inputKitchenObjectSO) {
                return burningRecipeSO;
            }
        }
        return null;
    }
}
