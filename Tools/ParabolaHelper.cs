using UnityEngine;

public static class ParabolaHelper
{
    /// <summary>
    /// 获取抛物线上的点，从出发点开始
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="volocity"></param>
    /// <param name="time"></param>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public static Vector3[] GetPhysicsParabolaLine(Vector3 pos, Vector3 volocity,
        uint pointCount = 20)
    {
        var res = new Vector3[pointCount + 1];
        res[0] = pos;
        if (pointCount == 0)
        {
            return res;
        }

        var gravity = Physics.gravity;

        float deltaTime = Time.fixedDeltaTime;
        float dTime = 0f; //已经过去的时间
        for (int i = 0; i < pointCount; i++)
        {
            //计算物体的重力速度
            dTime += deltaTime;
            var gravitySpeed = gravity * dTime; //重力的速度向量，t时为0
            //位移模拟轨迹
            res[i + 1] = res[i] + (volocity + gravitySpeed) * deltaTime;
//            res[i + 1] = pos + (volocity + gravity * dTime) * dTime;
        }

        return res;
    }
}