using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VirrVarr.Editor
{
    class ListEditorItFunc
    {
        public virtual int GetSize()
        {
            return -1;
        }

        public virtual void DisplayEditField(int index)
        {
        }

        public virtual void RemoveAtIndex(int index)
        {
        }

        public virtual void SwapIndexes(int indexA, int indexB)
        {
        }

        public virtual void AddNew()
        {
        }
    }

    class ListEditorItFunc_String : ListEditorItFunc
    {
        public List<string> StringList { get { return list; } }
        List<string> list;

        public ListEditorItFunc_String(List<string> originalList)
        {
            list = originalList;
        }

        public override int GetSize()
        {
            return list.Count;
        }

        public override void DisplayEditField(int index)
        {
            list[index] = EditorGUILayout.TextField(list[index]);
        }

        public override void RemoveAtIndex(int index)
        {
            list.RemoveAt(index);
        }

        public override void SwapIndexes(int indexA, int indexB)
        {
            string intermediate = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = intermediate;
        }

        public override void AddNew()
        {
            list.Add("New String");
        }
    }



    public class ListEditor
    {
        public static void Display(ref List<string> editList)
        {
            ListEditorItFunc_String stringItFunc = new ListEditorItFunc_String(editList);
            DoDisplay(stringItFunc);
            editList = stringItFunc.StringList;
        }

        private static void DoDisplay(ListEditorItFunc itFunc)
        {
            int removeIndex = -1;
            int swapIndexA = -1;
            int swapIndexB = -1;

            for (int i = 0; i < itFunc.GetSize(); ++i)
            {
                using (var horizontalScope = new EditorGUILayout.HorizontalScope())
                {
                    itFunc.DisplayEditField(i);
                    if (i != 0)
                    {
                        if (GUILayout.Button("Move Up", GUILayout.MaxWidth(80)))
                        {
                            swapIndexA = i - 1;
                            swapIndexB = i;
                        }
                    }
                    else
                    {
                        GUILayout.Space(84);
                    }
                    if (GUILayout.Button("Remove", GUILayout.MaxWidth(80)))
                    {
                        removeIndex = i;
                    }
                }
            }

            if(GUILayout.Button("Insert", GUILayout.MaxWidth(80)))
            {
                itFunc.AddNew();
            }


            if (swapIndexA != -1 && swapIndexB != -1)
            {
                itFunc.SwapIndexes(swapIndexA, swapIndexB);
            }
            if (removeIndex != -1)
            {
                itFunc.RemoveAtIndex(removeIndex);
            }
        }
    }
}
