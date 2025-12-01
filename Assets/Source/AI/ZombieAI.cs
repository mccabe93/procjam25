using System.Collections;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    public delegate void OnAwake();
    public event OnAwake AwakeEvent;

    private bool _isAwake = false;

    private GameObject _player;
    public Transform Self;

    // Update is called once per frame
    void Update()
    {
        if (_isAwake)
        {
            if (IsInAttackRange())
            {
                StartCoroutine("Attack");
            }
            else
            {
                Self.LookAt(_player.transform);
                Self.position = Vector3.MoveTowards(
                    Self.position,
                    _player.transform.position,
                    2.0f * Time.deltaTime
                );
            }
        }
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.25f);
        if (IsInAttackRange())
        {
            PlayerSoldier playerSoldier = _player.GetComponent<PlayerSoldier>();
            playerSoldier.HitByZombie();
        }
    }

    private bool IsInAttackRange()
    {
        return Vector3.Distance(_player.transform.position, Self.position) < 0.64f;
    }

    public void Wakeup()
    {
        _player = GameObject.FindWithTag("Player");
        if (!_isAwake)
        {
            _isAwake = true;
            AwakeEvent?.Invoke();
        }
    }
}
