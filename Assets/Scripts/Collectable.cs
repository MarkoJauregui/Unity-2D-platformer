using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    enum ItemType { Coin, Health, Ammo, InventoryItem } //Creates an ItemType enum (drop down)
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private string inventoryStringName;
    [SerializeField] private ItemType itemType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Adds things to inventory system and updates UI
        if (collision.gameObject == NewPlayer.Instance.gameObject)
        {
            if (itemType == ItemType.Coin)
            {
                NewPlayer.Instance.coinsCollected += 1;
            }
            else if (itemType == ItemType.Health)
            {
                if (NewPlayer.Instance.health < 100)
                {
                    NewPlayer.Instance.health += 10;
                }
            }
            else if (itemType == ItemType.Ammo)
            {

            }
            else if (itemType == ItemType.InventoryItem)
            {
                NewPlayer.Instance.AddInventoryItem(inventoryStringName, inventorySprite);
            }

            else
            {

            }

            NewPlayer.Instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}