using System;
using System.Collections.Generic;
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
            GenerateParameters(methb, Program.Variables);
            GenerateStatement(Program.TopNode);

            il.Emit(OpCodes.Ret);
            typeBuilder.CreateType();
            modb.CreateGlobalFunctions();
            asmb.SetEntryPoint(methb);
            asmb.Save(moduleName);

            this.symbolTable = null;
            this.il = null;
        }

        private void GenerateParameters(MethodBuilder methb, Tree.Variable[] variables)
        {
            for (int i = 0; i < variables.Length; i++)
                methb.DefineParameter(i, ParameterAttributes.HasDefault, variables[i].Name);
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
                else
                    il.Emit(OpCodes.Ldc_I4, valueNode.Value);
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
                else if (m_ArgTable[variableNode.Variable.Name] == 3)
                    il.Emit(OpCodes.Ldarg_3);
                else if (m_ArgTable[variableNode.Variable.Name] == 4)
                {
                    il.Emit(OpCodes.Ldc_I4_4);
                    il.Emit(OpCodes.Ldarg_S);
                }
                else throw new ApplicationException("Unknown argument");
            }
            else if (node is Tree.FuncNode)
            {
                Tree.FuncNode funcNode = node as Tree.FuncNode;
                Tree.Func func = funcNode.Function;

                // For these simple maths functions, first emit the arguments
                if (func is Tree.FuncAdd || func is Tree.FuncSubtract || func is Tree.FuncMultiply || func is Tree.FuncModulo)
                {
                    GenerateStatement(funcNode.Children[0]);
                    GenerateStatement(funcNode.Children[1]);
                }

                if (func is Tree.FuncAdd)
                {
                    il.Emit(OpCodes.Add);
                }
                else if (func is Tree.FuncMultiply)
                {
                    il.Emit(OpCodes.Mul);
                }
                else if (func is Tree.FuncSubtract)
                {
                    il.Emit(OpCodes.Sub);
                }
                else if (func is Tree.FuncModulo)
                {
                    il.Emit(OpCodes.Rem);
                }
                else if (func is Tree.FuncIf)
                {
                    Tree.FuncIf funcIf = func as Tree.FuncIf;

                    // Output the first two statements - which we will be comparing
                    GenerateStatement(funcNode.Children[0]);
                    GenerateStatement(funcNode.Children[1]);
                    
                    Label equalLabel = il.DefineLabel();
                    Label afterLabel = il.DefineLabel();

                    OpCode conditionOpcode = OpCodes.Beq_S;
                    if(funcIf.Comparator == Tree.Comparator.Equal) 
                        conditionOpcode = OpCodes.Beq_S;
                    else if(funcIf.Comparator == Tree.Comparator.GreaterThanOrEqual) 
                        conditionOpcode = OpCodes.Bge_S;
                    else if(funcIf.Comparator == Tree.Comparator.GreaterThan) 
                        conditionOpcode = OpCodes.Bgt_S;
                    else
                        throw new ApplicationException();

                    // If [condition], go to next bit. Otherwise do instruction 3, then go to end
                    il.Emit(conditionOpcode, equalLabel);
                    GenerateStatement(funcNode.Children[3]);
                    il.Emit(OpCodes.Br_S, afterLabel);
                    il.MarkLabel(equalLabel);
                    GenerateStatement(funcNode.Children[2]);
                    il.MarkLabel(afterLabel);

                    LocalBuilder aLocal = il.DeclareLocal(typeof(Int32));
                    il.Emit(OpCodes.Stloc, aLocal);
                    il.Emit(OpCodes.Ldloc, aLocal);
                }
                else
                    throw new ApplicationException("Unknown function");
            }
            else
                throw new ApplicationException("Unknown node");
        }
    }
}
