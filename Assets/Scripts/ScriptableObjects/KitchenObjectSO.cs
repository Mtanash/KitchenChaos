using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform kitchenObjectPrefab;
    public Sprite kitchenObjectSprite;
    public string kitchenObjectName;
}
