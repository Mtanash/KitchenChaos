using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO KitchenObjectSO;
    }

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
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { KitchenObjectSO = kitchenObjectSO });
            return true;
        }
        return false;
    }

    public List<KitchenObjectSO> GetKitchenObjectSOArray()
    {
        return kitchenObjectSOArray;
    }
}
