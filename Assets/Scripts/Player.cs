using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCoolTime = 1f;

    private float currentSpeed;
    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float nextDashTime = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        Dash();
    }

    private void PlayerMove()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        Vector3 moveTo = new Vector3(xInput, yInput, 0f);
        transform.position += moveTo * currentSpeed * Time.deltaTime;
    }

    private void Dash()
    {
        if (isDashing && Time.time >= dashEndTime)
        {
            // 대쉬 종료 후 원래 속도로 복귀
            isDashing = false;
            currentSpeed = moveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime)
        {
            // 대쉬 시작
            isDashing = true;
            currentSpeed = dashSpeed;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCoolTime;
        }
    }
}
