using System;
using System.Collections.Generic;


namespace DotNetDesignPatternDemos.Architectural.EntityComponentSystem.Example
{

    /*
     *  Un esempio semplificato di entity-component system.
     *  
     *  Fornisce solo le basi, non ha bisogno di essere molto efficiente perché è
     *  Più prezioso per i giochi più piccoli senza una tonnellata di entità
     *  
     *  ma rende l'idea dell'architettura logica di questo modello di prgoettazione.
     * 
     */

    /// <summary>
    /// Actor is a simple game object type with position and collision bounds
    /// </summary>
    public class Actor
        {
            public IDictionary<byte, Component> Components = new Dictionary<byte, Component>();

            public Actor()
            {

            }

            /// <summary>
            /// Safely gets the component of type attached to the Actor.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns>
            ///     Component of type T if component has been attached to the Actor
            ///     else returns null
            /// </returns>
            public T Component<T>() where T : ECS.Component
            {
                byte key = ECS.ComponentMapper.Get<T>();
                ECS.Component comp = null;
                if (Components.TryGetValue(key, out comp))
                {
                    return (T)comp;
                }
                return default(T);
            }

            /// <summary>
            /// Attaches a component onto the actor
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="component"></param>
            public void AddComponent<T>(T component) where T : Component
            {
                byte key = ECS.ComponentMapper.Get<T>();
                this.Components[key] = component;
            }

            /// <summary>
            /// Removes a component from an actor
            /// </summary>
            /// <typeparam name="T"></typeparam>
            public void RemoveComponent<T>() where T : Component
            {
                byte key = ECS.ComponentMapper.Get<T>();
                this.Components.Remove(key);
            }
        }

        /// <summary>
        /// Maps Components to byte based ids for faster retrieval and to prevent collision
        /// </summary>
        public class ComponentMapper
        {
            private static readonly ComponentMapper _instance = new ComponentMapper();

            private IDictionary<Type, byte> Map = new Dictionary<Type, byte>();
            private byte Index = 0x00;

            public static byte Register<T>()
            {
                Type key = typeof(T);
                _instance.Map[key] = _instance.Index;
                Console.Out.WriteLine("Registered " + key.Name + " to id: " + _instance.Index);
                _instance.Index++;
                return _instance.Index;
            }

            public static byte Get<T>() where T : Component
            {
                byte id;
                _instance.Map.TryGetValue(typeof(T), out id);
                return id;
            }
        }

        public interface Component
        {

        }

        public class Family
        {
            List<byte> _query;

            public Family()
            {
                this._query = new List<byte>();
            }

            public void Add<T>() where T : Component
            {
                this._query.Add(ComponentMapper.Get<T>());
            }

            public bool Matches(Actor a)
            {
                int matched = 0;
                foreach (byte q in _query)
                {
                    matched += (a.Components.ContainsKey(q)) ? 1 : 0;
                    if (matched == _query.Count)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public class World
        {
            private List<Actor> _entities = new List<Actor>();
            private List<EntitySystem> _systems = new List<EntitySystem>();
            private List<IEntityListener> _listeners = new List<IEntityListener>();

            public void Update(GameTime timer)
            {
                foreach (EntitySystem sys in this._systems)
                {
                    if (!sys.OnDemand())
                    {
                        sys.Update(timer);
                    }
                }
            }

            public void AddEntity(Actor a)
            {
                this._entities.Add(a);
                foreach (IEntityListener el in this._listeners)
                {
                    el.EntityAddedToWorld(a);
                }
                foreach (IEntityListener el in this._systems)
                {
                    el.EntityAddedToWorld(a);
                }
            }

            public void RemoveEntity(Actor a)
            {
                bool removed = this._entities.Remove(a);
                if (removed)
                {
                    foreach (IEntityListener el in this._listeners)
                    {
                        el.EntityRemovedFromWorld(a);
                    }
                    foreach (IEntityListener el in this._systems)
                    {
                        el.EntityRemovedFromWorld(a);
                    }
                }
            }

            public void AddSystem(EntitySystem sys)
            {
                this._systems.Add(sys);
                sys.AddedToWorld(this);
            }

            public void RemoveSystem(EntitySystem sys)
            {
                bool removed = this._systems.Remove(sys);
                if (removed)
                {
                    sys.RemovedFromWorld(this);
                }
            }

            public void AddListener(IEntityListener l)
            {
                this._listeners.Add(l);
            }

            public void RemoveListener(IEntityListener l)
            {
                this._listeners.Remove(l);
            }
        }

        public abstract class EntitySystem : IEntityListener
        {
            protected World World;

            /// <summary>
            /// Triggers the system's
            /// </summary>
            abstract public void Update(GameTime timer);

            /// <summary>
            /// Determines if update should be called once per cycle
            /// </summary>
            /// <returns></returns>
            abstract public bool OnDemand();

            public abstract void AddedToWorld(World w);
            public abstract void RemovedFromWorld(World w);

            public abstract void EntityAddedToWorld(Actor a);
            public abstract void EntityRemovedFromWorld(Actor a);
        }

        public interface IEntityListener
        {
            void EntityAddedToWorld(Actor a);
            void EntityRemovedFromWorld(Actor a);
        }
    }

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}