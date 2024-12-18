using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone_Script : MonoBehaviour
{
    private MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        float rotation = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(-90f, rotation, 0f);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;

        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(0.5f);
        renderer.enabled = true;
        yield return new WaitForSeconds(9.5f);
        Destroy(this.gameObject);
    }
}
