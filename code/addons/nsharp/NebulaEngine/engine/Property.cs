using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using Mathf;
using Nebula;
using Nebula.Game;

namespace Nebula
{
    namespace Game
    {
        public enum Events
        {
            OnActivate,
            OnDeactivate,
            
            // Update order
            OnBeginFrame, // Called every frame before rendering (old vis results)
            OnFixedFrame, // Called every couple of frames with a fixed delta frame time. These updates are distributed and might not be called on the same frame for all components (called after OnBeginFrame)
            OnFrame, // Called every frame while rendering (up to date vis results)
            OnEndFrame, // Called after rendering is done

            // Total number of events
            NumEvents
        }

        public class Property
        {
            private Entity entity;
            private bool attachedToEntity;

            public Entity Entity
            {
                get
                {
                    return entity;
                }
                set
                {
                    if (!attachedToEntity)
                    {
                        entity = value;
                        attachedToEntity = true;
                    }
                    else
                    {
                        Debug.Log("Warning: Trying to change owner entity of a property that is already owned!");
                    }
                }
            }

            public bool active;

            public virtual Nebula.Game.Events[] AcceptedEvents()
            {
                // Override in subclass to tell the property manager which functions to call for this class
                return null;
            }

            public virtual System.Type[] AcceptedMessages()
            {
                // Override in subclass
                return null;
            }

            public virtual void OnMessage(in Msg msg)
            {
                // Override in subclass
            }

            public override string ToString()
            {
                return GetType().ToString() + " -> Entity: " + entity.ToString();
            }

            public virtual void OnActivate() { }
            public virtual void OnDeactivate() { }
            public virtual void OnBeginFrame() { }
            public virtual void OnFrame() { }
            public virtual void OnEndFrame() { }
        }

        // ------ Example ---------
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

    }
}
