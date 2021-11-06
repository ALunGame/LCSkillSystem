using System;
using System.Collections.Generic;
using System.Reflection;
using Timeline.Player;
using UnityEngine;
using XPToolchains.Help;

namespace Timeline
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class TimelineViewAttribute : Attribute
    {
        public Type dataType;
        public Type playerType;

        /// <summary>
        /// 时间轴界面
        /// </summary>
        /// <param name="_dataType">数据类</param>
        /// <param name="_playerType">播放类</param>
        public TimelineViewAttribute(Type _dataType, Type _playerType = null)
        {
            dataType = _dataType;
            playerType = _playerType;
            if (playerType == null)
                playerType = typeof(BasePlayer);
        }
    }

    public class BaseTimelineView : BaseView
    {
        private object data;

        /// <summary>
        /// 数据
        /// </summary>
        public object Data
        {
            get
            {
                if (data == null)
                    data = new object();
                return data;
            }
        }

        private BasePlayer player;

        /// <summary>
        /// 播放器
        /// </summary>
        public BasePlayer Player
        {
            get
            {
                if (player == null)
                    player = new BasePlayer(this);
                return player;
            }
        }

        public void SetData(object _data)
        {
            data = _data;
        }

        #region Static

        //数据和显示绑定
        private static Dictionary<Type, Type> dataViewDict = new Dictionary<Type, Type>();

        private static bool isCollect = false;

        private static void CollectType()
        {
            if (isCollect)
                return;
            isCollect = true;
            dataViewDict.Clear();
            var viewTypeList = ReflectionHelper.GetChildTypes<BaseTimelineView>();
            foreach (var item in viewTypeList)
            {
                if (AttributeHelper.TryGetTypeAttribute(item, out TimelineViewAttribute trackAttribute))
                {
                    Type dataType = trackAttribute.dataType;
                    dataViewDict.Add(dataType, item);
                }
            }
        }

        public static BaseTimelineView CreateView(Type viewType, object data = null)
        {
            TimelineViewAttribute viewAttribute;
            if (!AttributeHelper.TryGetTypeAttribute(viewType, out viewAttribute))
            {
                Debug.LogWarning($"时间轴视图必须加上属性{viewType}");
                return null;
            }

            BaseTimelineView timelineView = Activator.CreateInstance(viewType, true) as BaseTimelineView;

            //数据
            object dataValue;
            if (data == null)
            {
                Type dataType = viewAttribute.dataType;
                dataValue = Activator.CreateInstance(dataType);
            }
            else
            {
                dataValue = data;
            }
            FieldInfo dataFieldInfo = ReflectionHelper.GetFieldInfo(viewType, "data");
            dataFieldInfo.SetValue(timelineView, dataValue);

            //播放器
            Type playerType = viewAttribute.playerType;
            object playerValue = Activator.CreateInstance(playerType, timelineView);
            FieldInfo playerFieldInfo = ReflectionHelper.GetFieldInfo(viewType, "player");
            playerFieldInfo.SetValue(timelineView, playerValue);

            return timelineView;
        }

        public static T CreateView<T>() where T : BaseTimelineView
        {
            return (T)CreateView(typeof(T));
        }

        public static BaseTimelineView CreateView(object viewData)
        {
            CollectType();
            Type dataType = viewData.GetType();
            Type viewType;
            if (dataViewDict.TryGetValue(dataType, out viewType))
            {
                return CreateView(viewType, viewData);
            }
            return null;
        }

        #endregion Static
    }
}