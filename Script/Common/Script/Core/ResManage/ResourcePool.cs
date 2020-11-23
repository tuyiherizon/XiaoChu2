using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;

public class ResourcePool : InstanceBase<ResourcePool>
{

    void Awake()
    {
        SetInstance(this);

    }

    void Destory()
    {
        SetInstance(null);
    }

    public void InitDefaultRes()
    {
        //if (ResourceManager._ResFromBundle)
        {
            InitEffect();
            InitAutio();
            InitConfig();
        }
    }

    #region effect

    public Dictionary<string, EffectController> _CommonHitEffect;

    public Dictionary<string, EffectController> _LoadedEffects = new Dictionary<string, EffectController>();

    private Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();

    public static string LineEffectName = "UI/EffectLine";
    public static string BombEffectName = "UI/EffectBomb";
    public static string LightEffectName = "UI/EffectLight";
    public static string HPBombName = "UI/EffectHpBomb";

    private void InitEffect()
    {
        if (_CommonHitEffect != null)
            return;

        _CommonHitEffect = new Dictionary<string, EffectController>();
        LoadEffect(LineEffectName, (effectName, effectGo, callBackHash) => { }, null);
        LoadEffect(BombEffectName, (effectName, effectGo, callBackHash) => { }, null);
        LoadEffect(LightEffectName, (effectName, effectGo, callBackHash) => { }, null);
        LoadEffect(HPBombName, (effectName, effectGo, callBackHash) => { }, null);
    }

    public void LoadEffect(string effectRes, LoadBundleAssetCallback<EffectController> callBack, Hashtable hash)
    {
        if (_LoadedEffects.ContainsKey(effectRes))
        {
            callBack.Invoke(effectRes, _LoadedEffects[effectRes], hash);
            return;
        }

        string effectPath = "Effect/" + effectRes;
        ResourceManager.Instance.LoadPrefab(effectPath, (effectName, effectGo, callBackHash) =>
        {
            EffectController effct = effectGo.GetComponent<EffectController>();
            effct.transform.SetParent(transform);
            effct.name = effectName;
            _LoadedEffects.Add(effectRes, effct);
            callBack.Invoke(effectRes, _LoadedEffects[effectRes], callBackHash);
        }, hash);
    }

    public EffectController GetIdleEffect(string name)
    {
        if (_LoadedEffects.ContainsKey(name))
        {
            return GetIdleEffect(_LoadedEffects[name]);
        }
        return null;
    }

    public EffectController GetIdleEffect(EffectController effct)
    {

        EffectController idleEffect = null;
        if (_IdleEffects.ContainsKey(effct.name))
        {
            if (_IdleEffects[effct.name].Count > 0)
            {
                idleEffect = _IdleEffects[effct.name].Pop();
            }
        }

        if (idleEffect == null)
        {
            idleEffect = GameObject.Instantiate<EffectController>(effct);
            idleEffect.name = effct.name;
        }

        idleEffect._EffectLastTime = effct._EffectLastTime;
        return idleEffect;
    }

    public void RecvIldeEffect(EffectController effct)
    {

        string effectName = effct.name;
        if (!_IdleEffects.ContainsKey(effectName))
        {
            _IdleEffects.Add(effectName, new Stack<EffectController>());
        }
        effct.transform.SetParent(transform);
        effct.HideEffect();
        effct.transform.localPosition = Vector3.zero;
        _IdleEffects[effectName].Push(effct);
    }

    public bool IsEffectInRecvl(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            return false;
        }
        return (_IdleEffects[effectName].Contains(effct));
    }


    public void ClearEffects()
    {
        foreach (var idleEffectKeys in _IdleEffects.Values)
        {
            foreach (var idleEffect in idleEffectKeys)
            {
                GameObject.Destroy(idleEffect);
            }
        }
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }

    public void PlaySceneEffect(EffectController effct, Vector3 position, Vector3 rotation)
    {
        var effectInstance = GetIdleEffect(effct);
        effectInstance.transform.SetParent(transform);
        effectInstance.transform.position = position;
        effectInstance.transform.rotation = Quaternion.Euler(rotation);
        effectInstance.PlayEffect();
    }

    private void InitEffectCallBack(string uiName, GameObject effectGO, Hashtable hashtable)
    {
        _CommonHitEffect.Add(uiName, effectGO.GetComponent<EffectController>());
        effectGO.SetActive(false);
        effectGO.transform.SetParent(transform);
        effectGO.transform.localPosition = Vector3.zero;
    }
    #endregion
    
    #region ui

    private Dictionary<string, Stack<GameObject>> _IdleUIItems = new Dictionary<string, Stack<GameObject>>();
    
    public T GetIdleUIItem<T>(GameObject itemPrefab, Transform parentTrans = null)
    {
        GameObject idleItem = null;
        if (_IdleUIItems.ContainsKey(itemPrefab.name))
        {
            if (_IdleUIItems[itemPrefab.name].Count > 0)
            {
                idleItem = _IdleUIItems[itemPrefab.name].Pop();
            }
        }

        if (idleItem == null)
        {
            idleItem = GameObject.Instantiate<GameObject>(itemPrefab);
        }
        idleItem.gameObject.SetActive(true);
        if (parentTrans != null)
        {
            idleItem.transform.SetParent(parentTrans);
            idleItem.transform.localPosition = Vector3.zero;
            idleItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
            idleItem.transform.localScale = Vector3.one;
        }
        return idleItem.GetComponent<T>();
    }

    public void RecvIldeUIItem(GameObject itemBase)
    {
        string itemName = itemBase.name.Replace("(Clone)", "");
        if (!_IdleUIItems.ContainsKey(itemName))
        {
            _IdleUIItems.Add(itemName, new Stack<GameObject>());
        }
        itemBase.gameObject.SetActive(false);
        itemBase.transform.SetParent(transform);
        if (!_IdleUIItems[itemName].Contains(itemBase))
        {
            _IdleUIItems[itemName].Push(itemBase);
        }
    }

    public void ClearUIItems()
    {
        foreach (var idleResKeys in _IdleUIItems.Values)
        {
            foreach (var idleRes in idleResKeys)
            {
                GameObject.Destroy(idleRes);
            }
        }
        _IdleUIItems = new Dictionary<string, Stack<GameObject>>();
    }

    #endregion

    #region model

    private Dictionary<string, Stack<GameObject>> _IdleModelItems = new Dictionary<string, Stack<GameObject>>();

    public void LoadModel(string modelName, LoadBundleAssetCallback<GameObject> callBack, Hashtable hash)
    {
        if (_IdleModelItems.ContainsKey(modelName))
        {
            var instance = PopIdleModel(modelName);
            callBack.Invoke(modelName, instance, hash);
            return;
        }

        string modelPath = "Model/" + modelName;
        ResourceManager.Instance.LoadPrefab(modelPath, (resName, modelGO, callBackHash) =>
        {
            modelGO.transform.SetParent(transform);
            modelGO.name = modelName;
            _IdleModelItems.Add(modelName, new Stack<GameObject>());
            _IdleModelItems[modelName].Push(modelGO);
            var instance = PopIdleModel(modelName);
            callBack.Invoke(modelName, instance, hash);
        }, hash);
    }

    public GameObject PopIdleModel(string modelName)
    {
        if (!_IdleModelItems.ContainsKey(modelName))
            return null;

        if (_IdleModelItems[modelName].Count == 1)
        {
            GameObject lastModel = _IdleModelItems[modelName].Peek();
            GameObject instance = GameObject.Instantiate(lastModel);
            instance.name = lastModel.name;
            instance.gameObject.SetActive(true);
            return instance;
        }
        else
        {
            var instance = _IdleModelItems[modelName].Pop();
            instance.gameObject.SetActive(true);
            return instance;
        }
    }

    public void RecvIldeModelItem(GameObject itemBase)
    {
        string itemName = itemBase.name.Replace("(Clone)", "");
        if (!_IdleModelItems.ContainsKey(itemName))
        {
            _IdleModelItems.Add(itemName, new Stack<GameObject>());
        }
        itemBase.gameObject.SetActive(false);
        itemBase.transform.SetParent(transform);
        if (!_IdleModelItems[itemName].Contains(itemBase))
        {
            _IdleModelItems[itemName].Push(itemBase);
        }
    }

    public void ClearModelItems()
    {
        foreach (var idleResKeys in _IdleModelItems.Values)
        {
            foreach (var idleRes in idleResKeys)
            {
                GameObject.Destroy(idleRes);
            }
        }
        _IdleModelItems = new Dictionary<string, Stack<GameObject>>();
    }

    #endregion

    #region audio

    public Dictionary<int, AudioClip> _CommonAudio;
    public int _HitSuperArmor = 0;

    private void InitAutio()
    {
        if (_CommonAudio != null)
            return;

        _CommonAudio = new Dictionary<int, AudioClip>();
 
        //ResourceManager.Instance.LoadAudio("common/HitArmor", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(0, resData);
        //}, null);

        //ResourceManager.Instance.LoadAudio("common/HitSwordNone", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(1, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitSwordBody", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(2, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitSwordSlap", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(3, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitSwordSlap2", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(4, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitHwNone", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(10, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitHwBody", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(11, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HwAtk", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(12, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkFire", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(100, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkIce", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(101, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkLighting", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(102, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkStone", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(103, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkWind", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(104, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitFire", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(110, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitFire", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(111, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitLighting", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(112, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitStone", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(113, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/HitWind", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(114, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkFire2", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(120, resData);
        //}, null);
        
        //ResourceManager.Instance.LoadAudio("common/AtkBow", (resName, resData, callBackHash) =>
        //{
        //    _CommonAudio.Add(200, resData);
        //}, null);
    }


    #endregion

    #region config prefab common

    public enum ConfigEnum
    {
        HitProtectedBuff,
        DexAccelateBuff,
        SuperArmor,
        SuperArmorBlock,
        BlockBullet,
        IntShieldBuff,
        StrBuff,
        ResourceConfig,
        RandomBuff,
        BlockSummon

    }

    public static Dictionary<ConfigEnum, string> ConfigPrefabs = new Dictionary<ConfigEnum, string>()
    {
        //{ ConfigEnum.HitProtectedBuff, "SkillMotion/CommonImpact/HitProtectedBuff"},
        //{ ConfigEnum.DexAccelateBuff, "SkillMotion/CommonImpact/DexAccelateBuff"},
        //{ ConfigEnum.SuperArmor, "SkillMotion/CommonImpact/SuperArmor"},
        //{ ConfigEnum.SuperArmorBlock, "SkillMotion/CommonImpact/SuperArmorBlock"},
        //{ ConfigEnum.BlockBullet, "SkillMotion/CommonImpact/BlockBullet"},
        //{ ConfigEnum.IntShieldBuff, "SkillMotion/CommonImpact/IntShieldBuff"},
        //{ ConfigEnum.StrBuff, "SkillMotion/CommonImpact/StrBuff"},
        //{ ConfigEnum.ResourceConfig, "Common/ResourceConfig"},
        //{ ConfigEnum.RandomBuff, "SkillMotion/CommonImpact/EliteRandomBuff"},
        //{ ConfigEnum.BlockSummon, "SkillMotion/CommonImpact/BlockSummon"},
    };

    public Dictionary<string, GameObject> _ConfigPrefabs;

    public T GetConfig<T>(ConfigEnum configType)
    {
        string configName = ConfigPrefabs[configType];
        if (_ConfigPrefabs.ContainsKey(configName))
        {
            return _ConfigPrefabs[configName].GetComponent<T>();
        }

        return default(T);
    }

    public void InitConfig()
    {
        if (_ConfigPrefabs != null)
            return;

        _ConfigPrefabs = new Dictionary<string, GameObject>();
        foreach (var configName in ConfigPrefabs.Values)
        {
            Hashtable hash = new Hashtable();
            hash.Add("ConfigName", configName);
            ResourceManager.Instance.LoadPrefab(configName, InitConfigPrefabCallBack, hash);
        }
    }

    private void InitConfigPrefabCallBack(string resName, GameObject resGO, Hashtable hashtable)
    {
        string configName = (string)hashtable["ConfigName"];
        _ConfigPrefabs.Add(configName, resGO);
        resGO.transform.SetParent(transform);
        //resGO.SetActive(false);
        resGO.transform.localPosition = Vector3.zero;
    }

    #endregion

    #region config prefab

    public Dictionary<string, GameObject> _LoadedConfig = new Dictionary<string, GameObject>();

    public void LoadConfig(string resName, LoadBundleAssetCallback<GameObject> callBack, Hashtable hash)
    {
        if (_LoadedConfig.ContainsKey(resName))
        {
            GameObject configInstance = GameObject.Instantiate(_LoadedConfig[resName]);
            callBack.Invoke(resName, configInstance, hash);
            return;
        }

        string resPath = resName;
        ResourceManager.Instance.LoadPrefab(resPath, (effectName, effectGo, callBackHash) =>
        {
            
            effectGo.transform.SetParent(transform);
            effectGo.name = effectName;
            if (!_LoadedConfig.ContainsKey(resName))
                _LoadedConfig.Add(resName, effectGo);

            GameObject configInstance = GameObject.Instantiate(_LoadedConfig[resName]);
            callBack.Invoke(resName, configInstance, hash);
        }, hash);
    }

    #endregion

}
