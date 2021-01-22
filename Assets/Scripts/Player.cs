using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject CoinPrefab = null;
    [SerializeField]
    private GameObject CloudPrefab = null;
    [SerializeField]
    private GameObject PlanePrefab = null;
    [SerializeField]
    private GameObject EagleMarkPrefab = null;
    [SerializeField]
    private FinalCloud[] finalClouds = null;

    private Rigidbody2D rb;
    private float dirX;

    private MainUI mainUI = null;
    private Vector2 startPlayerPosition;
    private Vector2 startCameraPosition;
    private float controlSpeed = 7.5f;
    private float fallingSpeed = 3.0f;
    private bool isCanMove = true;
    private int distanse = 0;
    private float startPosY;
    private float currentPosY;
    private int currentSpawnedPoint;
    private float time;
    private int timeScore;
    private int timeUsed;
    private int lastTimeSpawned;
    private int HP = 3;
    private float coinOffPosX = 0.2f;
    private float planeSpawnRandomize = 50.0f;
    private float eagleSpanwTime = 10.0f;

    internal int coin = 1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainUI = FindObjectOfType<MainUI>();
        startPlayerPosition = transform.position;
        startCameraPosition = Camera.main.transform.position;
        startPosY = transform.position.y;
    }

    private void LateUpdate()
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y - startPlayerPosition.y + startCameraPosition.y, -10);
    }

    private void Update()
    {
        Generator();
        Movement();
        SpeedUp();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            mainUI.AddCoin(coin);
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "BlackCoin")
        {
            mainUI.AddBlackCoin();
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Cloud")
        {
            mainUI.CloudFade();
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Eagle" || collision.tag == "Plane")
        {
            HP -= 1;

            if (HP == 0)
            {
                foreach (var fc in finalClouds)
                {
                    fc.isCloudActive = true;
                }

                StartCoroutine(mainUI.ScoreElementsAppear());
                isCanMove = false;
            }
            
            mainUI.LoseHP(HP);
        }
    }

    private void SpeedUp()
    {
        time += Time.deltaTime;
        timeScore = (int)time;

        if (timeUsed != timeScore)
        {
            fallingSpeed += 0.02f;
            controlSpeed += 0.03f;
            timeUsed = timeScore;
        }
    }

    private void Movement()
    {
        if (isCanMove)
        {
            /*float h = Input.GetAxis("Horizontal");
            if (transform.position.x <= -2 && h < 0)
                h = 0;
            else if (transform.position.x >= 2 && h > 0)
                h = 0;

            transform.Translate(transform.right * h * Time.deltaTime * controlSpeed);
            transform.Translate(-transform.up * Time.deltaTime * fallingSpeed);*/

            dirX = Input.acceleration.x * controlSpeed;
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2.0f, 2.0f), transform.position.y);
            transform.Translate(-transform.up * Time.deltaTime * fallingSpeed);
        }
        else
        {
            dirX = 0;
        }
    }

    private void Generator()
    {
        currentPosY = transform.position.y;
        distanse = (int)currentPosY - (int)startPosY;
        
        if (Mathf.Abs(distanse) % 15 == 0 && Mathf.Abs(distanse) != currentSpawnedPoint && Mathf.Abs(distanse) != 0)
        {
            int randCombo = Random.Range(0, 5);
            if (randCombo == 0)
                SpawnCoinCombo1();
            else if (randCombo == 1)
                SpawnCoinCombo3();
            else if (randCombo == 2)
                SpawnCoinCombo4();
            else if (randCombo == 3)
                SpawnCoinCombo5();
            else if (randCombo == 4)
                SpawnCoinCombo6();

            currentSpawnedPoint = Mathf.Abs(distanse);
        }
        else if (Mathf.Abs(distanse) % 1 == 0 && Mathf.Abs(distanse) != currentSpawnedPoint && Mathf.Abs(distanse) != 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                SpawnCoinCombo2();

                currentSpawnedPoint = Mathf.Abs(distanse);
            }

            if(Random.Range(0, 25) == 0)
            {
                Instantiate(CloudPrefab, new Vector2(Random.Range(-2f, 2f), currentPosY - 8), Quaternion.identity);
            }

            if (Random.Range(0, (int)planeSpawnRandomize) == 0)
            {
                Instantiate(PlanePrefab, new Vector2(-3.3f, currentPosY - 8), Quaternion.identity);
            }

            if (eagleSpanwTime > 5.0f)
            {
                planeSpawnRandomize -= 0.02f;
                eagleSpanwTime -= 0.01f;
            }
        }

        if(timeScore % (int)eagleSpanwTime == 0 && timeScore != 0 && timeScore != lastTimeSpawned && isCanMove)
        {
            lastTimeSpawned = timeScore;
            GameObject mark = Instantiate(EagleMarkPrefab);
            mark.transform.position = new Vector3(transform.position.x, -4, 0);
            mark.transform.SetParent(GameObject.Find("Canvas").transform);
            mark.transform.localScale = Vector3.one;
            mark.transform.localPosition = new Vector3(mark.transform.localPosition.x, -671.81f, 0);
            mark.GetComponent<BirdMark>().RedInd();
        }
    }

    private void SpawnCoinCombo1()
    {
        // Left Up
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);

        // Right Up
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);

        // Left Down
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Right Down
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        
        int randDir = Random.Range(0, 2);
        if(randDir == 0)
        {
            Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);
        }
        else if(randDir == 1)
        {
            Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
            Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);
        }
    }

    private void SpawnCoinCombo2()
    {
        Instantiate(CoinPrefab, new Vector2(Random.Range(-2f, 2f), currentPosY - 8), Quaternion.identity);
    }

    private void SpawnCoinCombo3()
    {
        // Left Up
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);

        // Left Middle
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Right Middle
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);

        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);

        // Left Down
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.25f), Quaternion.identity);
    }

    private void SpawnCoinCombo4()
    {
        // Right Up
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);

        // Right Middle
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Left Middle
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);

        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);

        // Right Down
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.25f), Quaternion.identity);
    }

    private void SpawnCoinCombo5()
    {
        // Line 1
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Line 2
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);

        // Line 3
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);

        // Line 4
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);
    }

    private void SpawnCoinCombo6()
    {
        // Left Up
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Right Up
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 8.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 9.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.25f), Quaternion.identity);

        // Left Middle Up
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);

        // Right Middle Up
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 11), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 10.5f), Quaternion.identity);

        // Middle
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 12.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 13.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.5f), Quaternion.identity);

        // Left Middle Down
        Instantiate(CoinPrefab, new Vector2(0 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 14.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.5f), Quaternion.identity);

        // Right Middle Down
        Instantiate(CoinPrefab, new Vector2(0.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(0.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 15.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.25f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.5f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(1.75f + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.5f), Quaternion.identity);

        // Left Down
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(-2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 19), Quaternion.identity);

        // Right Down
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 16.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 17.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.25f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.5f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 18.75f), Quaternion.identity);
        Instantiate(CoinPrefab, new Vector2(2 + Random.Range(-coinOffPosX, coinOffPosX), currentPosY - 19), Quaternion.identity);
    }

}
