using UnityEngine;

public class BlendShapeController : MonoBehaviour
{
    // ���b�V�������_���iVRoid�Ȃ�Face�A�j�[����Ȃ�Body)
    public SkinnedMeshRenderer skinnedMeshRenderer;

    // �Ď��Ώۂ̃u�����h�V�F�C�v�̃C���f�b�N�X
    public int monitoredBlendShapeIndex;

    // �ύX�Ώۂ̃u�����h�V�F�C�v�̃C���f�b�N�X
    public int targetBlendShapeIndex;

    // �ύX�Ώۂ̃u�����h�V�F�C�v�̒l�ɂ�����{��
    public float multiplier;

    // �U��q�̕������Z�̋��x
    public float springStrength = 1.0f;

    // �U��q�̌���
    public float damping = 0.1f;

    private float baseValue;
    private float velocity = 0.0f;

    void Start()
    {
        // ������
        if (skinnedMeshRenderer != null)
        {
            baseValue = skinnedMeshRenderer.GetBlendShapeWeight(targetBlendShapeIndex);
        }
    }

    void Update()
    {
        if (skinnedMeshRenderer == null) return;

        // �Ď��Ώۂ̃u�����h�V�F�C�v�̒l
        float monitoredBlendShapeValue = skinnedMeshRenderer.GetBlendShapeWeight(monitoredBlendShapeIndex);

        // �{���h�������u�����h�V�F�C�v�l
        float targetValue = monitoredBlendShapeValue * multiplier;

        // ���݂̕ύX�Ώۂ̃u�����h�V�F�C�v�l
        float currentValue = skinnedMeshRenderer.GetBlendShapeWeight(targetBlendShapeIndex);

        // �E�j�E�j��炷���
        // ������͂Ɏ��Ԃ��Ƃ̌��������������čŏI�I�ȗ͗ʂ��Z�o
        float force = springStrength * (targetValue - currentValue);
        velocity += force * Time.deltaTime;
        velocity *= Mathf.Exp(-damping * Time.deltaTime);

        float newValue = currentValue + velocity * Time.deltaTime;
        skinnedMeshRenderer.SetBlendShapeWeight(targetBlendShapeIndex, newValue);
    }

    // �u�����h�V�F�C�v�ꗗ���o�����
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
