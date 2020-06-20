using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace fuliu
{
    /// <summary>
    /// 使用方法：
    /// 预先设置好，大小格子等参数
    /// 游戏中使用：
    /// 1. 先设置好 externalAction 事件
    /// 2. 调用 CheckObjsInMyBox() 生成被控制的 objs 数据，也可以在设置 isCheckObjsOnAwake = true ,游戏开始时自动处理
    /// 3. 设置哪些 玩家 需要报告位置动态消息到  targets 中
    /// 4. StartCheck() 开始检查
    /// 5. 等待这些targets进入区域去触发 externalAction 事件
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class UnlimitedTriggerArea : MonoBehaviour
    {
        public bool isCreateTrigger = false;
        public int xCellCount = 3;
        public int yCellCount = 1;
        public int zCellCount = 3;
        private UnlimitedTriggerArea[,,] myCells;
        public float offsetLength = 0.15f;
        public float checkTimeSpan = 0.02f;
        public bool isCheckObjsOnAwake = false;

        private BoxCollider _boxCollider;
        public Vector3 size = Vector3.one;
        public List<Component> targets;
        public LayerMask layerMask;
        private List<Collider> hitColliders;
        /// <summary>
        /// 匿名函数的参数被传入： hitColliders, isInBox
        /// </summary>
        [SerializeField] public Action<List<Collider>, bool> externalAction;
        private bool lastIsInBox;
        private WaitForSeconds waitForSeconds;
        private Coroutine checkCor;
        private UnlimitedTriggerArea _parent;

        private UnlimitedTriggerArea parent
        {
            get
            {
                if (_parent)
                {
                    return _parent;
                }

                if (transform.parent)
                {
                    _parent = transform.parent.GetComponent<UnlimitedTriggerArea>();
                }
                return _parent;
            }
        }

        public bool isTest;

        private UnlimitedTriggerArea _root;
        private List<UnlimitedTriggerArea> InBoxColliders = new List<UnlimitedTriggerArea>();
        private List<UnlimitedTriggerArea> OutBoxColliders = new List<UnlimitedTriggerArea>();
        private Vector3 myPos;
        private Vector3 myScale;
        private float xMin;
        private float xMax;
        private float yMin;
        private float yMax;
        private float zMin;
        private float zMax;
        private Bounds myBounds;
        private bool isFirst = true;
        private bool isStartCheck;
        private int level = 0;
        [Tooltip("是否等边？若等边那么，调整offset不会让子格子超出本格子的大小")]
        public bool isEqualsEdge;

        private int updateCheckCountPerFrame;

        private UnlimitedTriggerArea root
        {
            get
            {
                if (_root)
                {
                    return _root;
                }
                
                if (parent != null)
                {
                    _root = parent.root;
                    return _root;
                }

                return this;
            }
        }

        private BoxCollider boxCollider
        {
            get
            {
                if(! _boxCollider)
                    _boxCollider = GetComponent<BoxCollider>();
                return _boxCollider;
            }
        }

        private void Awake()
        {
            FindChildren();
            
            // 
            if (isCheckObjsOnAwake)
            {
                CheckObjsInMyBox();
            }

            boxCollider.enabled = false;
            waitForSeconds = new WaitForSeconds(checkTimeSpan);
        }

        public void StartCheck()
        {
            StopCheck();
            InitComputerParams();
            isStartCheck = true;
            // checkCor = StartCoroutine(_StartCheck());
        }

        public void MyFixedUpdate()
        {
            if (isStartCheck)
            {
                UpdateCheck();
            }
        }
        
        void InitComputerParams()
        {
            myPos = transform.position;
            myScale = transform.lossyScale;
            xMin = myPos.x - size.x * 0.5f * myScale.x;
            xMax = myPos.x + size.x * 0.5f * myScale.x;
            yMin = myPos.y - size.y * 0.5f * myScale.y;
            yMax = myPos.y + size.y * 0.5f * myScale.y;
            zMin = myPos.z - size.z * 0.5f * myScale.z;
            zMax = myPos.z + size.z * 0.5f * myScale.z;
            boxCollider.enabled = true;
            myBounds = boxCollider.bounds;
            boxCollider.enabled = false;
            level = parent == null ? 0 : parent.level + 1;
            
            if (isCreateTrigger)
            {
                for (int i = 0; i < xCellCount; i++)
                {
                    for (int j = 0; j < yCellCount; j++)
                    {
                        for (int k = 0; k < zCellCount; k++)
                        {
                            myCells[i, j, k].InitComputerParams();
                        }
                    }
                }
            }
        }

        public void StopCheck()
        {
            isStartCheck = false;
            if(checkCor != null)
                StopCoroutine(checkCor);
        }

        private IEnumerator _StartCheck()
        {
            while (this)
            {
                yield return waitForSeconds;
                root.UpdateCheck();
            }
        }


        bool _isTargetsInMyBox()
        {
            return isTargetsInMyBox(root.targets);
        }

        
        /// <summary>
        /// 检查规则：
        /// 1. 从树根开始查找
        /// 2. 如果当前节点（包括树根）检查目标已经进来了，或者上次检查时目标是进来的，那么触发子节点检查，否则跳过不检查，根节点一直会检查。
       
        /// 外部调用，相当于强制检查
        /// </summary>
        
        public void UpdateCheck()
        {
            if (root == this)
            {
                InBoxColliders.Clear();
                OutBoxColliders.Clear();
                updateCheckCountPerFrame = 0;
            }
            Profiler.BeginSample("_isTargetsInMyBox");
            var isInBox = _isTargetsInMyBox();
            Profiler.EndSample();

            if (root == this || isInBox || lastIsInBox || isFirst)
            {
                if (isCreateTrigger)
                {
                    for (int i = 0; i < xCellCount; i++)
                    {
                        for (int j = 0; j < yCellCount; j++)
                        {
                            for (int k = 0; k < zCellCount; k++)
                            {
                                myCells[i, j, k].UpdateCheck();
                                root.updateCheckCountPerFrame++;
                            }
                        }
                    }
                }
            }
            if (lastIsInBox != isInBox || isFirst)
            {
                if (!isCreateTrigger)
                {
                    Profiler.BeginSample("addHitCollidersEvent");
                    if (isInBox)
                    {
                        root.addHitCollidersEvent(this, true);
                    }
                    else
                    {
                        root.addHitCollidersEvent(this, false);
                        // recursiveHide();
                    }
                    Profiler.EndSample(); 
                }
            }

            isFirst = false;
            lastIsInBox = isInBox;
            if (root == this)
            {
                Profiler.BeginSample("externalActionLoop");
                for (int i = OutBoxColliders.Count - 1 ; i >= 0; i--)
                {
                    if (InBoxColliders.Contains(OutBoxColliders[i]))
                    {
                        continue;
                    }
                    root.externalAction?.Invoke(OutBoxColliders[i].hitColliders, false);
                }
                for (int i = InBoxColliders.Count - 1 ; i >= 0; i--)
                {
                    root.externalAction?.Invoke(InBoxColliders[i].hitColliders, true);
                }
                Profiler.EndSample();
                // Debug.LogError(root.updateCheckCountPerFrame);
            }
        }

        private void recursiveHide()
        {
            lastIsInBox = false;
            root.addHitCollidersEvent(this, false);
            if (isCreateTrigger)
            {
                for (int i = 0; i < xCellCount; i++)
                {
                    for (int j = 0; j < yCellCount; j++)
                    {
                        for (int k = 0; k < zCellCount; k++)
                        {
                            if (myCells[i, j, k].lastIsInBox)
                            {
                                myCells[i, j, k].recursiveHide();
                            }
                        }
                    }
                }
            }
        }

        private void addHitCollidersEvent(UnlimitedTriggerArea area, bool isInBox)
        {
            if (isInBox)
            {
                if (!InBoxColliders.Contains(area))
                {
                    InBoxColliders.Add(area);
                }
            }
            else
            {
                if (!OutBoxColliders.Contains(area))
                {
                    OutBoxColliders.Add(area);
                }
            }
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_isTargetsInMyBox())
            {

                if (Selection.activeObject == gameObject)
                {
                    Gizmos.color = new Color(1, 1, 1, 0.2f);
                    Gizmos.DrawCube(transform.position, size);
                }
                else
                {
                    Gizmos.color = new Color(0, 1 - 0.3f * level, 0, 0.2f + 0.3f * level);
                    Gizmos.DrawWireCube(transform.position, size);
                }

            }
        }
#endif

        public bool isTargetsInMyBox(List<Component> targets)
        {
            if (targets != null && targets.Count > 0)
            {
                bool isIn;
                for (int i = 0; i < targets.Count; i++)
                {
                    var target = targets[i];
                    if (target is MyBound)
                    {
                        Profiler.BeginSample("newBounds");
                        MyBound mb = (MyBound) target;
                        var bounds = new Bounds(mb.transform.position, mb.size);
                        Profiler.EndSample();
                        Profiler.BeginSample("IsBoundIntersect");
                        isIn = UnityTools.IsBoundIntersect(bounds, myBounds);
                        Profiler.EndSample();
                        if (isIn)
                        {
                            return true;
                        }
                    }
                    if (target is Collider)
                    {
                        Profiler.BeginSample("Get Collider bounds");
                        var tC = ((Collider) target);
                        var bounds = tC.bounds;
                        Profiler.EndSample();
                        Profiler.BeginSample("IsBoundIntersect");
                        isIn = UnityTools.IsBoundIntersect(bounds, myBounds);
                        Profiler.EndSample();
                        if (isIn)
                        {
                            return true;
                        }
                    }
                    if (target is Renderer)
                    {
                        isIn = UnityTools.IsBoundIntersect(((Renderer)target).bounds, myBounds);
                        if (isIn)
                        {
                            return true;
                        }
                    }
                    if (target is Transform)
                    {
                        var tPos = target.transform.position;
                        if (                  tPos.x >= xMin && tPos.x <= xMax 
                                           && tPos.y >= yMin && tPos.y <= yMax 
                                           && tPos.z >= zMin && tPos.z <= zMax)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }


        

        /// <summary>
        /// 获取在这个box中包含的所有物体的Collider，此方法检查方式是从上到下发射射线，有些法线向下的物体可能检查不到。另外被检测的物体属于 layerMask 指定层才会被检查到。
        /// </summary>
        /// <returns></returns>
        public List<Collider> GetObjectsInMyBox()
        {
            var hits = Physics.BoxCastAll(transform.position, size / 2, Vector3.down, Quaternion.identity, size.y * 0.1f,
                root.layerMask);
            List<Collider> hitColliders = new List<Collider>(hits.Length);
            for (int i = 0; i < hits.Length; i++) 
            {
                 hitColliders.Add(hits[i].collider);
            }

            return hitColliders;
        }

        /// <summary>
        /// 检查并保存检查得到的数据，存在 hitColliders 中
        /// </summary>
        public void CheckObjsInMyBox()
        {
            // 有子Trigger
            if (! isCreateTrigger)
            {
                hitColliders = GetObjectsInMyBox();
                root.externalAction?.Invoke(hitColliders, false);
                return;
            }
            
            for (int i = 0; i < xCellCount; i++)
            {
                for (int j = 0; j < yCellCount; j++)
                {
                    for (int k = 0; k < zCellCount; k++)
                    {
                        myCells[i, j, k].CheckObjsInMyBox();
                    }
                }
            }
        }
        
        private void OnValidate()
        {
            _boxCollider = gameObject.GetComponent<BoxCollider>();
            myCells = new UnlimitedTriggerArea[xCellCount, yCellCount, zCellCount];
            boxCollider.size = size;
            FindChildren();
            if (isCreateTrigger)
            {
                if (isEqualsEdge)
                {
                    CreateCellTrigger2();
                }
                else
                {
                    CreateCellTrigger();
                }
            }

            if (isTest)
            {
                print(GetObjectsInMyBox().Count);
            }
        }

        private void CreateCellTrigger()  
        {
            if (myCells != null && (xCellCount != myCells.GetLength(0) || 
                                          yCellCount != myCells.GetLength(1) ||
                                          zCellCount != myCells.GetLength(2)))
            {
                clearAllTrigger();
                myCells = new UnlimitedTriggerArea[xCellCount, yCellCount, zCellCount];
            }
            xCellCount = Mathf.Clamp(xCellCount, 1, xCellCount);
            yCellCount = Mathf.Clamp(yCellCount, 1, yCellCount);
            zCellCount = Mathf.Clamp(zCellCount, 1, zCellCount); 
            var halfSizeX = size.x * 0.5f;
            var halfSizeY = size.y * 0.5f;
            var halfSizeZ = size.z * 0.5f;
            var cellX = size.x / xCellCount;
            var cellY = size.y / yCellCount;
            var cellZ = size.z / zCellCount; 
            var halfCellX = 0.5f * cellX;
            var halfCellY = 0.5f * cellY;
            var halfCellZ = 0.5f * cellZ;
            var cellSize = new Vector3(cellX + offsetLength, cellY + offsetLength, cellZ + offsetLength); 
            for (int i = 0; i < xCellCount; i++)
            {
                for (int j = 0; j < yCellCount; j++)
                {
                    for (int k = 0; k < zCellCount; k++)
                    {
                        UnlimitedTriggerArea cell = myCells[i, j, k];
                        if (cell == null)
                        {
                            cell = new GameObject($"c{i}_{j}_{k}").AddComponent<UnlimitedTriggerArea>();
                            cell.transform.SetParent(transform);
                            cell.layerMask = layerMask;
                            myCells[i, j, k] = cell;
                        }
                        else
                        { 
                            cell = myCells[i, j, k];
                        }
                        
                        cell.transform.localPosition = new Vector3(
                            cellX*i + halfCellX - halfSizeX, 
                            cellY*j + halfCellY - halfSizeY, 
                            cellZ*k + halfCellZ - halfSizeZ);
                        cell.size = cellSize;
                        cell.boxCollider.size = cellSize;
                    }
                }
            }
        }
        
        private void CreateCellTrigger2()  
        {
            if (myCells != null && (xCellCount != myCells.GetLength(0) || 
                                          yCellCount != myCells.GetLength(1) ||
                                          zCellCount != myCells.GetLength(2)))
            {
                clearAllTrigger();
                myCells = new UnlimitedTriggerArea[xCellCount, yCellCount, zCellCount];
            }
            xCellCount = Mathf.Clamp(xCellCount, 1, xCellCount);
            yCellCount = Mathf.Clamp(yCellCount, 1, yCellCount);
            zCellCount = Mathf.Clamp(zCellCount, 1, zCellCount);

            var xl = size.x / xCellCount + 2 * offsetLength;
            var yl = size.y / yCellCount + 2 * offsetLength;
            var zl = size.z / zCellCount + 2 * offsetLength;
            xl = Mathf.Clamp(xl, 0, size.x);
            yl = Mathf.Clamp(yl, 0, size.y);
            zl = Mathf.Clamp(zl, 0, size.z);
            var cellSize = new Vector3(xl,yl,zl);
            var halfSizeX = size.x * 0.5f;
            var halfSizeY = size.y * 0.5f;
            var halfSizeZ = size.z * 0.5f;
            var xSpan = xCellCount == 1 ? 0 : (size.x - xl) / (xCellCount - 1);
            var ySpan = yCellCount == 1 ? 0 : (size.y - yl) / (yCellCount - 1);
            var zSpan = zCellCount == 1 ? 0 : (size.z - zl) / (zCellCount - 1);
            for (int i = 0; i < xCellCount; i++)
            {
                for (int j = 0; j < yCellCount; j++)
                {
                    for (int k = 0; k < zCellCount; k++)
                    {
                        UnlimitedTriggerArea cell = myCells[i, j, k];
                        if (cell == null)
                        {
                            cell = new GameObject($"c{i}_{j}_{k}").AddComponent<UnlimitedTriggerArea>();
                            cell.transform.SetParent(transform);
                            cell.layerMask = layerMask;
                            myCells[i, j, k] = cell;
                        }
                        else
                        { 
                            cell = myCells[i, j, k];
                        }
                        
                        cell.transform.localPosition = new Vector3(
                            0.5f * xl + i * xSpan - halfSizeX, 
                            0.5f * yl + j * ySpan - halfSizeY, 
                            0.5f * zl + k * zSpan - halfSizeZ);
                        cell.size = cellSize;
                        cell.boxCollider.size = cellSize;
                    }
                }
            }
            
        }

        private void FindChildren() 
        {
            if (! isCreateTrigger)
            {
                return;
            }

            if (myCells == null)
            {
                myCells = new UnlimitedTriggerArea[xCellCount,yCellCount,zCellCount];
            }
            if (transform.childCount > 0)
            {
                for (int i = 0; i < xCellCount; i++)
                {
                    for (int j = 0; j < yCellCount; j++)
                    {
                        for (int k = 0; k < zCellCount; k++)
                        {
                            myCells[i, j, k] = transform.Find($"c{i}_{j}_{k}").GetComponent<UnlimitedTriggerArea>();
                        }
                    }
                }
            }
        }

        private void clearAllTrigger()
        {
            if (myCells != null)
            {
                for (int i = 0; i < xCellCount; i++)
                {
                    for (int j = 0; j < yCellCount; j++)
                    {
                        for (int k = 0; k < zCellCount; k++)
                        {
                            var o = myCells[i, j, k].gameObject;
                            if (o != null)
                            {
                                Debug.Log("删除： " + o.name);
                                DestroyImmediate(o);
                            }
                        }
                    }
                }
            }
        
            myCells = null;
        }

    }
}