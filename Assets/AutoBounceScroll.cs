using UnityEngine;

public class AutoBounceScroll : MonoBehaviour
{
    [Header("移動速度")]
    public float scrollSpeed = 2.0f;

    [Header("移動の端（X軸）")]
    public float leftLimitX = -5.0f;  // 左端の限界位置
    public float rightLimitX = 5.0f;   // 右端の限界位置

    // 現在移動してる向き（+が右, -が左）
    private float direction = 1.0f;

    void Update()
    {
        // 現在の向きに移動
        transform.Translate(Vector3.right * direction * scrollSpeed * Time.deltaTime);

        // 右端を超えたら左向きに反転
        if (transform.position.x >= rightLimitX)
        {
            // 位置を限界値に固定して食い込みを防止
            Vector3 pos = transform.position;
            pos.x = rightLimitX;
            transform.position = pos;

            direction = -1.0f; // 左へ移動
        }
        // 左端を超えたら右向きに反転
        else if (transform.position.x <= leftLimitX)
        {
            // 位置を限界値に固定して食い込みを防止
            Vector3 pos = transform.position;
            pos.x = leftLimitX;
            transform.position = pos;

            direction = 1.0f; // 右へ移動
        }
    }
}