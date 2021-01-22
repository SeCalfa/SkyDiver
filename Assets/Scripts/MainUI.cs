using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    private Player player;

    [SerializeField]
    private TextMeshProUGUI coinText = null;
    [SerializeField]
    private TextMeshProUGUI blackCoinText = null;
    [SerializeField]
    private TextMeshProUGUI hpText = null;
    [SerializeField]
    private Image fade = null;
    [SerializeField]
    private Image cloudDetector = null;
    [SerializeField]
    private GameObject[] ScoreElements = null;
    [SerializeField]
    private GameObject OK = null;

    private int yellowCoin = 0;
    private int blackCoin = 0;
    private int score = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    internal void AddCoin(int coin)
    {
        yellowCoin += coin;
        coinText.text = yellowCoin.ToString();
    }

    internal void AddBlackCoin()
    {
        blackCoin += 1;
        blackCoinText.text = blackCoin.ToString();
    }

    internal void LoseHP(int hp)
    {
        hpText.text = hp.ToString();
    }

    internal void CloudFade()
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
        cloudDetector.fillAmount = 1;
        StopAllCoroutines();
        StartCoroutine(WhiteFade());
        StartCoroutine(CloudDet());
    }

    internal IEnumerator ScoreElementsAppear()
    {
        float num = 0;
        score = yellowCoin - (blackCoin * 20);

        ScoreElements[0].GetComponent<TextMeshProUGUI>().text += yellowCoin.ToString();
        ScoreElements[1].GetComponent<TextMeshProUGUI>().text += blackCoin.ToString();
        ScoreElements[2].GetComponent<TextMeshProUGUI>().text += score.ToString();

        foreach (var s in ScoreElements)
        {
            s.SetActive(true);
        }
        Reset:
        yield return new WaitForSeconds(0.01f);
        foreach (var s in ScoreElements)
        {
            try
            {
                s.GetComponent<Image>().color = new Color(s.GetComponent<Image>().color.r, s.GetComponent<Image>().color.g, s.GetComponent<Image>().color.b, s.GetComponent<Image>().color.a + 0.01f);
            }
            catch
            {
                s.GetComponent<TextMeshProUGUI>().color = new Color(s.GetComponent<TextMeshProUGUI>().color.r, s.GetComponent<TextMeshProUGUI>().color.g, s.GetComponent<TextMeshProUGUI>().color.b, s.GetComponent<TextMeshProUGUI>().color.a + 0.01f);
            }
        }
        num += 0.01f;
        if (num < 1)
            goto Reset;

        OK.SetActive(true);
        if (PlayerPrefs.GetInt("Record") <= score)
            PlayerPrefs.SetInt("Record", score);
    }

    private IEnumerator WhiteFade()
    {
        Reset:
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - 0.01f);
        yield return new WaitForSeconds(0.02f);
        if (fade.color.a > 0)
            goto Reset;
    }

    private IEnumerator CloudDet()
    {
        player.coin = 2;
        Reset:
        yield return new WaitForSeconds(0.05f);
        cloudDetector.fillAmount -= 0.01f;
        if (cloudDetector.fillAmount > 0)
            goto Reset;
        player.coin = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
