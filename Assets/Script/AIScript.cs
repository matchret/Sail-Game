using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIScript : MonoBehaviour
{
    public float rotationSpeed = 5f;
    private GameObject closestCoin;


     void FindClosestCoin()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        float closestDistance = Mathf.Infinity;
        GameObject nearestCoin = null;

        foreach (GameObject coin in coins)
        {
            float distanceToCoin = Vector3.Distance(transform.position, coin.transform.position);
            if (distanceToCoin < closestDistance)
            {
                closestDistance = distanceToCoin;
                nearestCoin = coin;
            }
        }

        closestCoin = nearestCoin;
    }

    public void RotateTowardsCoin()
    {
        FindClosestCoin();
        // Get direction to the closest coin
        Vector3 direction = (closestCoin.transform.position - transform.position).normalized;

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate toward the coin
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
