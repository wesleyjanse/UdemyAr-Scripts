using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour {

    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
        this.transform.Translate(0, 0, 0.1f);
	}
}
