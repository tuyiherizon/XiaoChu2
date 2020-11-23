using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using UnityEditor.Animations;

public class ModifyRes : MonoBehaviour
{
    [MenuItem("TFImage/WritePList")]
    public static void WritePList()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                Debug.Log("texturePath:" + texturePath);
                var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Multiple;

                var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                string plistPath = texturePath.Replace(".png", ".plist");

                SaveSpritePList(plistPath, importer.spritesheet, texture.width, texture.height);
                //List<SpriteMetaData> spriteMetaDatas = new List<SpriteMetaData>();

                //SpriteMetaData spriteData = new SpriteMetaData();
                //spriteData.alignment = 1;
                //spriteData.name = "weapon_1";
                //spriteData.rect = new Rect(381 , 2, 378, 64);
                //spriteData.pivot = new Vector2(378, 64);

                //spriteMetaDatas.Add(spriteData);

                Debug.Log("importer.spritesheet:" + importer.spritesheet.Length);

                importer.SaveAndReimport();
            }
        }
    }


    public static void SaveSpritePList(string path, SpriteMetaData[] spriteDatas, int imgWidth, int imgHeight)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("plist");
        xmlDoc.AppendChild(root);
        XmlElement dict = xmlDoc.CreateElement("dict");
        root.AppendChild(dict);
        XmlElement frameKey = xmlDoc.CreateElement("key");
        frameKey.InnerText = "frames";
        dict.AppendChild(frameKey);
        XmlElement frameDict = xmlDoc.CreateElement("dict");
        dict.AppendChild(frameDict);


        foreach (var spriteData in spriteDatas)
        {
            XmlElement spriteFrame = xmlDoc.CreateElement("key");
            Debug.Log("spriteData.name:" + spriteData.name);
            spriteFrame.InnerText = spriteData.name;
            frameDict.AppendChild(spriteFrame);

            XmlElement spriteDict = xmlDoc.CreateElement("dict");
            frameDict.AppendChild(spriteDict);

            XmlElement keyFrame = xmlDoc.CreateElement("key");
            keyFrame.InnerText = "frame";
            spriteDict.AppendChild(keyFrame);
            

            float width = spriteData.rect.width;
            float height = spriteData.rect.height;
            float x = spriteData.rect.x;
            float y = imgHeight - spriteData.rect.y - height;
            Debug.Log(x + "," + y + "," + width + "," + height);
            string frame = "{{" + x + "," +  y + "},{" +  width + "," + height + "}}";
            XmlElement keyFrameValue = xmlDoc.CreateElement("string");
            keyFrameValue.InnerText = frame;
            spriteDict.AppendChild(keyFrameValue);
        }

        xmlDoc.Save(path);
    }


    [MenuItem("TFImage/CreateEffectPrefabs")]
    public static void CreateEffectPrefabs()
    {
        _PrefabX = 0;
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                Debug.Log("texturePath:" + texturePath);
                var importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;

                CreateEffect(selectGO as Texture2D);
            }
        }
    }

    private static float _PrefabX = 0;
    private static void CreateEffect(Texture2D effectTex)
    {
        string prefabPath = Application.dataPath + "/Imgs/Effect/Prefab";
        if (!Directory.Exists(prefabPath))
            Directory.CreateDirectory(prefabPath);

        string tempPrefab = "Assets/Imgs/Effect/_Template.prefab";

        string materialPath = Application.dataPath + "/Imgs/Effect/Material";
        if (!Directory.Exists(materialPath))
            Directory.CreateDirectory(materialPath);

        var shader = Shader.Find("Mobile/Particles/Additive");
        Material mat = new Material(shader);
        mat.mainTexture = effectTex;
        

        var temp = AssetDatabase.LoadAssetAtPath<GameObject>(tempPrefab);
        GameObject effectGO = GameObject.Instantiate(temp);
        effectGO.SetActive(true);
        effectGO.transform.position = new Vector3(_PrefabX, 0, 0);
        _PrefabX += 100;
        effectGO.name = effectTex.name;
        var particleSys = effectGO.GetComponent<ParticleSystemRenderer>();
        particleSys.material = mat;

        try
        {
            string effectMatPath = materialPath.Replace(Application.dataPath, "Assets") + "/" + effectTex.name + ".mat";
            Debug.Log(effectMatPath);
            AssetDatabase.CreateAsset(mat, effectMatPath);
            string effectPath = prefabPath.Replace(Application.dataPath, "Assets") + "/" + effectTex.name + ".prefab";
            //AssetDatabase.CreateAsset(effectGO, effectPath);
            PrefabUtility.CreatePrefab(effectPath, effectGO);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Create Asset error:" + e.ToString());
        }
    }

    [MenuItem("TFImage/CreateEffectAnim")]
    public static void CreateEffectAnim()
    {
        _PrefabX = 0;
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                var imgResources = AssetDatabase.LoadAllAssetsAtPath(texturePath);
                List<Sprite> sprites = new List<Sprite>();
                foreach (var sprite in imgResources)
                {
                    if (sprite is Sprite)
                    {
                        sprites.Add(sprite as Sprite);
                    }
                }

                CreateEffectAnim(texturePath, sprites);
            }
        }
    }

    [MenuItem("TFImage/CreateEffectSplitAnim")]
    public static void CreateEffectSplitAnim()
    {
        _PrefabX = 0;
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                var texturePath = AssetDatabase.GetAssetPath(selectGO);
                var imgResources = AssetDatabase.LoadAllAssetsAtPath(texturePath);
                Dictionary<string, List<Sprite>> sprites = new Dictionary<string, List<Sprite>>();
                foreach (var spriteRes in imgResources)
                {
                    if (spriteRes is Sprite)
                    {
                        Sprite sprite = spriteRes as Sprite;
                        string spriteName = sprite.name.Replace("(r)", "");
                        //string foldName = spriteName.Substring(0, spriteName.Length - 8);
                        var  splitNames = spriteName.Split('_');
                        string foldName = sprite.name;
                        if (splitNames.Length > 1)
                        {
                            foldName = splitNames[0] + splitNames[1];
                        }
                        if (!sprites.ContainsKey(foldName))
                        {
                            sprites.Add(foldName, new List<Sprite>());
                        }
                        sprites[foldName].Add(sprite);
                    }
                }

                foreach (var spriteList in sprites)
                {
                    string fole = Path.GetDirectoryName(texturePath);
                    string effectName = fole + "/" + spriteList.Value[0].name;
                    spriteList.Value.Sort((spriteA, spriteB) =>
                    {
                        Debug.Log("spriteA.name:" + spriteA.name + ", spriteB.name:" + spriteB.name);
                        var splitNamesA = spriteA.name.Replace(Path.GetExtension(spriteA.name), "").Split('_');
                        var splitNamesB = spriteB.name.Replace(Path.GetExtension(spriteB.name), "").Split('_');
                        char nameIdxA = splitNamesA[splitNamesA.Length - 1][0];
                        char nameIdxB = splitNamesA[splitNamesA.Length - 1][0];
                        int idxA = (int)splitNamesA[splitNamesA.Length - 1][0];
                        int idxB = (int)splitNamesA[splitNamesB.Length - 1][0];
                        //int.TryParse(splitNamesA[splitNamesA.Length - 1], out idxA);
                        //int.TryParse(splitNamesB[splitNamesB.Length - 1], out idxB);

                        if (idxA > idxB)
                            return 1;
                        else if (idxB > idxA)
                            return -1;
                        else
                            return 0;
                    });
                    CreateEffectAnim(effectName, spriteList.Value);

                }
            }
        }
    }

    [MenuItem("TFImage/CreateEffectFoldAnim")]
    public static void CreateEffectFoldAnim()
    {
        _PrefabX = 0;
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        List<Sprite> sprites = new List<Sprite>();
        string texturePath = "";
        foreach (var selectGO in selection)
        {
            if (selectGO is Texture2D)
            {
                texturePath = AssetDatabase.GetAssetPath(selectGO);
                var imgResources = AssetDatabase.LoadAllAssetsAtPath(texturePath);
                
                foreach (var sprite in imgResources)
                {
                    if (sprite is Sprite)
                    {
                        sprites.Add(sprite as Sprite);
                    }
                }

                
            }
        }

        sprites.Sort((spriteA, spriteB) =>
            {
                Debug.Log("spriteA.name:" + spriteA.name + ", spriteB.name:" + spriteB.name);
                int idxA = 0;
                int idxB = 0;
                int.TryParse(spriteA.name, out idxA);
                int.TryParse(spriteB.name, out idxB);
                if (idxA > idxB)
                    return 1;
                else if (idxB > idxA)
                    return -1;
                else
                    return 0;
            });
        CreateEffectAnim(texturePath, sprites);
    }

    private static void CreateEffectAnim(string name, List<Sprite> spriteList)
    {
        Debug.Log("CreateEffectAnim:" + name);
        string fileName = Path.GetFileNameWithoutExtension(name);

        string tempPrefab = "Assets/Res/Effect/_Template.prefab";
        var temp = AssetDatabase.LoadAssetAtPath<GameObject>(tempPrefab);
        GameObject effectGO = GameObject.Instantiate(temp);
        effectGO.SetActive(true);
        effectGO.transform.position = new Vector3(_PrefabX, 0, 0);
        _PrefabX += 100;
        effectGO.name = fileName;

        var resEffect = effectGO.GetComponent<ResEffect>();
        resEffect._Sprites = new List<ResEffect.EffectSpriteInfo>();
        foreach (var sprite in spriteList)
        {
            var spriteInfo = new ResEffect.EffectSpriteInfo();
            spriteInfo._Sprites = sprite;
            if (spriteInfo._Sprites.name.EndsWith("(r)"))
            {
                spriteInfo._IsRote = true;
            }
            else
            {
                spriteInfo._IsRote = false;
            }
            resEffect._Sprites.Add(spriteInfo);

        }

        try
        {
            string effectPath = name.Replace(Path.GetExtension(name), ".prefab");
            //AssetDatabase.CreateAsset(effectGO, effectPath);
            PrefabUtility.CreatePrefab(effectPath, effectGO);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Create Asset error:" + e.ToString());
        }
    }
}
