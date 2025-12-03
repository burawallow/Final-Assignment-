using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemypoint : MonoBehaviour

        {
    private bool hasGivenPoint = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasGivenPoint && other.CompareTag("Player"))
        {
            ScoreManager.instance.AddPoint();
            hasGivenPoint = true;
        }
    }
}

