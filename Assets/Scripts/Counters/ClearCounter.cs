using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
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
                else if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                {
                    // counter has a plat

                    bool ingredientAdded = plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO());

                    if (ingredientAdded)
                    {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        return;
    }

}
