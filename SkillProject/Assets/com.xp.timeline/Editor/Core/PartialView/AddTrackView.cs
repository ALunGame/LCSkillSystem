using System;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;

namespace Timeline.View
{
    //添加轨道
    public class AddTrackView : BaseView
    {
        private GUIContent AddContent;

        public override void OnInit()
        {
            AddContent = EditorGUIUtility.TrTextContent("Add", "Add new tracks.");
        }

        public override void OnDraw()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(GUILayout.Width(window.AddTrackSize.width));
            AddButtonGUI();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(AddContent, FocusType.Passive, "Dropdown"))
            {
                GenCustomMenu();
            }
        }

        public void GenCustomMenu()
        {
            GenericMenu pm = new GenericMenu();

            var trackList = ReflectionHelper.GetChildTypes<BaseTrackView>();
            foreach (var item in trackList)
            {
                TimelineTrackAttribute trackAttribute = null;
                if (AttributeHelper.TryGetTypeAttribute(item, out trackAttribute))
                {
                    string menuName = trackAttribute.menuName;
                    var paste = EditorGUIUtility.TrTextContent(menuName);
                    pm.AddItem(paste, false, () =>
                    {
                        OnAddTrackItem(item);
                    });
                }
            }

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void OnAddTrackItem(Type track)
        {
            BaseSequenceView sequenceView = window.GetPartialView<BaseSequenceView>();
            BaseTimelineView trackView = BaseTimelineView.CreateView(track);
            sequenceView.AddTrackView(trackView as BaseTrackView);
        }
    }
}