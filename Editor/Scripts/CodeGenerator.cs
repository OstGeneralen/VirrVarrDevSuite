using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirrVarr.Editor
{
    public enum ECodeType
    {
        Behaviour,
        ScriptableObject,
    }

    public enum ECodeAccessType
    {
        Public,
        Private,
        Protected
    }

    public class CodeGeneratorClassHandle
    {
        string className = "";
        string parentClassName = "";
        string namespaceName = "";
        CodeGenerator generator;
        List<CodeGeneratorFunctionHandle> functions;
        List<CodeGeneratorAttributeHandle> attributes;

        public CodeGeneratorClassHandle( string name, CodeGenerator ownerGenerator )
        {
            className = name;
            generator = ownerGenerator;
            functions = new List<CodeGeneratorFunctionHandle>();
            attributes = new List<CodeGeneratorAttributeHandle>();
        }

        public CodeGeneratorClassHandle SetParentClass( string parentName )
        {
            parentClassName = parentName;
            return this;
        }
        public CodeGeneratorFunctionHandle AddFunction( string functionName )
        {
            functions.Add(new CodeGeneratorFunctionHandle(functionName, generator));
            return functions[functions.Count - 1];
        }
        public CodeGeneratorAttributeHandle AddAttribute( string attributeName )
        {
            attributes.Add(new CodeGeneratorAttributeHandle(attributeName, generator));
            return attributes[attributes.Count - 1];
        }

        public void SetNamespace(string namespaceStr)
        {
            namespaceName = namespaceStr;
        }

        public void Generate()
        {
            bool hasNamespace = namespaceName.Length != 0;
            bool hasParent = parentClassName.Length != 0;

            if (hasNamespace)
            {
                generator.WriteLine("namespace {0}", namespaceName);
                generator.StartScope();
            }

            foreach (var attr in attributes)
            {
                attr.Generate();
            }

            if(hasParent)
            {
                generator.WriteLine("public class {0} : {1}", className, parentClassName);
            }
            else
            {
                generator.WriteLine("public class {0}", className);
            }

            generator.StartScope();

            foreach(var funcHandle in functions)
            {
                funcHandle.Generate();
            }

            generator.EndScope();

            if(hasNamespace)
            {
                generator.EndScope();
            }
        }
    }
    public class CodeGeneratorFunctionHandle
    {
        public enum EFunctionType
        {
            Default,
            Override,
        }

        string functionDescComment = "";
        string functionName;
        List<string> bodyLines;
        EFunctionType type = EFunctionType.Default;
        ECodeAccessType accessType = ECodeAccessType.Private;
        CodeGenerator generator;

        public CodeGeneratorFunctionHandle(string name, CodeGenerator ownerGenerator)
        {
            functionName = name;
            generator = ownerGenerator;
            bodyLines = new List<string>();
        }

        public CodeGeneratorFunctionHandle SetAccessType( ECodeAccessType setAccessType )
        {
            accessType = setAccessType;
            return this;
        }

        public CodeGeneratorFunctionHandle SetFunctionType( EFunctionType setType )
        {
            type = setType;
            return this;
        }

        public CodeGeneratorFunctionHandle SetFunctionDescComment( string commentText )
        {
            functionDescComment = commentText;
            return this;
        }

        public void AddLine( string line )
        {
            bodyLines.Add(line);
        }

        public void Generate()
        {
            string accessorStr = "";
            if (accessType == ECodeAccessType.Public)
                accessorStr = "public";
            if (accessType == ECodeAccessType.Private)
                accessorStr = "private";
            if (accessType == ECodeAccessType.Protected)
                accessorStr = "protected";

            string funcTypeStr = "";
            if (type == EFunctionType.Override)
                funcTypeStr = "override";

            if(functionDescComment.Length != 0)
            {
                generator.WriteLine("// {0}", functionDescComment);
            }

            generator.WriteLine( "{0} {1} void {2}()", accessorStr, funcTypeStr, functionName );
            generator.StartScope();
            foreach (var line in bodyLines)
            {
                generator.WriteLine(line);
            }
            generator.EndScope();
        }
    }
    public class CodeGeneratorAttributeHandle
    {
        string name;
        CodeGenerator generator;
        List<string> data;

        public CodeGeneratorAttributeHandle( string attributeName, CodeGenerator ownerGenerator )
        {
            name = attributeName;
            generator = ownerGenerator;
            data = new List<string>();
        }

        public CodeGeneratorAttributeHandle AddAttributeData( string dataLine )
        {
            data.Add(dataLine);
            return this;
        }

        public void Generate()
        {
            generator.WriteLineStart("[");
            generator.Write(name);

            if(data.Count > 0)
            {
                generator.Write("( ");
                for(int i = 0; i < data.Count; ++i)
                {
                    if(i + 1 < data.Count)
                    {
                        generator.Write(data[i] + ",");
                    }
                    else
                    {
                        generator.Write(data[i]);
                    }
                }
                generator.Write(" )");
            }

            generator.WriteLineEnd("]");
        }
    }


    public class CodeGenerator
    {
        System.Text.StringBuilder generatedCode;
        string scriptFilePath;
        string tabDepthStr = "";
        int tabDepth = 0;

        public CodeGenerator(string filePath)
        {
            scriptFilePath = filePath;
            generatedCode = new System.Text.StringBuilder();
            GenerateUsingDirs();
        }

        public void FinalizeGeneration()
        {
            System.IO.File.WriteAllText(scriptFilePath, generatedCode.ToString());
        }
        public CodeGeneratorClassHandle AddClass(string name)
        {
            return new CodeGeneratorClassHandle(name, this);
        }
        private void GenerateUsingDirs()
        {
            generatedCode.Append("using System.Collections;\r\n");
            generatedCode.Append("using System.Collections.Generic;\r\n");
            generatedCode.Append("using UnityEngine;\r\n");
            generatedCode.Append("\r\n");
        }
        public void WriteLine( string lineFmt, params object[] fmt )
        {
            WriteLineStart(lineFmt, fmt);
            WriteLineEnd("");
        }
        public void WriteLineStart( string textFmt, params object[] fmt )
        {
            Write(string.Format("{0}{1}", tabDepthStr, string.Format(textFmt, fmt)));
        }
        public void WriteLineEnd( string textFmt, params object[] fmt )
        {
            Write(string.Format("{0}\r\n", string.Format(textFmt, fmt)));
        }
        public void Write( string textFmt, params object[] fmt )
        {
            generatedCode.Append(string.Format(textFmt, fmt));
        }
        public void IncreaseTab()
        {
            tabDepth = tabDepth + 1;
            BuildTabDepthStr();
        }
        public void DecreaseTab()
        {
            tabDepth = tabDepth - 1;
            BuildTabDepthStr();
        }
        public void StartScope()
        {
            string scopeStart = "{";
            generatedCode.Append(tabDepthStr + scopeStart + "\r\n");
            IncreaseTab();
        }
        public void EndScope()
        {
            DecreaseTab();
            string scopeEnd = "}";
            generatedCode.Append(tabDepthStr + scopeEnd + "\r\n");
        }
        void BuildTabDepthStr()
        {
            tabDepthStr = "";
            for(int i = 0; i < tabDepth; ++i)
            {
                tabDepthStr += "\t";
            }
        }
    }
}
