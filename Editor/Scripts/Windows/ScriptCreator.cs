using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace VirrVarr.Editor.Windows
{
    public class ScriptCreatorSettingsBase
    {
        public virtual void DisplaySettings()
        {
        }

        public virtual void GenerateCode( string scriptName, string scriptPath )
        {
        }
    }

    public class ScriptCreatorBehaviourSettings : ScriptCreatorSettingsBase
    {
        bool genFunc_start = true;
        bool genFunc_update = true;
        bool genFunc_fixedUpdate = false;

        bool default_shouldTick = true;

        public override void DisplaySettings()
        {
            EditorGUILayout.LabelField("Functions:");
            genFunc_start = EditorGUILayout.Toggle("Start:", genFunc_start);
            genFunc_update = EditorGUILayout.Toggle("Update:", genFunc_update);
            genFunc_fixedUpdate = EditorGUILayout.Toggle("Fixed Update:", genFunc_fixedUpdate);

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Behaviour defaults:");
            default_shouldTick = EditorGUILayout.Toggle("Should tick:", default_shouldTick);
        }
        public override void GenerateCode(string scriptName, string scriptPath)
        {
            CodeGenerator codeGen = new CodeGenerator(scriptPath);
            var classGen = codeGen.AddClass(scriptName).SetParentClass("VirrVarr.BehaviourBase");

            if (genFunc_start)
            {
                var startFunc = classGen.AddFunction("Behaviour_Start")
                    .SetAccessType(ECodeAccessType.Protected)
                    .SetFunctionType(CodeGeneratorFunctionHandle.EFunctionType.Override)
                    .SetFunctionDescComment("Called before the first update");

                if(!default_shouldTick)
                {
                    startFunc.AddLine("enabled = false;");
                }
            }
            if (genFunc_update)
            {
                classGen.AddFunction("Behaviour_Update")
                    .SetAccessType(ECodeAccessType.Protected)
                    .SetFunctionType(CodeGeneratorFunctionHandle.EFunctionType.Override)
                    .SetFunctionDescComment("Called every tick if enabled");
            }
            if (genFunc_fixedUpdate)
            {
                classGen.AddFunction("Behaviour_FixedUpdate")
                    .SetAccessType(ECodeAccessType.Protected)
                    .SetFunctionType(CodeGeneratorFunctionHandle.EFunctionType.Override)
                    .SetFunctionDescComment("Called at a constant rate if enabled");
            }

            classGen.Generate();
            codeGen.FinalizeGeneration();
        }
    }
    public class ScriptCreatorScripObjSettings : ScriptCreatorSettingsBase
    {
        string createMenuName = "";

        public override void DisplaySettings()
        {
            EditorGUILayout.LabelField("Create menu name (leave empty for default)");
            createMenuName = EditorGUILayout.TextField("Create Menu:", createMenuName);
        }

        public override void GenerateCode( string scriptName, string scriptPath )
        {
            string createMenuNameStr = createMenuName.Length != 0 ? createMenuName : string.Format("Game/{0}", scriptName); 

            CodeGenerator codeGen = new CodeGenerator(scriptPath);
            var classGen = codeGen.AddClass(scriptName).SetParentClass("ScriptableObject");
            classGen.AddAttribute("CreateAssetMenu").AddAttributeData(string.Format("menuName = \"{0}\"", createMenuNameStr));
            classGen.Generate();
            codeGen.FinalizeGeneration();
        }
    }

    public class ScriptCreatorWindow : EditorWindow
    {
        string scriptName;
        bool showAdvanced = false;
        ECodeType type = ECodeType.Behaviour;
        Dictionary<ECodeType, ScriptCreatorSettingsBase> settings = new Dictionary<ECodeType, ScriptCreatorSettingsBase>();

        private void OnGUI()
        {
            titleContent = new GUIContent("Create Script");

            if(settings.Count == 0)
            {
                settings.Add(ECodeType.Behaviour, new ScriptCreatorBehaviourSettings());
                settings.Add(ECodeType.ScriptableObject, new ScriptCreatorScripObjSettings());
            }

            string selectionPath = EditorUtilities.GetSelectionDirectoryPath();

            scriptName = EditorGUILayout.TextField("Script Name:", scriptName);
            EditorGUILayout.TextField("Create in:", selectionPath);
            type = (ECodeType)EditorGUILayout.EnumPopup("Script type:", type);

            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Settings:");

            if (showAdvanced)
            {
                settings[type].DisplaySettings();
            }

            if (GUILayout.Button("Create"))
            {
                if (!selectionPath.EndsWith("/"))
                {
                    selectionPath += "/";
                }

                string fullScriptPath = selectionPath + scriptName + ".cs";
                System.IO.File.Create(fullScriptPath).Close();

                settings[type].GenerateCode(scriptName, fullScriptPath);

                AssetDatabase.Refresh();
                Close();
            }
        }
    }
}
