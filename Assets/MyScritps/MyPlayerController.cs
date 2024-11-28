using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ### 플레이어 이동, 공격, 점프 구현한 소스코드
/// </summary>
public class MyPlayerController : MonoBehaviour
{
    private Rigidbody2D rbody2D;

    // 각 상황에 따른 플래그 변수
    [SerializeField]
    private bool isGround, isJump, isDoubleJump;

    public float Jumppower; // 점프파워
    public float Downpower; // 추락속도
    public float Speed; // 이동속도

    public float dir; // 플레이어 방향 (좌, 우)

    [SerializeField]
    private Transform isGroundCheck; // 땅 감지를 위한 빈오브젝트

    [SerializeField]
    private GameObject BulletPrefab; // 총알 프리팹
    private GameObject Bullet; // 총알 생성 시 값을 담을 인스턴스 변수

    private SpriteRenderer sprite; // 캐릭터 스프라이트 인스턴스 변수

    private GameObject Weapon; // 무기 생성 시 값을 담을 인스턴스 변수
    private SpriteRenderer WeaponSprite; // 무기 스프라이트 인스턴스 변수

    private GameObject _Grenade; // 슈루탄 인스턴스 변수

    private FireMode curMode; // 현재 플레이어 공격모드

    [SerializeField]
    private int GrenadeCnt, BulletCnt; // 슈루탄 개수, 총알 개수

    private enum FireMode // 플레이어 공격모드는 3가지로 정의한다. (DontShoot, Shot, Grenade)
    {
        DontShoot, // DontShoot : 플레이어가 총을 쏘지 않는 상태
        Shot, // Shot : 플레이어가 총을 쏘는 상태
        Grenade // Grenade : 플레이어가 슈루탄을 던지는 상태
    }
    private void Awake() // 각 인스턴스 생성 및 무기 생성
    {
        rbody2D = GetComponent<Rigidbody2D>(); 
        sprite = GetComponent<SpriteRenderer>();
        Weapon = Instantiate(Resources.Load("MyPrefab/AK47") as GameObject, transform);

        WeaponSprite = Weapon.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        // 매프레임 땅 감지여부를 판단함.
        // 1. 플레이어 발쪽에 빈 오브젝트를 생성
        // 2. 빈 오브젝트에서 원형모양의 레이 생성
        // 3. 땅감지
        isGround = Physics2D.OverlapCircle(isGroundCheck.position, 0.05f, LayerMask.GetMask("Ground"));

        // dir = -1, 0, 1 (-1 : 왼쪽, 0 : 가만히, 1 : 오른쪽)
        dir = Input.GetAxisRaw("Horizontal");

        if(dir > 0) // 플레이어가 오른쪽을 본다면
        {
            sprite.flipX = true; // 이미지 반전
            WeaponSprite.flipX = false; // 무기 이미지 반전
        }
        else if(dir < 0)
        {
            sprite.flipX = false;
            WeaponSprite.flipX = true;
        }
        // 점프 키가 눌리고 캐릭터가 땅에 닿아있거나, 점프키가 눌리고 이단점프 플래그값에 따라서
        if(Input.GetKeyDown(KeyCode.Space) && (isGround || !isDoubleJump))
        {
            rbody2D.AddForce(new Vector2(0, Jumppower)); // 점프 수행
            if(!isDoubleJump || !isGround) // 한번 점프 했으니까 
                isDoubleJump = true; // 이단점프 가능

            // FixedUpdate()에 isDoubleJump = false가 존재함
            // Update()와 FixedUpdate() 반복 차이로 인해 isDoubleJump 플래그값 업데이트 속도가 늦음
            // 따라서 이단점프 가능
        }
        else if(!isGround) // 캐릭터가 공중에 떠 있다면
        {
            rbody2D.AddForce(new Vector2(0, -Downpower)); // 추락시킴
        }
        if(Input.GetKeyDown(KeyCode.V) && BulletCnt != 0) // 총 사격
        {
            curMode = FireMode.Shot;
        }
        else if(Input.GetKeyDown(KeyCode.B) && GrenadeCnt != 0) // 슈루탄 던짐
        {
            curMode = FireMode.Grenade;
        }
        else
        {
            curMode = FireMode.DontShoot;
        }
    }
    private void FixedUpdate()
    {
        if(isGround)
            isDoubleJump = false;
        rbody2D.velocity = new Vector2(dir * Speed, rbody2D.velocity.y); // 캐릭터 좌우 이동

        if(curMode == FireMode.Shot) // 현재 공격모드가 총 사격이면
        {
            StartCoroutine(ShotMode()); // 코루틴 시작
        }
        if(curMode == FireMode.Grenade)
        {
            StartCoroutine(Grenade());
        }
        Weapon.transform.position = new Vector2(transform.position.x,transform.position.y); // 무기가 캐릭터 계속 따라다니게.
    }
    private IEnumerator ShotMode()
    {
        Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation); // 총알 생성
        Rigidbody2D bulletrbody2D = Bullet.GetComponent<Rigidbody2D>(); // 총알의 리지드바디값 접근
        BulletCnt--; // 총알 한번 쐈으므로 유행탄수 -1

        if(sprite.flipX == true) // 캐릭터 이미지 좌우 반전값에 따라...
        {
            // 총알이 날라가는 방향, 생성위치가 달라짐
            Bullet.transform.position = new Vector2(transform.position.x + 2,transform.position.y);
            bulletrbody2D.AddForce(new Vector2(30,0),ForceMode2D.Impulse);
            Weapon.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else
        {
            Bullet.transform.position = new Vector2(transform.position.x - 2,transform.position.y);
            bulletrbody2D.AddForce(new Vector2(-30,0),ForceMode2D.Impulse);
            Weapon.transform.position = new Vector2(transform.position.x - 1,transform.position.y);
        }

        yield return new WaitForSeconds(3f); // 3초만 실행하고 코루틴 정지
    }
    private IEnumerator Grenade() // 위의 ShotMode()와 구조같음
    {
        _Grenade = Instantiate(Resources.Load("MyPrefab/Grenade") as GameObject);
        Rigidbody2D grenaderbody2D = _Grenade.GetComponent<Rigidbody2D>();
        GrenadeCnt--;

        if(sprite.flipX == true)
        {
            _Grenade.transform.position = new Vector3(transform.position.x + 1,transform.position.y,0);
            if(dir != 0)
                grenaderbody2D.AddForce(new Vector2(10 * 1.5f,3 * 1.5f),ForceMode2D.Impulse);
            else
                grenaderbody2D.AddForce(new Vector2(10,3),ForceMode2D.Impulse);
        }
        else
        {
            _Grenade.transform.position = new Vector3(transform.position.x - 1,transform.position.y,0);
            if(dir != 0)
                grenaderbody2D.AddForce(new Vector2(-10 * 1.5f,3 * 1.5f),ForceMode2D.Impulse);
            else
                grenaderbody2D.AddForce(new Vector2(-10,3),ForceMode2D.Impulse);
        }

        yield return new WaitForSeconds(3f); // 3초만 실행하고 코루틴 정지
    }
}
