using UnityEngine;

public class DotRunController : MonoBehaviour
{
    public static int point;
    public static int score;
    public float movingStatus;
    public GameObject explosionParticle;
    public GameObject getPointParticle;
    [HideInInspector]public SpriteRenderer sr;
    ParticleSystem particle = new ParticleSystem();
    ParticleSystem.MainModule main;
    GameManagerDotRun GM;
    bool canMove=false;


    [Header("Sound Setting")]
    [SerializeField] AudioClip clipExplosion;
    [SerializeField] AudioClip clipSoundHit;
    [SerializeField] AudioClip clipSwoosh;
    [SerializeField] AudioClip clipGetPoint;
    private void Awake()
    {
        GM = FindObjectOfType<GameManagerDotRun>();
        sr = GetComponent<SpriteRenderer>();
        particle = GetComponentInChildren<ParticleSystem>();
        main = particle.main;
        point = 0;
        score = 0;
        movingStatus = 2;
        canMove = false;
    }
    private void Start()
    {
        ChangeColor(GM.colorGame[Random.Range(0, GM.colorGame.Count)]);
        MoveDotStart();
    }
    void MoveDotStart()
    {
        LeanAudio.play(clipSwoosh).PlayDelayed(.3f);
        LeanTween.moveY(gameObject, -1.5f, 1f).setEase(LeanTweenType.easeOutCubic).setOnComplete(()=>canMove = true);
    }
    private void Update()
    {
        if (GM.gameState == GameStateDotRun.inPlayGame)
        {
            if (Input.GetMouseButton(0))
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, transform.position.y, transform.position.z), Time.deltaTime * movingStatus);

            }
        }


    }

    private void LateUpdate()
    {
        if (GM.gameState == GameStateDotRun.inPlayGame)
        {
            if (canMove)
            {
                // get the current position
                Vector3 clampedPosition = transform.position;
                // limit the x and y positions to be between the area's min and max x and y.
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, -1.6f, 1.6f);
                clampedPosition.y = Mathf.Clamp(clampedPosition.y, -1.5f, -1.5f);
                // z remains unchanged
                // apply the clamped position
                transform.position = clampedPosition;
            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("DotRunObstacle"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().color != sr.color)
            {
                if (point>0)
                {
                    point--;
                }
                else
                {
                    GM.gameState = GameStateDotRun.inGameOver;
                    GM.GameOver();
                    LeanAudio.play(clipExplosion);
                    InstantiateExplosion();
                    Destroy(gameObject);
                }
            }
            else
            {
                point++;
                score++;
            }
            //Destroy(collision.gameObject);
            LeanTween.scale(collision.gameObject, new Vector3(.2f, .25f + .002f), .01f);
            Destroy(collision.gameObject, .01f);
            LeanAudio.play(clipSoundHit);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("DotRunChangeColor"))
        {
            ChangeColor(collision.gameObject.GetComponent<SpriteRenderer>().color);
            InstantiatePointEffecrt(collision.transform, collision.gameObject.GetComponent<SpriteRenderer>().color);
            LeanAudio.play(clipGetPoint);
            Destroy(collision.gameObject);
        }
    }
    void InstantiatePointEffecrt(Transform pos,Color c)
    {
        GameObject gb = Instantiate(getPointParticle);
        gb.transform.position = pos.position;
        gb.transform.localScale = Vector3.one;
        ParticleSystem ps = gb.GetComponent<ParticleSystem>();
        ParticleSystem.MainModule md;
        md = ps.main;
        md.startColor = c;
        Destroy(gb,3f);

    }
    void ChangeColor(Color color)
    {
        sr.color = color;
        main.startColor = color;
    }

    void InstantiateExplosion()
    {
        GameObject gb = Instantiate(explosionParticle);
        gb.transform.position = gameObject.transform.position;
        gb.transform.localScale = Vector3.one;
        Destroy(gb, 2f);
    }
}
