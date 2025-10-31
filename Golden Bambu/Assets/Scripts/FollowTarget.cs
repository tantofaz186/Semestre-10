#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    bool customOffset = false;

    private Vector3 offset;
    private const float SmoothTime = 0.3f;

    private void Start()
    {
        if (target == null)
        {
            enabled = false;
            return;
        }
        if (!customOffset) offset = transform.position - target.position;
    }

    public void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothTime);
    }

    #if UNITY_EDITOR
[CustomEditor(typeof(FollowTarget))]
    public class FollowTargetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            FollowTarget _this = (FollowTarget)target;
            base.OnInspectorGUI();
            if (_this.customOffset)
            {
                _this.offset = EditorGUILayout.Vector3Field("Offset", _this.offset);
            }
        }
    }
    #endif
}
