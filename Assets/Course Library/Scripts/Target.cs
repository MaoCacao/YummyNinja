using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12;
    private float maxSpeed = 14;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -1;
    private GameManager gameManager;
    public int pointValue;
    public ParticleSystem explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse); // for rotation
        transform.position = RandomSpawnPos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnMouseDown()
    {
        if (gameManager.isGameActive) {
        gameManager.UpdateScore(pointValue);
        Destroy(gameObject);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        if (gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
        }
    }*/

    public void DestroyTarget()
    {
        Debug.Log("Destroy target ");
        if (gameManager.isGameActive)
        {
            gameManager.UpdateScore(pointValue);
            Destroy(gameObject);
            Instantiate(explosion, transform.position, explosion.transform.rotation);
            if (gameObject.CompareTag("Bad"))
            {
                gameManager.GameOver();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Sensor"))
        {
            return;
        }

        if (gameManager.lives > 0)
        {
            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.UpdateLives(gameManager.lives - 1);
            }
        }
        else
        {
            gameManager.GameOver();
        }
        Destroy(gameObject);
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
