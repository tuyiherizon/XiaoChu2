using System.Collections;

namespace Tables
{
    public class TableReader
    {

        #region 唯一实例

        private TableReader() { }

        private TableReader _Instance = null;
        public TableReader Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new TableReader();

                return _Instance;
            }
        }

        #endregion
        #region Logic

//
        public static CommonItem CommonItem { get; internal set; }
//
        public static GameDataValue GameDataValue { get; internal set; }
//
        public static Gem Gem { get; internal set; }
//
        public static GemExAttr GemExAttr { get; internal set; }
//
        public static MonsterBase MonsterBase { get; internal set; }
//
        public static Recharge Recharge { get; internal set; }
//
        public static SkillBase SkillBase { get; internal set; }
//
        public static StageInfo StageInfo { get; internal set; }
//
        public static StrDictionary StrDictionary { get; internal set; }
//
        public static Weapon Weapon { get; internal set; }

        public static void ReadTables()
        {
            //读取所有表
            CommonItem = new CommonItem(TableReadBase.GetTableText("CommonItem"), false);
            GameDataValue = new GameDataValue(TableReadBase.GetTableText("GameDataValue"), false);
            Gem = new Gem(TableReadBase.GetTableText("Gem"), false);
            GemExAttr = new GemExAttr(TableReadBase.GetTableText("GemExAttr"), false);
            MonsterBase = new MonsterBase(TableReadBase.GetTableText("MonsterBase"), false);
            Recharge = new Recharge(TableReadBase.GetTableText("Recharge"), false);
            SkillBase = new SkillBase(TableReadBase.GetTableText("SkillBase"), false);
            StageInfo = new StageInfo(TableReadBase.GetTableText("StageInfo"), false);
            StrDictionary = new StrDictionary(TableReadBase.GetTableText("StrDictionary"), false);
            Weapon = new Weapon(TableReadBase.GetTableText("Weapon"), false);

            //初始化所有表
            CommonItem.CoverTableContent();
            GameDataValue.CoverTableContent();
            Gem.CoverTableContent();
            GemExAttr.CoverTableContent();
            MonsterBase.CoverTableContent();
            Recharge.CoverTableContent();
            SkillBase.CoverTableContent();
            StageInfo.CoverTableContent();
            StrDictionary.CoverTableContent();
            Weapon.CoverTableContent();
        }

        #endregion
    }
}
