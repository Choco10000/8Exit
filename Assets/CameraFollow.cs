using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("追跡対象（プレイヤー）")]
    public Transform target;

    [Header("追従のスムーズ度")]
    public float smoothTime = 0.3f;

    [Header("カメラのオフセット（位置のズレ）")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // ターゲット（プレイヤー）が設定されていない場合は何もしない
        if (target == null) return;

        // 目標となるカメラの位置を計算（プレイヤーの位置 + オフセット）
        Vector3 targetPosition = target.position + offset;

        // 現在の位置から目標の位置まで、滑らかに移動させる（SmoothDamp）
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}