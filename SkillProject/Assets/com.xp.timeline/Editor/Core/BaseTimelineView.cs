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

        public void SetData(object _data)
        {
            data = _data;
        }

        #region Static

        //���ݺ���ʾ��
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
                Debug.LogWarning($"ʱ������ͼ�����������{viewType}");
                return null;
            }

            BaseTimelineView timelineView = Activator.CreateInstance(viewType, true) as BaseTimelineView;

            //����
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