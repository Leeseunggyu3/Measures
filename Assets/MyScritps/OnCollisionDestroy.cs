using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDestroy : MonoBehaviour
{
    private void Start()
    {
        Invoke("desTroy", 1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        desTroy();
    }
    private void desTroy()
    {
        Destroy(gameObject);
    }
}
