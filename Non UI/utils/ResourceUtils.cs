using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BibleReader.utils {
    public static class ResourceUtils {
        public static byte[] ReadEmbeddedResource(string name, Assembly assembly = null) {
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(name)) {
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

    }
}
