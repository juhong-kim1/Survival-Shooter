using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public PlayerInput input;
    public Transform firePosition;

    public AudioSource audioSource;
    public AudioClip shootSound;

    public ParticleSystem gunParticles;

    private LineRenderer lineRenderer;

    public float shootTimer = 0f;
    public float shootInterval = 0.1f;
    public float fireDistance = 30f;

    public float damage = 10f;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    private void Update()
    {
        if (input.Fire)
        {
            Fire();
        }

    }

    private void Fire()
    {
        shootTimer += Time.deltaTime;

        if(shootTimer >= shootInterval)
        {
            Vector3 hitPosition = Vector3.zero;

            gunParticles.Play();

            RaycastHit hit;
            if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
            {
                hitPosition = hit.point;
                var target = hit.collider.GetComponent<IDamagable>();
                if (target != null)
                {
                    target.OnDamage(damage, hit.point, hit.normal);
                }
            }
            else
            {
                hitPosition = firePosition.position + firePosition.forward * fireDistance;
            }

            StartCoroutine(CoShotEffect(hitPosition));

            shootTimer = 0f;
        }
    }


    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        audioSource.PlayOneShot(shootSound, SoundManager.Instance.sfxVolume);

        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.15f);

        lineRenderer.enabled = false;
    }
}
