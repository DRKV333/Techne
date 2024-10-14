using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace MEFedMVVM.Common
{
    public static class WeakEventHandlerManager
    {
        ///<summary>
        /// Invokes the handlers 
        ///</summary>
        ///<param name="sender"></param>
        ///<param name="handlers"></param>
        public static void CallWeakReferenceHandlers(object sender, List<WeakReference> handlers)
        {
            if (handlers != null)
            {
                // Take a snapshot of the handlers before we call out to them since the handlers
                // could cause the array to me modified while we are reading it.
                var callees = new EventHandler[handlers.Count];
                int count = 0;

                //Clean up handlers
                count = CleanupOldHandlers(handlers, callees, count);

                // Call the handlers that we snapshotted
                for (int i = 0; i < count; i++)
                {
                    CallHandler(sender, callees[i]);
                }
            }
        }

        private static void CallHandler(object sender, EventHandler eventHandler)
        {
            DispatcherProxy dispatcher = DispatcherProxy.CreateDispatcher();

            if (eventHandler != null)
            {
                if (dispatcher != null && !dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke((Action<object, EventHandler>) CallHandler, sender, eventHandler);
                }
                else
                {
                    eventHandler(sender, EventArgs.Empty);
                }
            }
        }

        private static int CleanupOldHandlers(List<WeakReference> handlers, EventHandler[] callees, int count)
        {
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                WeakReference reference = handlers[i];
                var handler = reference.Target as EventHandler;
                if (handler == null)
                {
                    // Clean up old handlers that have been collected
                    handlers.RemoveAt(i);
                }
                else
                {
                    callees[count] = handler;
                    count++;
                }
            }
            return count;
        }

        ///<summary>
        /// Adds a handler to the supplied list in a weak way.
        ///</summary>
        ///<param name="handlers">Existing handler list.  It will be created if null.</param>
        ///<param name="handler">Handler to add.</param>
        ///<param name="defaultListSize">Default list size.</param>
        public static void AddWeakReferenceHandler(ref List<WeakReference> handlers, EventHandler handler,
                                                   int defaultListSize)
        {
            if (handlers == null)
            {
                handlers = (defaultListSize > 0 ? new List<WeakReference>(defaultListSize) : new List<WeakReference>());
            }

            handlers.Add(new WeakReference(handler));
        }

        ///<summary>
        /// Removes an event handler from the reference list.
        ///</summary>
        ///<param name="handlers">Handler list to remove reference from.</param>
        ///<param name="handler">Handler to remove.</param>
        public static void RemoveWeakReferenceHandler(List<WeakReference> handlers, EventHandler handler)
        {
            if (handlers != null)
            {
                for (int i = handlers.Count - 1; i >= 0; i--)
                {
                    WeakReference reference = handlers[i];
                    var existingHandler = reference.Target as EventHandler;
                    if ((existingHandler == null) || (existingHandler == handler))
                    {
                        // Clean up old handlers that have been collected
                        // in addition to the handler that is to be removed.
                        handlers.RemoveAt(i);
                    }
                }
            }
        }

        #region Nested type: DispatcherProxy

        /// <summary>
        /// Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier
        /// </summary>
        private class DispatcherProxy
        {
            private readonly Dispatcher _innerDispatcher;

            private DispatcherProxy(Dispatcher dispatcher)
            {
                _innerDispatcher = dispatcher;
            }

            public static DispatcherProxy CreateDispatcher()
            {
                DispatcherProxy proxy = null;
#if SILVERLIGHT
                if (Deployment.Current == null)
                    return null;

                proxy = new DispatcherProxy(Deployment.Current.Dispatcher);
#else
                if (Application.Current == null)
                    return null;

                proxy = new DispatcherProxy(Application.Current.Dispatcher);
#endif
                return proxy;
            }

            public bool CheckAccess()
            {
                return _innerDispatcher.CheckAccess();
            }

            // TODO: Port of VS 2010 (AdeM) 
            // Suppress error. Investigate if this is the right fix. May be moot if we target 4.0 by default or were OK
            // with this in Prism 2.0
            //
            // Error	39	CA1903 : Microsoft.Portability : 
            // Member 'WeakEventHandlerManager.DispatcherProxy.BeginInvoke(Delegate, params object[])' uses member 
            // 'Dispatcher.BeginInvoke(Delegate, DispatcherPriority, params object[])'. Because this member was introduced 
            // in .NET Framework 3.0 Service Pack 2, which was not included in the project's target framework, .NET 
            // Framework 3.5, your application may fail to run on systems without this service pack installed.	

            [SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework",
                MessageId =
                    "System.Windows.Threading.Dispatcher.#BeginInvoke(System.Delegate,System.Windows.Threading.DispatcherPriority,System.Object[])"
                )]
            public DispatcherOperation BeginInvoke(Delegate method, params Object[] args)
            {
#if SILVERLIGHT
                return _innerDispatcher.BeginInvoke(method, args);
#else
                return _innerDispatcher.BeginInvoke(method, DispatcherPriority.Normal, args);
#endif
            }
        }

        #endregion
    }
}
