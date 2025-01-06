using System;
using dnlib.DotNet; // https://github.com/0xd4d/dnlib
using System.Linq;

namespace SimpleObfuscator 
{
    class Program
    {
        private readonly Random _random; 
        private string[] _randNameList;

        internal Program()
        {
            _random = new Random();
            _randNameList = [];
        }

        internal string GenerateRandomName()
        {
            int length = 10;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];
            string randName;
            do
            {
                for (int i = 0; i < length; i++)
                {
                    stringChars[i] = chars[_random.Next(chars.Length)];
                }
                randName = new string(stringChars);
            }
            while (_randNameList.Contains(randName));
            _randNameList.Append(randName);

            return randName;
        }

        internal void NameObfuscate(ModuleDefMD module, string nameSpace)
        {
            foreach (TypeDef type in module.Types)
            {
                if (!(type.IsGlobalModuleType || type.IsSpecialName || type.IsWindowsRuntime || type.IsRuntimeSpecialName || type.IsInterface))
                {
                    foreach (PropertyDef property in type.Properties)
                    {
                        if (!property.IsRuntimeSpecialName)
                            property.Name = GenerateRandomName();
                    }
                    foreach (FieldDef fields in type.Fields)
                    {
                        fields.Name = GenerateRandomName();
                    }
                    foreach (MethodDef method in type.Methods)
                    {
                        if (!(method.IsVirtual || method.IsRuntime || method.IsRuntimeSpecialName || method.IsStaticConstructor || method.IsConstructor))
                            method.Name = GenerateRandomName();
                        
                    }
                    type.Name = GenerateRandomName();
                }
                type.Namespace = GenerateRandomName();
            }
        }

        static int Main(string[] args)
        {
            string pathIn = "";
            string pathOut = "";
            string namespaceName = "";

            Program program = new Program();
            ModuleDefMD module = ModuleDefMD.Load(pathIn);
            program.NameObfuscate(module, namespaceName);
            module.Write(pathOut);

            return 0;
        }
    }
}
