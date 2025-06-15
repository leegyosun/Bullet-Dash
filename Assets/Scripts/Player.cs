using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move / Dash")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolTime = 1f;

    [Header("Life / Death")]
    [SerializeField] private int maxHp = 1;   // 1이면 즉사, 3 등으로 늘리면 체력제
    private int curHp;

    private float currentSpeed;
    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float nextDashTime = 0f;

    /* === 화면 경계용 === */
    private Camera mainCam;
    private float halfW, halfH;
    /* =================== */

    void Awake()
    {
        mainCam = Camera.main;
        curHp = maxHp;

        var sr = GetComponent<SpriteRenderer>();
        halfW = sr.bounds.extents.x;
        halfH = sr.bounds.extents.y;
    }

    void Start() => currentSpeed = moveSpeed;

    void Update()
    {
        PlayerMove();
        Dash();
        LookAtMouse();          
    }

    /*──────────────────────────────── 클램프 이동 ───────────────────────────────*/
    private void PlayerMove()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 delta = new Vector3(x, y) * currentSpeed * Time.deltaTime;
        Vector3 newPos = transform.position + delta;

        Vector3 bl = mainCam.ViewportToWorldPoint(Vector3.zero);
        Vector3 tr = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        newPos.x = Mathf.Clamp(newPos.x, bl.x + halfW, tr.x - halfW);
        newPos.y = Mathf.Clamp(newPos.y, bl.y + halfH, tr.y - halfH);

        transform.position = newPos;
    }

    /*──────────────────────────────── 대  시 ───────────────────────────────*/
    private void Dash()
    {
        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
            currentSpeed = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime)
        {
            isDashing = true;
            currentSpeed = dashSpeed;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCoolTime;
        }
    }

    /*─────────────────────────────  마우스 바라보기  ─────────────────────────*/
    private void LookAtMouse()
    {
        // 1) 마우스 위치를 월드 좌표로
        Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;                          // 2D이므로 z=0 고정

        // 2) 방향 벡터 (목표 - 나)
        Vector2 dir = mouseWorld - transform.position;

        // 3) 각도 계산 (라디안 → °)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;

        // 4) 스프라이트의 “기본 바라보는 방향”이 오른쪽(+X)라면 그대로,
        //    위쪽(+Y)이면 -90° 등 보정값을 더해 줌.
        transform.rotation = Quaternion.Euler(0, 0, angle);
        // 예) 기본이 위(+Y)라면: Quaternion.Euler(0,0, angle - 90f);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        // 차 프리팹에 Tag = "Obstacle" 이라면
        if (col.collider.CompareTag("Obstacle"))
        {
            TakeDamage(1);          // 즉사라면 1, 체력제가 필요하면 원하는 값
        }
    }
    private void TakeDamage(int dmg)
    {
        curHp -= dmg;
        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO: 게임오버 UI, 리스폰, 사운드 등
        Debug.Log("플레이어 사망!");
        Destroy(gameObject);      // 당장은 오브젝트 삭제로 끝
    }
}
