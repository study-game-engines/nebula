using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using ConsoleHook;
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

    class AudioEmitterProperty : Property
    {
        public bool autoplay;
        public bool loop;
        public float volume;
        public float pitch;

        public override void OnActivate()
        {
            base.OnActivate();

            // Do on activate stuff
        }

        public override void OnMessage(in Msg msg)
        {

        }

        public override void OnBeginFrame()
        {
            base.OnBeginFrame();

            Mathf.Matrix t = this.Entity.GetTransform();
            AudioEmitterProperty prop = this.Entity.GetProperty<AudioEmitterProperty>();

            // Do on frame stuff
        }

        public override System.Type[] AcceptedMessages()
        {
            return new[]
            {
                    typeof(PlayAudioMessage)
                };
        }

        public override Nebula.Game.Events[] AcceptedEvents()
        {
            return new[] {
                Nebula.Game.Events.OnActivate,
                Nebula.Game.Events.OnFrame
            };
        }
    }

    public class PlayAudioMessage : Msg
    {
        public float volume;
        public bool looping;
    }

    public class Tests
    {
        [DllImport("__Internal", EntryPoint = "VerifyManaged")]
        private static extern void VERIFY(bool success, string filePath, int lineNumber);

        public static void Verify(bool test,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0)
        {
            VERIFY(test, filePath, lineNumber);
        }

        public class VariablePassing
        {
            [DllImport("__Internal", EntryPoint = "PassVec2"), SuppressUnmanagedCodeSecurity]
            public static extern void TestPassVec2(in Mathf.Vector2 vec);
            [DllImport("__Internal", EntryPoint = "PassVec3"), SuppressUnmanagedCodeSecurity]
            public static extern void TestPassVec3(in Mathf.Vector3 vec);
            [DllImport("__Internal", EntryPoint = "PassVec4"), SuppressUnmanagedCodeSecurity]
            public static extern void TestPassVec4(in Mathf.Vector4 vec);

            [UnmanagedCallersOnly]
            public static void RunTests()
            {
                Vector2 vec2 = new Vector2(1, 2);
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
            public static extern void TestArrayOfInt(int[] arr, int size);

            [DllImport("__Internal", EntryPoint = "TestArrayOfVec3")]
            public static extern void TestArrayOfVec3(Mathf.Vector3[] arr, int size);

            [DllImport("__Internal", EntryPoint = "TestArrayOfVec4")]
            public static extern void TestArrayOfVec4(Mathf.Vector4[] arr, int size);

            [UnmanagedCallersOnly]
            public static void RunTests()
            {
                string myStr = "This is a C# string!\n";
                PassString(myStr);

                int[] arr = new int[10];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = i + 1;
                }
                TestArrayOfInt(arr, arr.Length);

                Mathf.Vector3[] vec3Arr = new Mathf.Vector3[10];
                for (int i = 0; i < vec3Arr.Length; i++)
                {
                    vec3Arr[i] = new Vector3(1, 2, 3);
                }
                TestArrayOfVec3(vec3Arr, vec3Arr.Length);


                Mathf.Vector4[] vec4Arr = new Mathf.Vector4[10];
                for (int i = 0; i < vec4Arr.Length; i++)
                {
                    vec4Arr[i] = new Vector4(1, 2, 3, 4);
                }
                TestArrayOfVec4(vec4Arr, vec4Arr.Length);
            }
        }
    }

    public class AppEntry
    {
        [UnmanagedCallersOnly]
        static public void Main()
        {
            Nebula.Runtime.Setup(Assembly.GetExecutingAssembly());

            Console.Write("Console.Write works!\n");
            Console.WriteLine("Console.WriteLine works!");
            Nebula.Debug.Log("Nebula.Debug.Log works!\n");

            TestProperty testProp0 = new TestProperty();
            PropertyManager.Instance.RegisterProperty(testProp0);

            Entity entity = new Entity();
            entity.AddProperty(testProp0);

            World world = Nebula.Game.World.Get(World.DEFAULT_WORLD);
            world.RegisterEntity(entity);

            PropertyManager.Instance.PrintAllProperties();

            Tests.Verify(1 == 1);
            Tests.Verify(1 == 0);
        }
    }
}
