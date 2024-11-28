using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDestroyOnTime : MonoBehaviour
{
    private void Start()
    {
        Invoke("OnDestroy", 0.5f);
    }
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
