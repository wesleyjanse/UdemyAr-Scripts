using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    float speed = 0.5f;
    float turnSpeed = 1.0f;
    public bool dead = false;
    GameObject home;
    Animator anim;
    bool lookingForHome = true;

	// Use this for initialization
	void Start () {
        InvokeRepeating("FindHome", 0, 1);
        anim = this.GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "bullet") {
            Hit();
        } else if (col.gameObject.tag == "home") {
            Destroy(this.gameObject, 1);
            this.GetComponent<AudioSource>().Play();
        }
    }

    private void Hit() {
        dead = true;
        anim.SetTrigger("IsDying");
        Destroy(this.GetComponent<Collider>(), 1.0f);
        Destroy(this.GetComponent<Rigidbody>(), 1.0f);
        Destroy(this.gameObject, 4.0f);
    }

    void FindHome() {
        home = GameObject.FindWithTag("home");
        if (home) {
            CancelInvoke("FindHome");
            lookingForHome = true;
        }
    }


    // Update is called once per frame
    void Update() {

        if (dead) return;

        if (home) {
            Vector3 direction = home.transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(direction),
                turnSpeed * Time.smoothDeltaTime);
        } else if (lookingForHome) {
            InvokeRepeating("FindHome", 0.0f, 1.0f);
            lookingForHome = false;
        }
        this.transform.Translate(0, 0, speed * Time.deltaTime);
	}
}
