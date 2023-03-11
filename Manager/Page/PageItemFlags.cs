using System;

namespace Manager.Page
{
    [Flags]
    public enum PageItemFlags
    {
        /// <summary>
        /// 正常
        /// </summary>
        Default,

        /// <summary>
        /// 不支持搜索
        /// </summary>
        NoIndex,

        /// <summary>
        /// 支持描述搜索
        /// </summary>
        IndexWithDescrption,

        /// <summary>
        /// 自绘制名称
        /// </summary>
        DrawNameBySelf,

        /// <summary>
        /// 自绘制描述
        /// </summary>
        DrawDescrptionBySelf
    }
}
