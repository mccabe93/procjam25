using Unity.VisualScripting;
using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public Animator Animator;
    public int Health = 10;
    public AudioClip OnHitSound;

    public ZombieAI ZombieAi;
    private FlashVFX _flashVfx;
    private FadeVFX _fadeVfx;

    void Awake()
    {
        ZombieAi.AwakeEvent += OnZombieAwake;
        _fadeVfx = this.AddComponent<FadeVFX>();
        _fadeVfx.Duration = 3.0f;
        _fadeVfx.Parent = gameObject;
        _flashVfx = this.AddComponent<FlashVFX>();
        _flashVfx.Color = Color.red;
        _flashVfx.Duration = 1.0f;
    }

    private void OnZombieAwake()
    {
        Animator.Play("zombie-walk");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet_Player")
        {
            AudioSource.PlayClipAtPoint(OnHitSound, transform.position, 0.1f);
            Health -= 5;
            if (Health <= 0 && !_fadeVfx.IsFading)
            {
                Animator.Play("zombie-dead");
                Destroy(ZombieAi);
                _fadeVfx.BeginFX();
            }
            else if (!_fadeVfx.IsFading && !_flashVfx.IsFlashing)
            {
                _flashVfx.BeginFX();
            }
        }
    }
}
