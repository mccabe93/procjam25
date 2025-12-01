using System.Collections.Generic;
using UnityEngine;

public class HudUpdate : MonoBehaviour
{
    public List<GameObject> Bullets;
    public GameObject CharacterIcon;
    public GameObject WeaponIcon;

    public Sprite ZombieImage;
    public Sprite SoldeirImage;
    public Sprite GunImage;
    public Sprite HandsImage;

    public void AddBullet()
    {
        var nextInactiveBullet = Bullets.Find(t => t.activeSelf == false);
        if (nextInactiveBullet != null)
        {
            nextInactiveBullet.SetActive(true);
        }
    }

    public void RemoveBullet()
    {
        var nextActiveBullet = Bullets.Find(t => t.activeSelf == true);
        if (nextActiveBullet != null)
        {
            nextActiveBullet.SetActive(false);
        }
    }

    public void RemoveAllBullets()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            RemoveBullet();
        }
    }

    public void AddAllBullets()
    {
        for (int i = 0; i < Bullets.Count; i++)
        {
            AddBullet();
        }
    }

    public void SwitchToZombie()
    {
        CharacterIcon.GetComponent<UnityEngine.UI.Image>().sprite = ZombieImage;
        WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = HandsImage;
        RemoveAllBullets();
    }

    public void SwitchToSoldier()
    {
        CharacterIcon.GetComponent<UnityEngine.UI.Image>().sprite = SoldeirImage;
        WeaponIcon.GetComponent<UnityEngine.UI.Image>().sprite = GunImage;
        AddAllBullets();
    }
}
