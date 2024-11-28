using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyHpBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider; // 체력바
    [SerializeField]
    private Canvas canvas; // Slider는 UI요소 이므로 Canvas 하위 오브젝트로 생성해야함.

    private RectTransform HpbarPos; // Slider는 RectTransform

    private int maxHP = 100; // 최대 피
    private int curHP = 100; // 현재 피

    private void Awake()
    {
        GameObject temp = Instantiate(Resources.Load("MyPrefab/HpBar")) as GameObject; // 체력바 생성
        slider = temp.GetComponent<Slider>(); // Slider 인스턴스 생성
        slider.transform.SetParent(canvas.transform); // Canvas를 부모객체로 설정
        HpbarPos = slider.GetComponent<RectTransform>(); // RectTransform 인스턴스 생성
    }
    private void Start()
    {
        slider.value = (float)curHP / (float)maxHP; // 체력바에 현자 체력 반영
    }
    private void FixedUpdate()
    {
        // 게임좌표와 Canvas좌표는 다름. -> Camera.main.WorldToScreenPoint 사용
        // 플레이어 머리 위에 체력바가 표시되게 함
        HpbarPos.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 1, 0));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet")) // 총알에 맞으면
        {
            Debug.Log("hit");
            curHP -= 10; // 체력 10 닳고
            slider.value = Mathf.Lerp(slider.value,(float)curHP / (float)maxHP,Time.deltaTime * 10); // 현재체력 업데이트
            // 이때, 부드러운 체력바 움직임을 위해 보간법을 사용함.
        }
    }
}
