using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ResEffect))]
public class ResEffectEditor : Editor
{
    ResEffect _TargetResEffect;
    Rect _PreviewRect;
    GUIStyle _PreviewBackground;
    int _LastEffectFrame = -1;

    public override bool HasPreviewGUI()
    {
        return true;
    }

    public override GUIContent GetPreviewTitle()
    {
        return new GUIContent("预览");
    }

    public override void OnPreviewSettings()
    {
        GUILayout.Label("文本", "preLabel");
        GUILayout.Button("按钮", "preButton");
    }

    public void OnEnable()
    {
        EditorApplication.update += UpdateEffectPreview;
    }

    public void OnDisabel()
    {
        EditorApplication.update -= UpdateEffectPreview;
    }

    public void UpdateEffectPreview()
    {
        
        if (m_PreviewInstance != null)
        {
            int curIdx = m_PreviewInstance.GetComponent<ResEffect>().GetCurFrameIdx();
        
            if (_LastEffectFrame != curIdx)
            {
                _LastEffectFrame = curIdx;

                //int controlID = GUIUtility.GetControlID(FocusType.Passive);
                //Event current = Event.current;
                //GUIUtility.hotControl = controlID;
                //current.Use();
                GUI.changed = true;
                Repaint();
                
            }
        }
    }
    

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        //Debug.Log("OnPreviewGUI:" + Time.realtimeSinceStartup + ",type:" + Event.current.type);
        _PreviewRect = r;
        _PreviewBackground = background;

        InitPreview();
        if (Event.current.type != EventType.Repaint)
        {
            //return;
            
            return;
        }

        m_PreviewUtility.AddSingleGO(m_PreviewInstance);

        m_PreviewUtility.BeginPreview(r, background);
        
        Camera camera = m_PreviewUtility.camera;

        camera.orthographic = true;
        camera.orthographicSize = 2;
        camera.transform.position = m_PreviewInstance.transform.position + new Vector3(0, 0, -6);
        camera.transform.LookAt(m_PreviewInstance.transform);

        SetEnabledRecursive(m_PreviewInstance, true);
        camera.Render();
        SetEnabledRecursive(m_PreviewInstance, false);

        m_PreviewUtility.EndAndDrawPreview(r);
    }

    private PreviewRenderUtility m_PreviewUtility;
    private GameObject m_PreviewInstance;

    private void InitPreview()
    {
        if (_TargetResEffect == null)
        {
            _TargetResEffect = (ResEffect)target;
        }

        if (m_PreviewUtility == null)
        {
            // 参数true代表绘制场景内的游戏对象
            m_PreviewUtility = new PreviewRenderUtility(true);
            // 设置摄像机的一些参数
            m_PreviewUtility.cameraFieldOfView = 1f;

            

            // 创建预览的游戏对象
            CreatePreviewInstances();
        }
        
    }

    private void DestroyPreview()
    {
        if (m_PreviewUtility != null)
        {
            // 务必要进行清理，才不会残留生成的摄像机对象等
            m_PreviewUtility.Cleanup();
            m_PreviewUtility = null;
        }
    }

    private void CreatePreviewInstances()
    {
        DestroyPreviewInstances();

        // 绘制场景上已经存在的游戏对象
        //m_PreviewInstance = GameObject.Find("ZuoCi_DaZhao_BeiJi");
        m_PreviewInstance = Instantiate(_TargetResEffect.gameObject, Vector3.zero, Quaternion.identity) as GameObject;

        InitInstantiatedPreviewRecursive(m_PreviewInstance);
        //// 关闭对象渲染
        SetEnabledRecursive(m_PreviewInstance, false);
    }

    private void DestroyPreviewInstances()
    {
        if (m_PreviewInstance != null)
        {
            DestroyImmediate(m_PreviewInstance);
        }
        m_PreviewInstance = null;
    }

    // 预览摄像机的绘制层 Camera.PreviewCullingLayer
    // 为了防止引擎更改，可以通过反射获取，这里直接写值
    private const int kPreviewCullingLayer = 31;

    private static void InitInstantiatedPreviewRecursive(GameObject go)
    {
        go.hideFlags = HideFlags.HideAndDontSave;
        go.layer = kPreviewCullingLayer;
        foreach (Transform transform in go.transform)
        {
            InitInstantiatedPreviewRecursive(transform.gameObject);
        }
    }

    public static void SetEnabledRecursive(GameObject go, bool enabled)
    {
        Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < componentsInChildren.Length; i++)
        {
            Renderer renderer = componentsInChildren[i];
            renderer.enabled = enabled;
        }
    }

    void OnDestroy()
    {
        DestroyPreviewInstances();
        DestroyPreview();
    }
}
