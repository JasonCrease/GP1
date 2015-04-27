using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace GP1.Compiler
{
    public class Compiler
    {
        System.Reflection.Emit.ILGenerator il = null;
        Dictionary<string, LocalBuilder> symbolTable;
        Dictionary<string, int> m_ArgTable = new Dictionary<string, int>();

        public void Compile(Program Program, string moduleName)
        {
            if (System.IO.Path.GetFileName(moduleName) != moduleName)
                throw new Exception("can only output into current directory!");

            // Deal with arguments
            Type[] argTypes = new Type[Program.Variables.Length];
            for (int i = 0; i < Program.Variables.Length; i++)
            {
                argTypes[i] = typeof(int);
                m_ArgTable.Add(Program.Variables[i].Name, i);
            }

            AssemblyName name = new AssemblyName(System.IO.Path.GetFileNameWithoutExtension(moduleName));
            AssemblyBuilder asmb = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
            ModuleBuilder modb = asmb.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = modb.DefineType("Program");
            MethodBuilder methb = typeBuilder.DefineMethod("Function", MethodAttributes.Static | MethodAttributes.Public, typeof(int), argTypes);

            // CodeGenerator 
            this.il = methb.GetILGenerator();
            this.symbolTable = new Dictionary<string, LocalBuilder>();

            // Go Compile 
            GenerateParameters(Program.Variables);
            GenerateStatement(Program.TopNode);

            il.Emit(OpCodes.Ret);
            typeBuilder.CreateType();
            modb.CreateGlobalFunctions();
            asmb.SetEntryPoint(methb);
            asmb.Save(moduleName);

            this.symbolTable = null;
            this.il = null;
        }

        private void GenerateParameters(Tree.Variable[] variables)
        {

        }

        private void GenerateStatement(Tree.Node node)
        {
            if (node is Tree.ValueNode)
            {
                Tree.ValueNode valueNode = node as Tree.ValueNode;

                if (valueNode.Value == 0)
                    il.Emit(OpCodes.Ldc_I4_0);
                else if (valueNode.Value == 1)
                    il.Emit(OpCodes.Ldc_I4_1);
                else if (valueNode.Value == 2)
                    il.Emit(OpCodes.Ldc_I4_2);
                else if (valueNode.Value == 3)
                    il.Emit(OpCodes.Ldc_I4_3);
                else if (valueNode.Value == 4)
                    il.Emit(OpCodes.Ldc_I4_4);
                else throw new ApplicationException("Unknown value");
            }
            else if (node is Tree.VariableNode)
            {
                Tree.VariableNode variableNode = node as Tree.VariableNode;

                if (m_ArgTable[variableNode.Variable.Name] == 0)
                    il.Emit(OpCodes.Ldarg_0);
                else if (m_ArgTable[variableNode.Variable.Name] == 1)
                    il.Emit(OpCodes.Ldarg_1);
                else if (m_ArgTable[variableNode.Variable.Name] == 2)
                    il.Emit(OpCodes.Ldarg_2);
                else throw new ApplicationException("Unknown argument");
            }
            else if (node is Tree.FuncNode)
            {
                Tree.FuncNode funcNode = node as Tree.FuncNode;

                foreach(Tree.Node child in funcNode.Children)
                    GenerateStatement(child);

                if (funcNode.Function is Tree.FuncAdd)
                    il.Emit(OpCodes.Add);
                else if (funcNode.Function is Tree.FuncMultiply)
                    il.Emit(OpCodes.Mul);
                else if (funcNode.Function is Tree.FuncSubtract)
                    il.Emit(OpCodes.Sub);
                else if (funcNode.Function is Tree.FuncModulo)
                    il.Emit(OpCodes.Rem);
                else
                    throw new ApplicationException("Unknown function");
            }
            else
                throw new ApplicationException("Unknown node");
        }
    }
}
