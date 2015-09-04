using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

//Event Design: http://msdn.microsoft.com/en-us/library/ms229011.aspx

namespace BugTracker.MVVM
{
    /// <summary>
    /// Updates the GUI when a property changes in the viewmodel
    /// </summary>
    [Serializable]
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        /// <summary>
        /// Event called when property updates
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Updates the handler when property updates
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Updates the handler when property updates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpresssion"></param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
            this.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Updates the handler when property updates
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(String propertyName)
        {
            VerifyPropertyName(propertyName);
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Warns the developer if this Object does not have a public property with
        /// the specified name. This method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(String propertyName)
        {
            // verify that the property name matches a real,  
            // public, instance property on this Object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.Fail("Invalid property name: " + propertyName);
            }
        }

        /// <summary>
        /// Property Support Class
        /// </summary>
        private static class PropertySupport
        {
            /// <summary>
            /// Used to extract the property name from the expression
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="propertyExpresssion"></param>
            /// <returns></returns>
            public static String ExtractPropertyName<T>(Expression<Func<T>> propertyExpresssion)
            {
                if (propertyExpresssion == null)
                {
                    throw new ArgumentNullException("propertyExpresssion");
                }

                var memberExpression = propertyExpresssion.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentException("The expression is not a member access expression.", "propertyExpresssion");
                }

                var property = memberExpression.Member as PropertyInfo;
                if (property == null)
                {
                    throw new ArgumentException("The member access expression does not access a property.", "propertyExpresssion");
                }

                var getMethod = property.GetGetMethod(true);
                if (getMethod.IsStatic)
                {
                    throw new ArgumentException("The referenced property is a static property.", "propertyExpresssion");
                }

                return memberExpression.Member.Name;
            }
        }
    }
}
