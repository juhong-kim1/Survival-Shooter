using UnityEngine;
using UnityEngine.Audio;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);

    }

    protected override void Die()
    {
        base.Die();

        animator.SetTrigger("Die");
    }

}
