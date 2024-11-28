using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyHpBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Canvas canvas;

    private RectTransform HpbarPos;

    private int maxHP = 100;
    private int curHP = 100;

    private void Awake()
    {
        GameObject temp = Instantiate(Resources.Load("MyPrefab/HpBar")) as GameObject;
        slider = temp.GetComponent<Slider>();
        slider.transform.SetParent(canvas.transform);
        HpbarPos = slider.GetComponent<RectTransform>();
    }
    private void Start()
    {
        slider.value = (float)curHP / (float)maxHP;
    }
    private void FixedUpdate()
    {
        HpbarPos.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("hit");
            curHP -= 10;
            slider.value = Mathf.Lerp(slider.value,(float)curHP / (float)maxHP,Time.deltaTime * 10);
        }
    }
}
