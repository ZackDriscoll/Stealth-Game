using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryRing : MonoBehaviour
{
    public GameObject VictoryBackground;
    public GameObject VictoryText;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            Destroy(player.gameObject);
            VictoryBackground.SetActive(true);
            VictoryText.SetActive(true);
        }
    }
}
