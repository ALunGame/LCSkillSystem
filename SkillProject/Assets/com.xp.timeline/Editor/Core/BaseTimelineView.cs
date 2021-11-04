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
        /// ʱ�������
        /// </summary>
        /// <param name="_dataType">������</param>
        /// <param name="_playerType">������</param>
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
        /// ����
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
        /// ������
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
                Debug.LogWarning($"ʱ������ͼ�����������{viewType}");
                return null;
            }

            BaseTimelineView timelineView = Activator.CreateInstance(viewType, true) as BaseTimelineView;

            //����
            Type dataType = viewAttribute.dataType;
            object dataValue = Activator.CreateInstance(dataType);
            FieldInfo dataFieldInfo = ReflectionHelper.GetFieldInfo(viewType, "data");
            dataFieldInfo.SetValue(timelineView, dataValue);

            //������
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