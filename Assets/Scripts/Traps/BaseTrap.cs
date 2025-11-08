using System;
using UnityEngine;

public abstract class BaseTrap : MonoBehaviour
{
    [SerializeField]
    protected float damage = 50;
    [SerializeField]
    private float knockbackForce = 20;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trap entered");
        if (other.CompareTag("Player"))
        {
            Effect(other);
        }
    }

    public abstract void Effect(Collider other);
}
