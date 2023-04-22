using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter, IHasProgress
{

    public event EventHandler<OnStoveStateChangeEventArgs> OnStoveStateChange;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStoveStateChangeEventArgs : EventArgs
    {
        public State state;
    }

    [SerializeField] FryingRecipeSO[] fryingRecipieSOs;
    [SerializeField] BurningRecipeSO[] burningRecipieSOs;

    public enum State
    {
        IDLE,
        FRYING,
        FRIED,
    }

    private float fryingTimer;
    private float burningTimer;
    private State state;
    private FryingRecipeSO currentFryingRecipeSO;
    private BurningRecipeSO currentBurningRecipeSO;

    private void Start()
    {
        state = State.IDLE;
    }

    private void Update()
    {
        switch (state)
        {
            case State.IDLE:
                break;
            case State.FRYING:
                fryingTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressFillAmoutNormalized = fryingTimer / currentFryingRecipeSO.maxFryingTime
                });

                if (fryingTimer > currentFryingRecipeSO.maxFryingTime)
                {
                    fryingTimer = 0;

                    // destroy current kitchen object and instantiate the output one
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(currentFryingRecipeSO.output, this);

                    burningTimer = 0;

                    currentBurningRecipeSO = GetBurinigRecipieSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());

                    state = State.FRIED;
                    OnStoveStateChange?.Invoke(this, new OnStoveStateChangeEventArgs { state = state });
                }
                break;
            case State.FRIED:
                burningTimer += Time.deltaTime;

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressFillAmoutNormalized = burningTimer / currentBurningRecipeSO.maxBurningTime
                });

                if (burningTimer > currentBurningRecipeSO.maxBurningTime)
                {
                    state = State.IDLE;
                    burningTimer = 0;
                    OnStoveStateChange?.Invoke(this, new OnStoveStateChangeEventArgs { state = state });


                    // destroy current kitchen object and instantiate the output one
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(currentBurningRecipeSO.output, this);
                }

                break;
            default:
                break;
        }
    }

    public override void Interact(Player player)
    {
        // stove counter has a kitchen object
        if (HasKitchenObject())
        {
            // player does not have a kitchen object
            if (!player.HasKitchenObject())
            {
                // give the kitchen object to the player
                state = State.IDLE;
                OnStoveStateChange?.Invoke(this, new OnStoveStateChangeEventArgs { state = state });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressFillAmoutNormalized = 0f
                });
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        else
        {
            // player have a kitchen object
            if (player.HasKitchenObject())
            {
                //check if the kitchen object is in the stove counter recipe input list
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // cash current frying recipeSO
                    currentFryingRecipeSO = GetFryingRecipieSOForKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO());

                    // give the kitchen object to the stove counter
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    // configure the state machine and reset the timer
                    fryingTimer = 0;
                    state = State.FRYING;
                    OnStoveStateChange?.Invoke(this, new OnStoveStateChangeEventArgs { state = state });

                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        return;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO fryingRecipie = GetFryingRecipieSOForKitchenObjectSO(kitchenObjectSO);
        if (fryingRecipie)
        {
            return true;
        }
        return false;
    }

    private FryingRecipeSO GetFryingRecipieSOForKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipieSO in fryingRecipieSOs)
        {
            if (fryingRecipieSO.input == kitchenObjectSO)
            {
                return fryingRecipieSO;
            }
        }
        return null;
    }

    private BurningRecipeSO GetBurinigRecipieSOForKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipieSO in burningRecipieSOs)
        {
            if (burningRecipieSO.input == kitchenObjectSO)
            {
                return burningRecipieSO;
            }
        }
        return null;
    }
}
