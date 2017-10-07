using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.utils {
    public static class ConfigUtils {
        public static T GetOptional<T>(string name, T defaultValue) {
            string value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;
            return Parse<T>(value);
        }

        public static T GetMandatory<T>(string name) {
            string value = ConfigurationManager.AppSettings[name];
            if (string.IsNullOrWhiteSpace(value))
                throw new Exception("Missing mandatory parameter: " + name);
            return Parse<T>(value);
        }

        // name must point to config parameter which, if it exsists, must be the class name of a class that 'is' T
        public static T GetOptionalObject<T>(string name, T defaultObject) {
            string className = ConfigurationManager.AppSettings[name];
            if (className == null)
                return defaultObject;

            Type type = Type.GetType(className, true /* throw exception on error */);

            T theObject = (T)Activator.CreateInstance(type);
            return theObject;
        }

        private static T Parse<T>(string value) {
            Type type = typeof(T);
            if (type.IsEnum)
                return (T)Enum.Parse(type, value);
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
