//using com.bball.view.returnHelper;
//using UnityEngine;
//
//namespace com.bball.core.single {
//    public abstract class GameSingleBhv<T> : SingleBhv<T>, IReturnLoadData where T : MonoBehaviour {
//        protected bool isGameInited;
//
//        /// <summary>
//        /// 原路跳转到此界面/返回此界面时会调用此方法,
//        /// 实现此方法的任务是:根据object[] parameters的不同如何调用loadData方法
//        /// </summary>
//        /// <param name="parameters"></param>
//        public abstract void onReturnLoadData(params object[] parameters);
//
//        /// <summary>
//        /// 此次界面离开时会调用此方法,
//        /// 任务是:保存界面上的数据到object[]中,onReturnLoadData相对应
//        /// </summary>
//        /// <returns></returns>
//        public abstract object[] setParamsOnJumpToOtherView();
//
//        public virtual void loadData() {
//            if (!isGameInited) {
//                initData();
//                initEvent();
//                isGameInited = true;
//            }
//            initUI();
//        }
//
//        /// <summary>
//        /// 初始化UI，任务是寻找组件、处理语言本地化
//        /// </summary>
//        public abstract void initData();
//
//        /// <summary>
//        /// 初始化事件
//        /// </summary>
//        public abstract void initEvent();
//
//        /// <summary>
//        /// 初始化UI，任务是将具体的数据放入UI中
//        /// </summary>
//        public abstract void initUI();
//    }
//}
