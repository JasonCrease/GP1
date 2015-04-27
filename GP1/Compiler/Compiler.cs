using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Reflection;

namespace GP1.Compiler
{
    class Compiler
    {
        System.Reflection.Emit.ILGenerator il = null;
        System.Collections.Generic.Dictionary<string, LocalBuilder> symbolTable;

        public void Compile(Program Program, string moduleName)
        {
            if (System.IO.Path.GetFileName(moduleName) != moduleName)
                throw new Exception("can only output into current directory!");

            AssemblyName name = new AssemblyName(System.IO.Path.GetFileNameWithoutExtension(moduleName));
            AssemblyBuilder asmb = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Save);
            ModuleBuilder modb = asmb.DefineDynamicModule(moduleName);
            TypeBuilder typeBuilder = modb.DefineType("Program");
            MethodBuilder methb = typeBuilder.DefineMethod("Main", MethodAttributes.Static, typeof(void), System.Type.EmptyTypes);

            // CodeGenerator 
            this.il = methb.GetILGenerator();
            this.symbolTable = new Dictionary<string, LocalBuilder>();

            // Go Compile 
            GenerateStatement(Program.TopNode);

            il.Emit(OpCodes.Ret);
            typeBuilder.CreateType();
            modb.CreateGlobalFunctions();
            asmb.SetEntryPoint(methb);
            asmb.Save(moduleName);

            this.symbolTable = null;
            this.il = null;
        }

        private void GenerateStatement(Tree.Node node)
        {
            if (node is Tree.ValueNode)
            {
                Tree.ValueNode valueNode = node as Tree.ValueNode;
            }
            else if (node is Tree.VariableNode)
            {
                Tree.VariableNode variableNode = node as Tree.VariableNode;

            }
            else if (node is Tree.FunctionNode)
            {
                Tree.FunctionNode functionNode = node as Tree.FunctionNode;

            }
            else
                throw new ApplicationException();
        }
    }
}
