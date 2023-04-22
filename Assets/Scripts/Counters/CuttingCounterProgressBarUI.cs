using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingCounterProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject targetGameObject;

    private IHasProgress hasProgress;

    private void Start()
    {
        
        if (!targetGameObject.TryGetComponent<IHasProgress>(out hasProgress))
        {
            Debug.Log("Gmaeobject" + targetGameObject + "does not have a component of type IHasProgress");
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        progressBar.fillAmount = e.progressFillAmoutNormalized;
        if (e.progressFillAmoutNormalized == 0 || e.progressFillAmoutNormalized == 1)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
