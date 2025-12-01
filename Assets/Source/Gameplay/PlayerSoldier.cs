using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSoldier : MonoBehaviour
{
    public delegate void DiedHandler();
    public event DiedHandler OnDied;

    public int Health = 2;
    public int MaxAmmo = 6;
    public int Ammo = 6;
    public bool IsReloading = false;
    public bool IsShooting = false;
    public float ReloadTime = 1f;
    public float RecoilTime = 0.2f;

    public HudUpdate Hud;
    public GameObject Bullet;
    public Transform GunBarrel;
    public Transform PlayerTransform;

    [SerializeField]
    private InputActionReference ReloadAction;

    [SerializeField]
    private InputActionReference AttackAction;

    // Update is called once per frame
    void Update()
    {
        if (AttackAction.action.WasPerformedThisFrame())
        {
            if (!IsShooting)
            {
                IsReloading = false;
                StartCoroutine("Shoot");
            }
        }

        if (ReloadAction.action.WasPerformedThisFrame() && !IsReloading)
        {
            StartCoroutine("Reload");
        }
    }

    public IEnumerator Shoot()
    {
        IsShooting = true;
        Fire();
        yield return new WaitForSeconds(RecoilTime);
        IsShooting = false;
    }

    // Reference
    // https://dev.to/sarvesh42/c-code-to-create-a-bullet-and-shoot-it-in-unity-3b9f
    public void Fire()
    {
        if (Ammo > 0 && !IsReloading)
        {
            Ammo -= 1;
            Hud.RemoveBullet();
            GameObject bullet = Instantiate(
                Bullet,
                GunBarrel.position + PlayerTransform.forward * 0.05f,
                PlayerTransform.rotation
            );
            bullet.GetComponent<Rigidbody>().linearVelocity = PlayerTransform.forward * 10f;
        }
    }

    public IEnumerator Reload()
    {
        IsReloading = true;
        do
        {
            yield return new WaitForSeconds(ReloadTime);
            if (!IsReloading)
            {
                break;
            }
            Ammo += 1;
            Hud.AddBullet();
        } while (Ammo < MaxAmmo);
        IsReloading = false;
    }

    public void HitByZombie()
    {
        Health -= 1;
        if (Health <= 0)
        {
            OnDied?.Invoke();
        }
    }
}
