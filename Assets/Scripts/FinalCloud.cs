using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCloud : MonoBehaviour
{

    internal bool isCloudActive = false;
    private float alpha = 0;
    [SerializeField]
    private float endLocX = 0;
    private float startLocX;

    private void Awake()
    {
        startLocX = transform.localPosition.x;
    }

    private void FixedUpdate()
    {
        if(isCloudActive && alpha < 1)
        {
            alpha += Time.fixedDeltaTime * 1.2f;
            transform.localPosition = Vector2.Lerp(new Vector2(startLocX, transform.localPosition.y), new Vector2(endLocX, transform.localPosition.y), alpha);
        }
    }
}
