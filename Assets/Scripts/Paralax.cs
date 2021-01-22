using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{

    private float startPos;
    private Camera cam;
    private MainMenu mainMenu;

    [SerializeField]
    private float paralaxEffect = 0;
    [SerializeField]
    private AnimationCurve curve = null;

    private void Start()
    {
        startPos = transform.position.x;
        cam = Camera.main;
        mainMenu = FindObjectOfType<MainMenu>();
    }

    private void FixedUpdate()
    {
        float dist = cam.transform.position.x + mainMenu.paralaxRange * paralaxEffect * curve.length;

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }

}
