using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    [SerializeField] private List<KitchenObjectSO> allowedKitchenObjectSoArray;

    private List<KitchenObjectSO> kitchenObjectSOArray;

    private void Awake()
    {
        kitchenObjectSOArray = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {

        // make sure the kitchenObjectSO does not duplicate and kitchenObjectSO is allowed
        if (!kitchenObjectSOArray.Contains(kitchenObjectSO) && allowedKitchenObjectSoArray.Contains(kitchenObjectSO))
        {
            kitchenObjectSOArray.Add(kitchenObjectSO);
            return true;
        }
        return false;
    }
}
