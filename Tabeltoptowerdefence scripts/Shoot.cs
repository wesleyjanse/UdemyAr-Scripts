using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public GameObject bullet;
    public GameObject spawnPos;
    GameObject goob;
    float turnSpeed = 1.0f;

    private void OnTriggerEnter(Collider obj) {
        if(obj.gameObject.tag == "goober"  && goob == null) {
            goob = obj.gameObject;
            InvokeRepeating("ShootBullet", 0.0f, 1.0f);
        }
    }

    // Use this for initialization
    void Start () {
        // InvokeRepeating("ShootBullet", 1.0f, 1.0f);
	}

    void ShootBullet() {
        Instantiate(bullet, spawnPos.transform.position, spawnPos.transform.rotation);
        this.GetComponent<AudioSource>().Play();
        if (goob.GetComponent<Move>().dead) {
            goob = null;
            CancelInvoke("ShootBullet");
        }
    }

    private void OnTriggerExit(Collider obj) {
        if(obj.gameObject == goob) {
            goob = null;
            CancelInvoke("ShootBullet");
        }
    }

    // Update is called once per frame
    void Update () {
        if (goob) {
            Vector3 direction = goob.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(direction),
                turnSpeed * Time.smoothDeltaTime);
        }
	}
}
