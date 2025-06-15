using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CarObstacle : MonoBehaviour, IDamageable
{
    [Header("Stat")]
    [SerializeField] private int maxHp = 3;   // 차마다 다르게 지정 가능
    [SerializeField] private float speed = 4f;
    [SerializeField] private float killY = -10f;

    private int curHp;

    private void Awake() { curHp = maxHp; }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y < killY) Destroy(gameObject);
    }

    /*──── IDamageable 구현 ────*/
    public void TakeDamage(int dmg)
    {
        curHp -= dmg;
        if (curHp <= 0)
            Destroy(gameObject);              // TODO: 폭발 이펙트 등
    }
}
