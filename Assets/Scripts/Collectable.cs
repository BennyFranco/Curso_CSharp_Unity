using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    healthPotion,
    manaPotion,
    money
}

public class Collectable : MonoBehaviour
{
    public CollectableType type = CollectableType.money;
    public int value = 1;

    private SpriteRenderer sprite;
    private CircleCollider2D circleCollider;
    private bool hasBennCollected;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

    }

    void Show()
    {
        sprite.enabled = true;
        circleCollider.enabled = true;
        hasBennCollected = false;
    }

    void Hide()
    {
        sprite.enabled = false;
        circleCollider.enabled = false;
    }

    void Collect()
    {
        Hide();
        hasBennCollected = true;

        switch(type)
        {
            case CollectableType.money:
                GameManager.sharedInstance.CollectObject(this);
                break;
            case CollectableType.healthPotion:
                GameManager.sharedInstance.CollectObject(this);
                break;
            case CollectableType.manaPotion:
                GameManager.sharedInstance.CollectObject(this);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Collect();
        }
    }
}
