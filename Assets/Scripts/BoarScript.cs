using Unity.VisualScripting;
using UnityEngine;

public class BoarScript : MonoBehaviour
{
    /// <summary>
    /// Boar Enemy Script
    /// by Ross 
    /// </summary>

    private Vector3 targetPoint;
    public GameObject player;
    public float speed;
    public Rigidbody2D rb;
    public BoxCollider2D box;

    private float distance;
    private SpriteRenderer SpriteRenderer;
    private bool lockedOn;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
        
    
    public void Awake()
    {
        this.SpriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        this.SpriteRenderer.flipX = player.transform.position.x > this.transform.position.x;
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(this.transform.position,player.transform.position, speed * Time.deltaTime);
    }

   
}
