﻿using UnityEngine;
using System.Collections;

public class CameraController_m : MonoBehaviour 
{
	public Transform Player;
	public float m_speed = 0.1f;
	Camera mycam;

	public void Start()
	{
		mycam = GetComponent<Camera> ();
	}

	public void Update()
	{

		mycam.orthographicSize = (Screen.height / 100f) / 0.8f;

		if (Player) 
		{
		
			transform.position = Vector3.Lerp(transform.position, Player.position, m_speed);
		}


	}
}
