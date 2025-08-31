using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private Animator animator;
    public Slider healthSlider;
    public UiManager uiManager;

    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.value = Health / MaxHealth;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (IsDead)
            return;

        base.OnDamage(damage, hitPoint, hitNormal);

        healthSlider.value = Health / MaxHealth;
        audioSource.PlayOneShot(hurtSound, SoundManager.Instance.sfxVolume);

        uiManager.ShowDamage();
    }

    protected override void Die()
    {
        base.Die();

        animator.SetTrigger("Die");
        healthSlider.fillRect.gameObject.SetActive(false);
        audioSource.PlayOneShot(deathSound, SoundManager.Instance.sfxVolume);
    }

    public void RestartLevel()
    {

    }

}
