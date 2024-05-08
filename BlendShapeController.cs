using UnityEngine;

public class BlendShapeController : MonoBehaviour
{
    // メッシュレンダラ（VRoidならFace、桔梗さんならBody)
    public SkinnedMeshRenderer skinnedMeshRenderer;

    // 監視対象のブレンドシェイプのインデックス
    public int monitoredBlendShapeIndex;

    // 変更対象のブレンドシェイプのインデックス
    public int targetBlendShapeIndex;

    // 変更対象のブレンドシェイプの値にかける倍率
    public float multiplier;

    // 振り子の物理演算の強度
    public float springStrength = 1.0f;

    // 振り子の減衰
    public float damping = 0.1f;

    private float baseValue;
    private float velocity = 0.0f;

    void Start()
    {
        // 初期化
        if (skinnedMeshRenderer != null)
        {
            baseValue = skinnedMeshRenderer.GetBlendShapeWeight(targetBlendShapeIndex);
        }
    }

    void Update()
    {
        if (skinnedMeshRenderer == null) return;

        // 監視対象のブレンドシェイプの値
        float monitoredBlendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(monitoredBlendShapeIndex);

        // 倍率ドンしたブレンドシェイプ値
        float targetValue = monitoredBlendShapeValue * multiplier;

        // 現在の変更対象のブレンドシェイプ値
        float currentValue = skinnedMeshRenderer.GetBlendShapeWeight(targetBlendShapeIndex);

        // ウニウニゆらすやつ
        // かかる力に時間ごとの減衰を差っ引いて最終的な力量を算出
        float force = springStrength * (targetValue - currentValue);
        velocity += force * Time.deltaTime;
        velocity *= Mathf.Exp(-damping * Time.deltaTime);

        float newValue = currentValue + velocity * Time.deltaTime;
        skinnedMeshRenderer.SetBlendShapeWeight(targetBlendShapeIndex, newValue);
    }

    // ブレンドシェイプ一覧を出すやつ
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(BlendShapeController))]
    public class BlendShapeControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BlendShapeController script = (BlendShapeController)target;

            if (script.skinnedMeshRenderer != null)
            {
                string[] blendShapeNames = new string[script.skinnedMeshRenderer.sharedMesh.blendShapeCount];
                for (int i = 0; i < blendShapeNames.Length; i++)
                {
                    blendShapeNames[i] = script.skinnedMeshRenderer.sharedMesh.GetBlendShapeName(i);
                }
                script.monitoredBlendShapeIndex = UnityEditor.EditorGUILayout.Popup("Monitor Blend Shape", script.monitoredBlendShapeIndex, blendShapeNames);
                script.targetBlendShapeIndex = UnityEditor.EditorGUILayout.Popup("Target Blend Shape", script.targetBlendShapeIndex, blendShapeNames);
            }
        }
    }
#endif
}
