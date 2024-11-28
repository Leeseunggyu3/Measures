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
    private bool isGround, isDoubleJump;

    public float JumpPower = 10f; // 점프파워
    public float Speed = 5f; // 이동속도
    public float DownPower = 1f; // 추락속도

    private float dir; // 플레이어 방향 (좌, 우)

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
    private int GrenadeCnt = 3, BulletCnt = 10; // 슈루탄 개수, 총알 개수

    private enum FireMode // 플레이어 공격모드는 3가지로 정의한다. (DontShoot, Shot, Grenade)
    {
        DontShoot,
        Shot,
        Grenade
    }

    private void Awake()
    {
        rbody2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        Weapon = Instantiate(Resources.Load("MyPrefab/AK47") as GameObject, transform);
        WeaponSprite = Weapon.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 땅 감지
        isGround = Physics2D.OverlapCircle(isGroundCheck.position, 0.05f, LayerMask.GetMask("Ground"));

        // 방향 입력
        dir = Input.GetAxisRaw("Horizontal");

        // 방향에 따라 스프라이트 뒤집기
        if (dir > 0)
        {
            sprite.flipX = true;
            WeaponSprite.flipX = false;
        }
        else if (dir < 0)
        {
            sprite.flipX = false;
            WeaponSprite.flipX = true;
        }

        // 점프 입력 처리
        if (Input.GetKeyDown(KeyCode.Space) && (isGround || !isDoubleJump))
        {
            rbody2D.velocity = new Vector2(rbody2D.velocity.x, JumpPower);

            if (!isGround)
                isDoubleJump = true;
        }

        // 공격 모드 변경
        if (Input.GetKeyDown(KeyCode.V) && BulletCnt > 0)
        {
            curMode = FireMode.Shot;
        }
        else if (Input.GetKeyDown(KeyCode.B) && GrenadeCnt > 0)
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
        // 좌우 이동
        rbody2D.velocity = new Vector2(dir * Speed, rbody2D.velocity.y);

        // 땅에 있을 경우 이단 점프 해제
        if (isGround)
            isDoubleJump = false;

        // 공격 모드 실행
        if (curMode == FireMode.Shot)
        {
            StartCoroutine(ShotMode());
        }
        if (curMode == FireMode.Grenade)
        {
            StartCoroutine(Grenade());
        }

        // 무기 위치 갱신
        Weapon.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    private IEnumerator ShotMode()
    {
        Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        Rigidbody2D bulletRbody = Bullet.GetComponent<Rigidbody2D>();
        BulletCnt--;

        if (sprite.flipX)
        {
            Bullet.transform.position = new Vector2(transform.position.x + 2, transform.position.y);
            bulletRbody.velocity = new Vector2(30f, 0f);
            Weapon.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
        else
        {
            Bullet.transform.position = new Vector2(transform.position.x - 2, transform.position.y);
            bulletRbody.velocity = new Vector2(-30f, 0f);
            Weapon.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
        }

        yield return new WaitForSeconds(3f);
    }

    private IEnumerator Grenade()
    {
        _Grenade = Instantiate(Resources.Load("MyPrefab/Grenade") as GameObject);
        Rigidbody2D grenadeRbody = _Grenade.GetComponent<Rigidbody2D>();
        GrenadeCnt--;

        if (sprite.flipX)
        {
            _Grenade.transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
            grenadeRbody.velocity = new Vector2(15f, 5f);
        }
        else
        {
            _Grenade.transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
            grenadeRbody.velocity = new Vector2(-15f, 5f);
        }

        yield return new WaitForSeconds(3f);
    }
}
