using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        speed = Random.Range(0.1f, 0.25f);
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(transform.up * Time.deltaTime * speed);
    }
}
