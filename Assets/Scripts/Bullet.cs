using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed   = 15f;
    [SerializeField] private float lifeTime= 2f;
    [SerializeField] private int   damage  = 1;   // ← Inspector에서 조절

    private Vector2 dir;

    public void Init(Vector2 direction)
    {
        dir = direction.normalized; Invoke(nameof(SelfKill), lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 상대가 IDamageable 구현했으면 데미지 전달
        if (other.TryGetComponent(out IDamageable target))
            target.TakeDamage(damage);

        SelfKill(); // 총알은 1회 타격 후 파괴  
    }
    
    private void SelfKill() { Destroy(gameObject); }
}
