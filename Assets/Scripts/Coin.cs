using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void Start()
    {
        if(Random.Range(0, 50) == 0)
        {
            GetComponent<SpriteRenderer>().color = Color.black;
            tag = "BlackCoin";
        }
    }
}
