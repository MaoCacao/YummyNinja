using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class SwipeCutter : MonoBehaviour
{
    private GameManager gameManager;
    private Camera swipeCamera;
    private Vector3 mousePos;
    private TrailRenderer trail;
    private BoxCollider swipeCollider;
    private bool isSwiping = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        swipeCamera = Camera.main;
        trail = GetComponent<TrailRenderer>();
        swipeCollider = GetComponent<BoxCollider>();
        trail.enabled = false;
        swipeCollider.enabled = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                UpdateComponents();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isSwiping = false;
                UpdateComponents();
            }
            if (isSwiping)
            {
                UpdateMousePosition();
            }
        }
    }

    void UpdateMousePosition()
    {
        mousePos = swipeCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    void UpdateComponents()
    {
        trail.enabled = isSwiping;
        swipeCollider.enabled = isSwiping;
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Target>())
        {
            //Destroy the target
            collision.gameObject.GetComponent<Target>().DestroyTarget();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        Target target = other.GetComponent<Target>();
        if (target != null)
        {
            target.DestroyTarget();
        }
    }
}
