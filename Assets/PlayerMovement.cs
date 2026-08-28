using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private float horizontalInput;

    // 元のサイズを保存しておくための変数
    private Vector3 defaultScale;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // ゲーム開始時のインスペクターのScaleの値を記憶する
        defaultScale = transform.localScale;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            float left = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? -1f : 0f;
            float right = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f;
            horizontalInput = left + right;
        }

        if (horizontalInput != 0)
        {
            anim.SetBool("isWalking", true);

            // defaultScaleをベースに、X方向の向きだけを掛け合わせる
            float direction = Mathf.Sign(horizontalInput);
            transform.localScale = new Vector3(defaultScale.x * direction, defaultScale.y, defaultScale.z);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
}