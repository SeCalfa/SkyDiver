using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour
{

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        transform.Translate(transform.up * Time.deltaTime * 1.5f);
    }
}
