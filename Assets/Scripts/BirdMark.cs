using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdMark : MonoBehaviour
{
    [SerializeField]
    private GameObject Eagle = null;

    private Image Red;

    private void Awake()
    {
        Red = GetComponent<Image>();
    }

    internal void RedInd()
    {
        StartCoroutine(RedIndCor());
    }

    private IEnumerator RedIndCor()
    {
    Reset:
        yield return new WaitForSeconds(0.01f);
        Red.fillAmount += 0.01f;
        if (Red.fillAmount < 1)
            goto Reset;

        Instantiate(Eagle, new Vector2(transform.position.x, transform.position.y - 1), Quaternion.identity);
        Destroy(gameObject);
    }
}
