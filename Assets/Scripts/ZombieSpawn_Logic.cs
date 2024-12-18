using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn_Logic : MonoBehaviour
{
    [SerializeField] private GameObject zombie;
    [SerializeField] private float timeBetweenSpawns = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cooldown());
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(5);
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        timeBetweenSpawns -= 0.05f;
        timeBetweenSpawns = Mathf.Max(timeBetweenSpawns, 0.5f);

        float x = Random.Range(-50, 50);
        float z = Random.Range(-50, 50);

        transform.position = new Vector3(x, 3, z);

        Instantiate(zombie, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenSpawns);
        StartCoroutine(Spawn());
    }
}
