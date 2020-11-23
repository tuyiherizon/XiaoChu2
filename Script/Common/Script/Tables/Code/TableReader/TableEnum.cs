using System.Collections;

namespace Tables
{

    //
    public enum ELEMENT_TYPE
    {
        NONE = 0, //None,枚举必须保留0值
        FIRE = 1, //FIRE,FIRE
        ICE = 2, //ICE,ICE
        WIND = 3, //WIND,WIND
        LIGHT = 4, //LIGHT,LIGHT
        DARK = 5, //DARK,DARK
    }

    //
    public enum ITEM_QUALITY
    {
        WHITE = 0, //WHITE,枚举必须保留0值
        GREEN = 1, //GREEN,GREEN
        BLUE = 2, //BLUE,BLUE
        PURPER = 3, //PURPER,PURPER
        ORIGIN = 4, //ORIGIN,ORIGIN
    }

    //
    public enum STAGE_TYPE
    {
        NORMAL = 0, //NORMAL,枚举必须保留0值
        BOSS = 1, //BOSS,BOSS
        DOUBLE_BOSS = 2, //DOUBLE_BOSS,DOUBLE_BOSS
        ACT_GOLD = 3, //ACT_GOLD,ACT_GOLD
        ACT_GEM = 4, //ACT_GEM,ACT_GEM
    }


}