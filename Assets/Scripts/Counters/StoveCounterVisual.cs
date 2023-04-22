using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] GameObject particles;
    [SerializeField] GameObject stoveHeat;

    private void Start()
    {
        stoveCounter.OnStoveStateChange += StoveCounter_OnStoveStateChange;
    }

    private void StoveCounter_OnStoveStateChange(object sender, StoveCounter.OnStoveStateChangeEventArgs e)
    {
        bool isActive = e.state == StoveCounter.State.FRYING || e.state == StoveCounter.State.FRIED;
        particles.SetActive(isActive);
        stoveHeat.SetActive(isActive);
    }
}
