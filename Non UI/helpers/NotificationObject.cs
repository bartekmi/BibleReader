using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BibleReader.helpers {
    public abstract class NotificationObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected NotificationObject() {
            PropertyChanged = delegate { };
        }

        public void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression) {
            DoNotUseUnlessNecessary_RaisePropertyChanged(GetPropertyName(propertyExpression));
        }

        public static bool IsProperty<T>(string propertyName, Expression<Func<T>> propertyExpression) {
            return GetPropertyName(propertyExpression) == propertyName;
        }

        public static string GetPropertyName(LambdaExpression propertyExpression) {

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("'propertyExpression' should be a member expression");

            return body.Member.Name;
        }

        public static string GetPropertyNameTyped<T>(Expression<Func<T>> propertyExpression) {

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("'propertyExpression' should be a member expression");

            return body.Member.Name;
        }

        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression) {
            PropertyInfo info = typeof(T).GetProperty(GetPropertyName(propertyExpression));
            return info;
        }

        public virtual void DoNotUseUnlessNecessary_RaisePropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
