using System;
using System.Collections.Generic;

namespace ET
{
    public class EntitySystemSingleton: Singleton<EntitySystemSingleton>, ISingletonAwake, ISingletonLoad
    {
        public TypeSystems TypeSystems { get; private set; }
        
        public void Awake()
        {
            this.TypeSystems = new TypeSystems(InstanceQueueIndex.Max);

            foreach (Type type in EventSystem.Instance.GetTypes(typeof (EntitySystemAttribute)))
            {
                object obj = Activator.CreateInstance(type);

                if (obj is ISystemType iSystemType)
                {
                    TypeSystems.OneTypeSystems oneTypeSystems = this.TypeSystems.GetOrCreateOneTypeSystems(iSystemType.Type());
                    oneTypeSystems.Map.Add(iSystemType.SystemType(), obj);
                    int index = iSystemType.GetInstanceQueueIndex();
                    if (index > InstanceQueueIndex.None && index < InstanceQueueIndex.Max)
                    {
                        oneTypeSystems.QueueFlag[index] = true;
                    }
                }
            }
        }
        
        public void Load()
        {
            World.Instance.AddSingleton<EntitySystemSingleton>(true);
        }
        
        public void Serialize(Entity component)
        {
            if (component is not ISerialize)
            {
                return;
            }
            
            List<object> iSerializeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (ISerializeSystem));
            if (iSerializeSystems == null)
            {
                return;
            }

            foreach (ISerializeSystem serializeSystem in iSerializeSystems)
            {
                if (serializeSystem == null)
                {
                    continue;
                }

                try
                {
                    serializeSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        public void Deserialize(Entity component)
        {
            if (component is not IDeserialize)
            {
                return;
            }
            
            List<object> iDeserializeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IDeserializeSystem));
            if (iDeserializeSystems == null)
            {
                return;
            }

            foreach (IDeserializeSystem deserializeSystem in iDeserializeSystems)
            {
                if (deserializeSystem == null)
                {
                    continue;
                }

                try
                {
                    deserializeSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        // GetComponentSystem
        public void GetComponent(Entity entity, Entity component)
        {
            List<object> iGetSystem = this.TypeSystems.GetSystems(entity.GetType(), typeof (IGetComponentSystem));
            if (iGetSystem == null)
            {
                return;
            }

            foreach (IGetComponentSystem getSystem in iGetSystem)
            {
                if (getSystem == null)
                {
                    continue;
                }

                try
                {
                    getSystem.Run(entity, component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
        
        // AddComponentSystem
        public void AddComponent(Entity entity, Entity component)
        {
            List<object> iAddSystem = this.TypeSystems.GetSystems(entity.GetType(), typeof (IAddComponentSystem));
            if (iAddSystem == null)
            {
                return;
            }

            foreach (IAddComponentSystem addComponentSystem in iAddSystem)
            {
                if (addComponentSystem == null)
                {
                    continue;
                }

                try
                {
                    addComponentSystem.Run(entity, component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake(Entity component)
        {
            List<object> iAwakeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IAwakeSystem));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1>(Entity component, P1 p1)
        {
            if (component is not IAwake<P1>)
            {
                return;
            }
            
            List<object> iAwakeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IAwakeSystem<P1>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1, P2>(Entity component, P1 p1, P2 p2)
        {
            if (component is not IAwake<P1, P2>)
            {
                return;
            }
            
            List<object> iAwakeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IAwakeSystem<P1, P2>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1, P2> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1, p2);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Awake<P1, P2, P3>(Entity component, P1 p1, P2 p2, P3 p3)
        {
            if (component is not IAwake<P1, P2, P3>)
            {
                return;
            }
            
            List<object> iAwakeSystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IAwakeSystem<P1, P2, P3>));
            if (iAwakeSystems == null)
            {
                return;
            }

            foreach (IAwakeSystem<P1, P2, P3> aAwakeSystem in iAwakeSystems)
            {
                if (aAwakeSystem == null)
                {
                    continue;
                }

                try
                {
                    aAwakeSystem.Run(component, p1, p2, p3);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        public void Destroy(Entity component)
        {
            if (component is not IDestroy)
            {
                return;
            }
            
            List<object> iDestroySystems = this.TypeSystems.GetSystems(component.GetType(), typeof (IDestroySystem));
            if (iDestroySystems == null)
            {
                return;
            }

            foreach (IDestroySystem iDestroySystem in iDestroySystems)
            {
                if (iDestroySystem == null)
                {
                    continue;
                }

                try
                {
                    iDestroySystem.Run(component);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }
    }
}