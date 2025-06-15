/* AutoShooter.cs ─ 플레이어에 붙여서 자동 발사 */
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 8f;      // 초당 발사수
    [SerializeField] private Transform muzzle;         // 총구 위치

    private Camera cam;
    private float  nextFireTime;

    void Awake() => cam = Camera.main;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void Fire()
    {
        // 1) 총구 위치 + 회전
        if (muzzle == null) muzzle = this.transform;

        // 2) 마우스 방향 벡터
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector2 dir = (mouseWorld - muzzle.position).normalized;

        // 3) 총알 생성 & 초기화
        GameObject go = Instantiate(bulletPrefab, muzzle.position, Quaternion.identity);
        go.GetComponent<Bullet>().Init(dir);
    }
}
