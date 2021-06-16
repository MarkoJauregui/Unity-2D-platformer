using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    [Header("Attributes")]
    [SerializeField] private int attackPower = 10;
    public int health = 100;
    private int maxHealth = 100;
    [SerializeField] private float maxSpeed;


    private int direction = 1;
    private RaycastHit2D rightLedgeRaycastHit;
    private RaycastHit2D rightWallRaycastHit;
    private RaycastHit2D leftLedgeRaycastHit;
    private RaycastHit2D leftWallRaycastHit;

    [Header("Raycast Properties")]

    [SerializeField] private LayerMask raycastLayerMask; // what layer do we want the raycast to interact with 
    [SerializeField] private Vector2 raycastOffset; 
    [SerializeField] private float raycastLength = 2;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        enemyMovement();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject == NewPlayer.Instance.gameObject)
        {
            //Hurt player and update UI
            NewPlayer.Instance.health -= attackPower;
            NewPlayer.Instance.UpdateUI();
        }
    }
    void enemyMovement()
    {
        targetVelocity = new Vector2(maxSpeed * direction, 0);

        //Check for right ledge
        rightLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x + raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down * raycastLength, Color.red);
        if (rightLedgeRaycastHit.collider == null) direction = -1;

        //Check for left ledge
        leftLedgeRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down, raycastLength);
        Debug.DrawRay(new Vector2(transform.position.x - raycastOffset.x, transform.position.y + raycastOffset.y), Vector2.down * raycastLength, Color.green);
        if (leftLedgeRaycastHit.collider == null) direction = 1;


        //check for right wall
        rightWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.right * raycastLength, Color.blue);
        if (rightWallRaycastHit.collider != null) direction = -1;

        //check for left wall
        leftWallRaycastHit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, raycastLength, raycastLayerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), Vector2.left * raycastLength, Color.yellow);
        if (leftWallRaycastHit.collider != null) direction = 1;
    }
}
