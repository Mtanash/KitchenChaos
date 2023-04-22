using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] CuttingRecipieSO[] cuttingRecipieSOs;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float recipeCuttingCountNormalized;
    }

    private int recipeCuttingCount;

    public override void Interact(Player player)
    {
        // cutting counter has a kitchen object
        if (HasKitchenObject())
        {
            // player does not have a kitchen object
            if (!player.HasKitchenObject())
            {
                // give the kitchen object to the player
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressFillAmoutNormalized = 0f
                });
            }
            else // player has a kitchen object and the counter has a kitchen object
            {

                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // player has a plat

                    bool ingredientAdded = plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO());

                    if (ingredientAdded)
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
        else
        {
            // player have a kitchen object
            if (player.HasKitchenObject())
            {
                //check if the kitchen object is in the cuting counter recipe input list
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    // give the kitchen object to the cutting counter
                    recipeCuttingCount = 0;

                    CuttingRecipieSO cuttingRecipieSO = GetCuttingRecipieSOForKitchenObjectSO(player.GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressFillAmoutNormalized = (float)recipeCuttingCount / cuttingRecipieSO.maxCuttingCount
                    });
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

            recipeCuttingCount++;

            CuttingRecipieSO cuttingRecipie = GetCuttingRecipieSOForKitchenObjectSO(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressFillAmoutNormalized = (float)recipeCuttingCount / cuttingRecipie.maxCuttingCount
            });

            OnCut?.Invoke(this, EventArgs.Empty);

            if (outputKitchenObjectSO && recipeCuttingCount >= cuttingRecipie.maxCuttingCount)
            {
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipieSO cuttingRecipie = GetCuttingRecipieSOForKitchenObjectSO(kitchenObjectSO);
        if (cuttingRecipie)
        {
            return cuttingRecipie.output;
        }
        return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipieSO cuttingRecipie = GetCuttingRecipieSOForKitchenObjectSO(kitchenObjectSO);
        if (cuttingRecipie)
        {
            return true;
        }
        return false;
    }

    private CuttingRecipieSO GetCuttingRecipieSOForKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        foreach (CuttingRecipieSO cuttingRecipieSO in cuttingRecipieSOs)
        {
            if (cuttingRecipieSO.input == kitchenObjectSO)
            {
                return cuttingRecipieSO;
            }
        }
        return null;
    }
}
