using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewPlayer : PhysicsObject
{
    [Header("Attributes")]
    [SerializeField] private float attackDuration;
    public int attackPower = 25;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private float maxSpeed = 1;

    [Header("Inventory")]

    public int ammo;
    public int coinsCollected;
    public int health = 100;
    private int maxHealth = 100;

    [Header("References")]

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject attackBox;
    private Vector2 healthBarOrigSize;
    public Dictionary<string, Sprite> inventory = new Dictionary<string, Sprite>(); //Stores all inventory item strings and values
    public Sprite inventoryItemBlank; //default inventory item slot sprite
    public Sprite keyGemSprite; 
    public Sprite keySprite; 



    //Singleton instantation
    private static NewPlayer instance;
    public static NewPlayer Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
            return instance;
        }
    }

    private void Awake()
    {
        if (GameObject.Find("New Player")) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        gameObject.name = "New Player";

        healthBarOrigSize = GameManager.Instance.healthBar.rectTransform.sizeDelta;
        UpdateUI();

        SetSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, 0);


        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = jumpPower;
        }

        //Flip the players localScale.x if the move speed is greater than .01 or less than -.01
        if (targetVelocity.x < -.01)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        else if (targetVelocity.x > .01)
        {
            transform.localScale = new Vector2(1, 1);
        }

        // if we press "Fire1" set attackBox to true, else false
        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(ActivateAttack());
        }

        if (health <= 0)
        {
            Die();
        }

        animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetFloat("velocityY", velocity.y);
        animator.SetBool("grounded", grounded);
    }
    public IEnumerator ActivateAttack()
    {
        attackBox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackBox.SetActive(false);
    }

    //Update UI elements
    public void UpdateUI()
    {
        // if the healthBarOrigSize has not been set, match it to the healtBar rectTrasform size
        if (healthBarOrigSize == Vector2.zero) healthBarOrigSize = GameManager.Instance.healthBar.rectTransform.sizeDelta;
        GameManager.Instance.coinsText.text = coinsCollected.ToString();
        GameManager.Instance.healthBar.rectTransform.sizeDelta = new Vector2(healthBarOrigSize.x * ((float)health / (float)maxHealth), GameManager.Instance.healthBar.rectTransform.sizeDelta.y);
    }

    public void SetSpawnPosition()
    {
        transform.position = GameObject.Find("Spawn Location").transform.position;
    }

    public void Die()
    {
        SceneManager.LoadScene("SampleScene");
    }


    public void AddInventoryItem(string inventoryName, Sprite image)
    {
        inventory.Add(inventoryName, image);
        GameManager.Instance.inventoryItemImage.sprite = inventory[inventoryName];
    }

    public void RemoveInventoryItem(string inventoryName)
    {
        inventory.Remove(inventoryName);
        GameManager.Instance.inventoryItemImage.sprite = inventoryItemBlank;
    }
}