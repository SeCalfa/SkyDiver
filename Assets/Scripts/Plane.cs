using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = Random.Range(0.8f, 2.5f);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(transform.right * Time.deltaTime * speed);
    }
}
