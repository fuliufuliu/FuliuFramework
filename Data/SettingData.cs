
using System;

[Serializable]
public class SettingData : BaseData
{
    /// <summary>
    /// 是否开发陀螺仪
    /// </summary>
    public bool isOpenGyro = false;
    /// <summary>
    /// 是否播放声音
    /// </summary>
    public bool isOpenSound = true;
    /// <summary>
    /// 是否开启 Bloom 效果
    /// </summary>
    public bool isOpenBloom = true;
    /// <summary>
    /// 是否开启 震动 效果
    /// </summary>
    public bool isOpenShake = true;
}
