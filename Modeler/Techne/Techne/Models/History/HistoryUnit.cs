/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Reflection;
using System.Windows.Media.Media3D;
using Cinch;
using Techne.Plugins.Interfaces;

namespace Techne
{
    public class HistoryUnit
    {
        private HistoryAction action;
        private DispatcherNotifiedObservableCollection<Visual3D> collection;
        private bool isCollection;
        private object newValue;
        private object oldValue;

        private PropertyInfo property;
        private ITechneVisual visualHit;

        public HistoryUnit(object oldValue, object newValue, PropertyInfo property, HistoryAction action)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.property = property;
            this.action = action;
        }

        public HistoryUnit(object oldValue, object newValue, DispatcherNotifiedObservableCollection<Visual3D> collection, HistoryAction action)
        {
            isCollection = true;
            this.collection = collection;
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.action = action;
        }

        public HistoryUnit(ITechneVisual visualHit, HistoryAction historyAction)
        {
            // TODO: Complete member initialization
            this.visualHit = visualHit;
            action = historyAction;
        }

        public object OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }

        public object NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        public ITechneVisual VisualHit
        {
            get { return visualHit; }
            set { visualHit = value; }
        }

        internal HistoryAction Action
        {
            get { return action; }
            set { action = value; }
        }

        public bool IsCollection
        {
            get { return isCollection; }
            set { isCollection = value; }
        }

        public PropertyInfo Property
        {
            get { return property; }
            set { property = value; }
        }
    }
}

