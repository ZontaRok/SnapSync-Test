using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("GeneratedLine")) // Assuming the generated lines are tagged with "GeneratedLine"
        {
            // Handle collision with generated lines (e.g., decrease health, trigger animation)
            Debug.Log("Player collided with a generated line!");
        }
    }
}
