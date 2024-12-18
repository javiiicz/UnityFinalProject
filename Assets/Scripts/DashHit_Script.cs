using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHit_Script : MonoBehaviour
{
    [SerializeField] private GameObject logic;
    [SerializeField] private GameObject tombstone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Instantiate(tombstone, other.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            logic.SendMessage("OnZombieKill");
        }
    }
}
