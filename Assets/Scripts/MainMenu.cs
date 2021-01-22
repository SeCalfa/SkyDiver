using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private float direction = 1;

    public float paralaxRange { get; private set; } = 0;

    private void FixedUpdate()
    {
        paralaxRange += Time.fixedDeltaTime * 0.7f * direction;

        if (paralaxRange >= 1)
            direction = -1;
        else if (paralaxRange <= -1)
            direction = 1;
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("MyGame");
    }

}
