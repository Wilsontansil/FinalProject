using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float force = 5;

    [Header("Component Player")]
    Rigidbody2D rb;
    SpriteRenderer sr;

    [Header("Game Manager")]
    GameManagerColorDot GMColorDot;
    ScoreManager scoreManager;

    [Header("Game Effect")]
    [SerializeField] GameObject particleStar;
    [SerializeField] GameObject explosionParticle;
    [SerializeField] CameraShake cameraShake;

    [Header("Sound")]
    [SerializeField] AudioClip getPointClip;
    [SerializeField] AudioClip hitObstacleClip;
    [SerializeField] AudioClip changeColorClip;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        GMColorDot = FindObjectOfType<GameManagerColorDot>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.up * force;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("StarPoint"))
        {
            scoreManager.AddScore(10);
            GetPointSound();
            GMColorDot.instantiateObstacle();
            SpawnStarPoint(collision.transform.position);
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("Obstacle1"))
        {
            GMColorDot.instantiateObstacle();
            scoreManager.AddScore(5);
            collision.transform.tag = "DeadPoint";
        }
        else if(collision.CompareTag("ChangeColor"))
        {
            ChangeColorSound();
            GMColorDot.instantiatePoint();
            ChangeColorPoint();
            Destroy(collision.gameObject);
        }
        else if(collision.CompareTag("DeadPoint"))
        {
            HitObstacleSound();
            SpawnExplosion(transform.position);
            Destroy(gameObject);
            GMColorDot.GameOver();
        }
        else
        {
            if (sr.color != collision.GetComponent<SpriteRenderer>().color)
            {
                HitObstacleSound();
                SpawnExplosion(transform.position);
                Destroy(gameObject);
                GMColorDot.GameOver();
            }
            else
            {
                Debug.Log("Good");
            }
        }
    }

    void ChangeColorPoint()
    {
        int x = Random.Range(0, GMColorDot.ColorList.Count);
        while (sr.color == GMColorDot.ColorList[x])
        {
            x = Random.Range(0, GMColorDot.ColorList.Count);
        }
        //sr.color = GMColorDot.ColorList[x];
        LeanTween.color(gameObject, GMColorDot.ColorList[x], .2f);
    }

    void SpawnStarPoint(Vector2 position)
    {
        GameObject gb = Instantiate(particleStar);
        gb.transform.localScale = new Vector3(.5f, .5f, .5f);
        gb.transform.position = position;
        Destroy(gb, 2f);
    }

    void SpawnExplosion(Vector2 position)
    {
        StartCoroutine(cameraShake.Shake(.1f, .08f));
        GameObject gb = Instantiate(explosionParticle);
        var main = gb.GetComponent<ParticleSystem>().main;
        main.startColor = sr.color;
        gb.transform.localScale = Vector3.one;
        gb.transform.position = position;
        Destroy(gb, 2f);
    }

    void GetPointSound()
    {
        LeanAudio.play(getPointClip);
    }
    void HitObstacleSound()
    {
        LeanAudio.play(hitObstacleClip);
    }
    void ChangeColorSound()
    {
        LeanAudio.play(changeColorClip);
    }
}
