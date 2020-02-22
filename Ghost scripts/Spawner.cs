using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject character;
    void Start()
    {
        InvokeRepeating("Spawn", 0, 4f);
    }
    void Spawn()
    {
        GameObject go = Instantiate(character, character.transform.position, this.transform.rotation);
        go.transform.Rotate(new Vector3(0, Random.Range(-10, 10),0 ));
    }
    void Update()
    {
        
    }
}
