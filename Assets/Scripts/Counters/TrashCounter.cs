using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {

        // player have a kitchen object
        if (player.HasKitchenObject())
        {
           player.GetKitchenObject().DestroySelf();
        }

    }

    public override void InteractAlternate(Player player)
    {
        return;
    }
}
