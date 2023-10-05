using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mathf;
using Nebula.Game;

namespace NST
{
    public class TestProperty : Property
    {
        public int i;
        public float f;
        public string s;
        public Mathf.Vector3 v;
    }

    public class Tests
    {
        public class InternalCalls
        {
            [MethodImplAttribute(MethodImplOptions.InternalCall)]
            public static extern void TestPassVec2(in Mathf.Vector2 vec);
            [MethodImplAttribute(MethodImplOptions.InternalCall)]
            public static extern void TestPassVec3(in Mathf.Vector3 vec);
            [MethodImplAttribute(MethodImplOptions.InternalCall)]
            public static extern void TestPassVec4(in Mathf.Vector4 vec);

            public static void RunTests()
            {
                    Vector2 vec2 = new Vector2(1,2);
                    TestPassVec2(vec2);
                    Vector3 vec3 = new Vector3(1, 2, 3);
                    TestPassVec3(vec3);
                    Vector4 vec4 = new Vector4(1, 2, 3, 4);
                    TestPassVec4(vec4);
            }
        }

        public class DLLImportCalls
        {
            [DllImport("__Internal", EntryPoint = "PassString")]
            public static extern void PassString([MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Util.StringMarshaler))] string val);

            [DllImport("__Internal", EntryPoint = "TestArrayOfInt")]
            public static extern void TestArrayOfInt(int[] arr);
            public static void RunTests()
            {
                string myStr = "This is a C# string!\n";
                PassString(myStr);

                int[] arr = new int[10];
                for (int i = 0; i < arr.Length; i++)
                { 
                    arr[i] = i + 1;
                }
                //TestArrayOfInt(arr);
            }
        }
    }

    public class AppEntry
    {
        static public void Main()
        {
            Console.Write("Console.Write works!\n");
            Console.WriteLine("Console.WriteLine works!");
            Nebula.Debug.Log("Nebula.Debug.Log works!\n");

            // Test properties
            TestProperty testProp0 = new TestProperty();
            PropertyManager.Instance.RegisterProperty(testProp0);

            Entity entity = new Entity();
            entity.AddProperty(testProp0);


            PropertyManager.Instance.PrintAllProperties();
        }
    }
}
