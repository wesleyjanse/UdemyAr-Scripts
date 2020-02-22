using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject ballpreFab;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2, 0.5f);
    }

    // Update is called once per frame
    void Spawn()
    {
        GameObject obj = Instantiate(ballpreFab, this.transform.position, Quaternion.identity);
        obj.transform.parent = this.transform;
    }
}
