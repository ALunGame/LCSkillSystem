using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Timeline.Player;
using UnityEditor;
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

        public static BaseTimelineView CreateView(Type viewType)
        {
            TimelineViewAttribute viewAttribute;
            if (!AttributeHelper.TryGetTypeAttribute(viewType, out viewAttribute))
            {
                Debug.LogWarning($"时间轴视图必须加上属性{viewType}");
                return null;
            }

            BaseTimelineView timelineView = Activator.CreateInstance(viewType, true) as BaseTimelineView;

            //数据
            Type dataType = viewAttribute.dataType;
            object dataValue = Activator.CreateInstance(dataType);
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
    }
}