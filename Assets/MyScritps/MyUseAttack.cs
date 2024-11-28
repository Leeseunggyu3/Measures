using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUseAttack : MonoBehaviour
{
    private GameObject BulletPrefab;
    private GameObject Bullet;

    [SerializeField]
    private Transform TargetPos;

    private FireMode curMode;

    private enum FireMode
    {
        Shot,
        Volley
    }
    private void Awake()
    {
        BulletPrefab = Resources.Load("Prefab/Bullet") as GameObject;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            curMode = FireMode.Shot;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            curMode = FireMode.Volley;
        }
    }
    private IEnumerator ShotMode()
    {
        Bullet = Instantiate(BulletPrefab, transform.position, transform.rotation);
        Rigidbody2D Bulletrbody2D = Bullet.GetComponent<Rigidbody2D>();
        Bullet.transform.position = new Vector3(TargetPos.position.x + 1, TargetPos.position.y, 0);

        Bulletrbody2D.AddForce(new Vector2(4f, 0));
        yield return new WaitForSeconds(2f);
    }
    private IEnumerator VolleyMode()
    {
        while(true)
        {

            yield return new WaitForSeconds(2f);
        }
    }
}
