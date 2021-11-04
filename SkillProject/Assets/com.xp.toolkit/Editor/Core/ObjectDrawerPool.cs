using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XPToolchains.Core
{
    public class ObjectDrawerPool
    {

        private static Dictionary<Type, Type> objectDrawerTypeMap = new Dictionary<Type, Type>();

        private static Dictionary<int, ObjectDrawer> objectDrawerMap = new Dictionary<int, ObjectDrawer>();

        private static bool mapBuilt = false;

        private static void BuildObjectDrawers()
        {
            if (ObjectDrawerPool.mapBuilt)
            {
                return;
            }
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly != null)
                {
                    try
                    {
                        foreach (Type type in assembly.GetExportedTypes())
                        {
                            CustomFieldDrawerAttribute[] array;
                            if (typeof(ObjectDrawer).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract && (array = (type.GetCustomAttributes(typeof(CustomFieldDrawerAttribute), false) as CustomFieldDrawerAttribute[])).Length > 0)
                            {
                                ObjectDrawerPool.objectDrawerTypeMap.Add(array[0].Type, type);
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            ObjectDrawerPool.mapBuilt = true;
        }

        private static bool ObjectDrawerForType(Type _fieldType, ref ObjectDrawer _fieldDrawer, ref Type _fieldDrawerType, int _hash)
        {
            ObjectDrawerPool.BuildObjectDrawers();
            if (!ObjectDrawerPool.objectDrawerTypeMap.ContainsKey(_fieldType))
            {
                return false;
            }
            _fieldDrawerType = ObjectDrawerPool.objectDrawerTypeMap[_fieldType];
            if (ObjectDrawerPool.objectDrawerMap.ContainsKey(_hash))
            {
                _fieldDrawer = ObjectDrawerPool.objectDrawerMap[_hash];
            }
            return true;
        }

        public static ObjectDrawer GetObjectDrawer(FieldInfo _fieldInfo)
        {
            ObjectDrawer objectDrawer = null;
            Type type = null;
            if (!ObjectDrawerPool.ObjectDrawerForType(_fieldInfo.FieldType, ref objectDrawer, ref type, _fieldInfo.GetHashCode()))
                return null;
            if (objectDrawer == null)
            {
                objectDrawer = (Activator.CreateInstance(type) as ObjectDrawer);
                ObjectDrawerPool.objectDrawerMap.Add(_fieldInfo.GetHashCode(), objectDrawer);
            }
            objectDrawer.FieldInfo = _fieldInfo;
            return objectDrawer;
        }

        public static ObjectDrawer GetObjectDrawer(FieldAttribute attribute)
        {
            ObjectDrawer objectDrawer = null;
            Type type = null;
            if (!ObjectDrawerPool.ObjectDrawerForType(attribute.GetType(), ref objectDrawer, ref type, attribute.GetHashCode()))
                return null;
            if (objectDrawer != null)
                return objectDrawer;
            objectDrawer = (Activator.CreateInstance(type) as ObjectDrawer);
            objectDrawer.Attribute = attribute;
            ObjectDrawerPool.objectDrawerMap.Add(attribute.GetHashCode(), objectDrawer);
            return objectDrawer;
        }
    }
}
