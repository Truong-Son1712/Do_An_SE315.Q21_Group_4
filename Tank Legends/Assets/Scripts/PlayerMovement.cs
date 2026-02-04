using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    public Transform spriteTransform; // Transform của child object chứa sprite

    private void Start()
    {
        // Nếu chưa gán spriteTransform, tự động tìm trong child objects
        if (spriteTransform == null)
        {
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteTransform = spriteRenderer.transform;
            }
        }
        
        // Đảm bảo player transform luôn có rotation = 0
        transform.rotation = Quaternion.identity;
    }

    private void Update()
    {
        // Lấy input từ bàn phím
        float horizontal = 0f;
        float vertical = 0f;

        // Ưu tiên di chuyển ngang (chặn di chuyển chéo)
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }
        // Chỉ di chuyển dọc nếu không có input ngang
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                vertical = 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                vertical = -1f;
            }
        }

        // Quay sprite theo hướng di chuyển (chỉ dùng rotation, không dùng flip)
        if (spriteTransform != null)
        {
            if (horizontal < 0f) // Di chuyển trái
            {
                spriteTransform.rotation = Quaternion.Euler(180, 0, 180); // Quay 180 độ (trái)
            }
            else if (horizontal > 0f) // Di chuyển phải
            {
                spriteTransform.rotation = Quaternion.Euler(0, 0, 0); // Quay 0 độ (phải - mặc định)
            }
            else if (vertical > 0f) // Di chuyển lên
            {
                spriteTransform.rotation = Quaternion.Euler(0, 0, 90); // Quay 90 độ lên trên
            }
            else if (vertical < 0f) // Di chuyển xuống
            {
                spriteTransform.rotation = Quaternion.Euler(0, 0, -90); // Quay -90 độ xuống dưới
            }
        }

        // Tính toán vector di chuyển
        Vector2 movement = new Vector2(horizontal, vertical);

        // Di chuyển player
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
