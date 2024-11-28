using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : MonoBehaviour
{
    private Rigidbody2D rbody2D;
    [SerializeField]
    private bool isGround, isJump, isDoubleJump;

    [SerializeField]
    private int Jumpcnt;

    public float Jumppower;
    public float Downpower;
    public float Speed;

    public float dir;

    [SerializeField]
    private Transform isGroundCheck;

    [SerializeField]
    private GameObject BulletPrefab;
    private GameObject Bullet;

    private SpriteRenderer sprite;

    private GameObject Weapon;
    private SpriteRenderer WeaponSprite;

    private GameObject _Grenade;

    private FireMode curMode;

    [SerializeField]
    private int GrenadeCnt, BulletCnt;

    private enum FireMode
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
        isGround = Physics2D.OverlapCircle(isGroundCheck.position, 0.05f, LayerMask.GetMask("Ground"));
        dir = Input.GetAxisRaw("Horizontal");

        if(dir > 0)
        {
            sprite.flipX = true;
            WeaponSprite.flipX = false;
        }
        else if(dir < 0)
        {
            sprite.flipX = false;
            WeaponSprite.flipX = true;
        }
        if(Input.GetKeyDown(KeyCode.Space) && (isGround || !isDoubleJump))
        {
            rbody2D.AddForce(new Vector2(0, Jumppower));
            if(!isDoubleJump || !isGround)
                isDoubleJump = true;
        }
        else if(!isGround)
        {
            rbody2D.AddForce(new Vector2(0, -Downpower));
        }
        if(Input.GetKeyDown(KeyCode.V) && BulletCnt != 0)
        {
            curMode = FireMode.Shot;
        }
        else if(Input.GetKeyDown(KeyCode.B) && GrenadeCnt != 0)
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
        rbody2D.velocity = new Vector2(dir * Speed, rbody2D.velocity.y);

        if(curMode == FireMode.Shot)
        {
            StartCoroutine(ShotMode());
        }
        if(curMode == FireMode.Grenade)
        {
            StartCoroutine(Grenade());
        }
        Weapon.transform.position = new Vector2(transform.position.x,transform.position.y);
    }
    private IEnumerator ShotMode()
    {
        Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        Rigidbody2D bulletrbody2D = Bullet.GetComponent<Rigidbody2D>();
        BulletCnt--;

        if(sprite.flipX == true)
        {
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

        yield return new WaitForSeconds(3f);
    }
    private IEnumerator Grenade()
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

        yield return new WaitForSeconds(3f);
    }
}
