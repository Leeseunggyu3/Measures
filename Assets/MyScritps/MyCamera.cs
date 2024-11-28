using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public Transform Player;
    private Camera myCam;

    private void Awake()
    {
        myCam = GetComponent<Camera>();
    }
    private void Update()
    {
        myCam.orthographicSize = (Screen.height / 100) * 0.7f;
        transform.position = Vector3.Lerp(transform.position, new Vector3(Player.position.x, Player.position.y, -10), 0.2f);
    }
}
