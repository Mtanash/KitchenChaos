using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisualAnimation : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;

    private const string OPEN_CLOSE = "OpenClose";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnContainerCounterTriggered += ContainerCounter_OnContainerCounterTriggered;
    }

    private void ContainerCounter_OnContainerCounterTriggered(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
