using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Timeline.View
{
    //添加轨道
    public class AddTrackView : BaseTimelineView
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

        void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(AddContent, FocusType.Passive, "Dropdown"))
            {
                GenCustomMenu();
            }
        }

        public void GenCustomMenu()
        {
            GenericMenu pm = new GenericMenu();
            var paste = EditorGUIUtility.TrTextContent("粘贴\t #p");
            pm.AddItem(paste, false, PasteTrack);

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void PasteTrack()
        {
        }

        private void OnAddTrackItem(object arg)
        {
            Type type = (Type)arg;
        }
    }
}
