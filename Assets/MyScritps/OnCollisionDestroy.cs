using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionDestroy : MonoBehaviour
{
    private void Start()
    {
        Invoke("desTroy", 1f); // 1초뒤 총알 제거
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        desTroy(); // 어느 물체든 닿이면 총알 제거
    }
    private void desTroy()
    {
        Destroy(gameObject);
    }
}
