using UnityEngine;

public class Trap : BaseTrap
{
    public override void Effect(Collider other)
    {
        PlayerHealthController.instance.TakeDamage(damage);
    }
}