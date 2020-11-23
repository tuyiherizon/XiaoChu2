using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetRenderImage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region camera

    public Camera _BattleCamera;
    public RawImage _RawImage;

    public void SetRenderImage()
    {
        var textureSize = GetTextureSize();
        var renderTexture = RenderTexture.GetTemporary(Mathf.FloorToInt(textureSize.x), Mathf.FloorToInt(textureSize.y), 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        _BattleCamera.targetTexture = renderTexture;

        _RawImage.texture = renderTexture;
    }

    private Vector2 GetTextureSize()
    {
        var result = Vector2.zero;
        if (_RawImage != null)
        {
            var imageSize = _RawImage.rectTransform.rect.size;
            if (imageSize.x > 0f && imageSize.y > 0f)
            {
                // 贴图尺寸校正到单边不超过512
                //var ratio = Mathf.Min(512f / imageSize.x, 512f / imageSize.y, 1f);
                result = imageSize;// * ratio;
            }
        }
        return result;
    }

    #endregion
}
