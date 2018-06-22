using System.Linq;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace ConsoleBox.UI
{
    public class StateManager : IChangeTracking
    {
        private class StateContainer
        {
            public object State { get; set; }

            public bool IsLoaded { get; set; }

            public Action<StateManager, object> TearDown { get; set; }
        }

        private Dictionary<string, StateContainer> states = new Dictionary<string, StateContainer>();

        private bool isChanged = false;

        public bool IsChanged
        {
            get
            {
                return isChanged || states.Values
                   .Where(s => s.State is IChangeTracking)
                   .Any(s => ((IChangeTracking)s.State).IsChanged);
            }
        }

        public void FlagChanged()
        {
            isChanged = true;
        }

        public void AcceptChanges()
        {
            foreach (var k in states.Keys.ToList())
            {
                var container = states[k];
                if (!container.IsLoaded)
                {
                    container.TearDown?.Invoke(this, container.State);
                    if (container.State is IDisposable)
                    {
                        ((IDisposable)container.State).Dispose();
                    }
                    states.Remove(k);
                }
                else
                {
                    // reset the status
                    container.IsLoaded = false;
                    if (container.State is IChangeTracking)
                    {
                        ((IChangeTracking)container.State).AcceptChanges();
                    }
                }
            }

            isChanged = false;
        }

        public bool TryGet<T>(string key, out T value, bool causesLoad = true)
        {
            if (states.ContainsKey(key))
            {
                var state = states[key];
                state.IsLoaded = causesLoad;

                value = (T)state.State;
                return true;
            }

            value = default(T);
            return false;
        }

        public void Remove(string key)
        {
            if (states.ContainsKey(key))
            {
                states.Remove(key);
            }
        }

        public void Set<T>(string key, T value)
        {
            Set(key, value, null);
        }

        public void Set<T>(string key, T value, Action<StateManager, T> tearDown)
        {
            StateContainer state;

            if (states.ContainsKey(key))
            {
                state = states[key];
            }
            else
            {
                state = new StateContainer();
                states.Add(key, state);
            }

            state.State = value;
            if (tearDown != null)
            {
                state.TearDown = (man, s) => tearDown?.Invoke(man, (T)s); // TODO optimization
            }
            state.IsLoaded = true;
        }
    }
}
