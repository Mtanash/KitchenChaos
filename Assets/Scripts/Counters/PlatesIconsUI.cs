using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatesIconsUI : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] Transform iconsTemplate;

    private void Awake()
    {
        iconsTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual() { 
        foreach (Transform transform in transform)
        {
            if (transform == iconsTemplate) continue;
            Destroy(transform.gameObject);
        }

        foreach(KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOArray())
        {
            Transform spawnedIconTempalte =  Instantiate(iconsTemplate, transform);
            spawnedIconTempalte.gameObject.SetActive(true);
            if (spawnedIconTempalte.TryGetComponent<PlateIconSingleUI>(out PlateIconSingleUI iconSingleUI))
            {
                iconSingleUI.UpdateImageSource(kitchenObjectSO.kitchenObjectSprite);
            }
        }
    }
}
