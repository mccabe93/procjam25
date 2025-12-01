using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Soldier,
    Zombie,
}

public class PlayerController : MonoBehaviour
{
    public PlayerState CurrentState = PlayerState.Soldier;
    public PlayerSoldier SoldierMode;
    private HashSet<EntityId> _awakeIds = new HashSet<EntityId>();

    private void Start()
    {
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
        foreach (GameObject zombie in zombies)
        {
            if (Vector3.Distance(zombie.transform.position, transform.position) < 2.0f)
            {
                zombie.GetComponentInChildren<ZombieAI>().Wakeup();
                _awakeIds.Add(zombie.GetEntityId());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CurrentState == PlayerState.Soldier && other.gameObject.CompareTag("Zombie"))
        {
            EntityId id = other.gameObject.GetEntityId();
            if (!_awakeIds.Contains(id))
            {
                other.gameObject.GetComponent<ZombieAI>().Wakeup();
                _awakeIds.Add(id);
            }
        }
        else if (CurrentState == PlayerState.Zombie && other.gameObject.CompareTag("Player")) { }
    }
}
