using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBox.UI.Widgets
{
    public class WidgetApplication
    {
        private bool stopped;

        private bool forceRender;

        private ManualResetEventSlim reset = new ManualResetEventSlim(false);

        private Func<IWidget> factory;

        public BuildContext Context { get; set; }

        public UserInputManager InputManager { get; set; }

        public IConsoleBox Console { get; set; }

        public UIManager UI { get; set; }

        public WidgetApplication(IConsoleBox console, Func<IWidget> viewFactory)
        {
            Console = console;
            Context = new BuildContext();
            factory = viewFactory;
            InputManager = new UserInputManager();

            UI = new UIManager(console);

            // note:  Order on wiring up events matters!
            // we want the UI to process the events prior to 
            // firing off renders
            console.KeyEvent += (s, e) =>
            {
                InputManager.OnKey(e);
                Render();
            };

            console.MouseMove += (s, e) => Render();
            console.MouseButtonUp += (s, e) => Render();
            console.MouseClick += (s, e) => Render();
            console.MouseWheel += (s, e) => Render();
            console.MouseButtonDown += (s, e) => Render();

            console.ResizeEvent += (s, e) =>
            {
                console.Clear();
                Render(true);
            };

            Context.State.Set(UserInputManager.STATE_KEY, InputManager);
        }

        private void Render(bool force = false)
        {
            //if (force)
            //{
            //    forceRender = true;
            //}
            //reset.Set();
            RenderSync(force);
        }

        public void Stop()
        {
            stopped = true;
        }

        private void RenderLoop(CancellationToken cancel)
        {
            while (!stopped && !cancel.IsCancellationRequested)
            {
                if (forceRender || Context.State.IsChanged)
                {
                    try
                    {
                        var root = factory();
                        UI.Render(root.Build(Context));

                        forceRender = false;
                        Context.State.AcceptChanges();

                        // TODO.. should this be a widget or no?
                        // it has to wire up to the console to 'work' correctly
                        // lots of options needed to make this work sanely...
                        Context.State.Set(UserInputManager.STATE_KEY, InputManager);
                    }
                    catch (Exception ex)
                    {
                        var bob = ex;
                    }
                }

                reset.Reset();
                reset.Wait();
            }
        }

        public void RenderSync(bool force = false)
        {
            if (force || Context.State.IsChanged)
            {
                try
                {
                    var root = factory();
                    UI.Render(root.Build(Context));

                    Context.State.AcceptChanges();

                    // TODO.. should this be a widget or no?
                    // it has to wire up to the console to 'work' correctly
                    // lots of options needed to make this work sanely...
                    // the console could be injected into the build context as an alternative
                    // could also create an 'app widget' you implement to start things up
                    Context.State.Set(UserInputManager.STATE_KEY, InputManager);

                }
                catch (Exception ex)
                {
                    var bob = ex;
                    throw;
                }
            }
        }

        public void StartSync(CancellationToken cancel = default(CancellationToken))
        {
            RenderSync(true);
            Console.PollEvents(cancel);
        }

        public Task Start(CancellationToken cancel = default(CancellationToken))
        {
            forceRender = true;
            return Task.WhenAll(
                Task.Run(() => RenderLoop(cancel)),
                Task.Run(() => Console.PollEvents(cancel))
                );
        }
    }
}
