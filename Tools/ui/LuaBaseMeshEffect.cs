//using LuaFramework;
//using LuaInterface;
//using UnityEngine.UI;
//
//public class LuaBaseMeshEffect : BaseMeshEffect
//{
//    public LuaFunction luaModifyMesh;
//
//    public void Init(LuaFunction luaModifyMesh)
//    {
//        this.luaModifyMesh = luaModifyMesh;
//    }
//
//    public override void ModifyMesh(VertexHelper vh)
//    {
//        if (luaModifyMesh != null)
//        {
//            Util.CallMethodNotDispose(luaModifyMesh, vh, this);
//        }
//    }
//
//    void OnDestroy()
//    {
//        if (luaModifyMesh != null)
//        {
//            luaModifyMesh.Dispose();
//            luaModifyMesh = null;
//        }
//    }
//}
