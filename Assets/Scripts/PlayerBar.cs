using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BarType
{
    healthBar,
    manaBar
}

public class PlayerBar : MonoBehaviour
{
    public Slider slider;
    public BarType type;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        player = GameObject.Find("Player");
        switch (type)
        {
            case BarType.healthBar:
                slider.maxValue = PlayerController.MAX_HEALTH;
                break;
            case BarType.manaBar:
                slider.maxValue = PlayerController.MAX_MANA;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(type)
        {
            case BarType.healthBar:
                slider.value = player.GetComponent<PlayerController>().GetHealth();
                break;
            case BarType.manaBar:
                slider.value = player.GetComponent<PlayerController>().GetMana();
                break;
        }
    }
}
