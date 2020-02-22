using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eat : MonoBehaviour
{
    GameObject food;
    Vector3 goalPos;

    float speed = 0.5f;
    float accuracy = 0.25f;
    float rotSpeed = 2f;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (food == null)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("pizza");
            if (gos.Length > 0)
            {
                food = gos[0];
                goalPos = food.transform.position;
            }
        }
    }

    void RemoveFood()
    {
        Destroy(food);
    }

    private void LateUpdate()
    {
        Vector3 lookAtGoal = new Vector3(goalPos.x, this.transform.position.y, goalPos.z);
        Vector3 direction = lookAtGoal - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);

        if (Vector3.Distance(transform.position, lookAtGoal) > accuracy)
        {
            this.transform.Translate(0, 0, speed * Time.deltaTime);
            anim.SetBool("isRunning", true);
            anim.SetFloat("speed", 1.0f);
        } else
        {
            anim.SetBool("isRunning", false);
            if (food != null)
            {
                anim.SetBool("isEating", true);
                Destroy(food, 4);
            }
        }

        if (food == null)
        {
            anim.SetBool("isEating", false);
        }
    }
}
