/*  
 *  This file is a part of Arxml Editor.
 *  
 *  Copyright (C) 2021-2023 DJS Studio E-mail: ddsilence@sina.cn
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using Meta.Helper;
using Meta.Iface;

namespace ArxmlEditor.Model
{
    public static class IMetaObjectInstanceExtend
    {
        public static Dictionary<IMetaRI, object> GetExistingMember(this IMetaObjectInstance arObj)
        {
            Dictionary<IMetaRI, object> result = new();

            foreach (var o in arObj.MetaAllRoles)
            {
                if (o.RoleType == RoleTypeEnum.Reference)
                {
                    continue;
                }

                if (o.Single())
                {
                    if (o.Option())
                    {
                        if (arObj.IsSpecified(o.Name))
                        {
                            result.Add(o, arObj.GetValue(o.Name));
                        }
                    }
                    else
                    {
                        result.Add(o, arObj.GetValue(o.Name));
                    }
                }
                else if (o.Multiply())
                {
                    var childObjs = arObj.GetCollectionValueRaw(o.Name);
                    if (childObjs is IMetaCollectionInstance metaObjs)
                    {
                        if (metaObjs.Count > 0)
                        {
                            result.Add(o, childObjs);
                        }
                    }
                }
            }
            return result;
        }

        public static List<IMetaRI> GetCandidateMember(this IMetaObjectInstance arObj)
        {
            List<IMetaRI> result = new();

            foreach (var o in arObj.MetaAllRoles)
            {
                if (o.RoleType == RoleTypeEnum.Reference)
                {
                    continue;
                }

                if (o.Single())
                {
                    if (o.Option())
                    {
                        if (!arObj.IsSpecified(o.Name))
                        {
                            result.Add(o);
                        }
                    }
                }
                else if (o.Multiply())
                {
                    var childObjs = arObj.GetCollectionValueRaw(o.Name);
                    if (childObjs is IMetaCollectionInstance metaObjs)
                    {
                        if (metaObjs.Count < (uint)o.MaxOccurs)
                        {
                            result.Add(o);
                        }
                    }
                }
            }
            return result;
        }

        public static object? AddObject(this IMetaObjectInstance arObj, IMetaRI role)
        {
            var method = arObj.GetType().GetMethod($"Add{role.Name}");
            var newObj = Activator.CreateInstance(role.InterfaceType);
            if ((method != null) && (newObj != null))
            {
                method.Invoke(arObj, new object[] { newObj });
                return newObj;
            }
            return null;
        }
        public static void RemoveAllObject(this IMetaObjectInstance arObj, IMetaRI role)
        {
            var collection = arObj.GetCollectionValueRaw(role.Name);
            if (collection is IEnumerable<IMetaObjectInstance> metas)
            {
                foreach (var meta in metas.ToList())
                {
                    meta.DeleteAndRemoveFromOwner();
                }
            }
            else if (collection is IMetaCollectionInstance c)
            {
                var count = c.Count;
                for (Int32 i = 0; i < count; i++)
                {
                    arObj.RemoveObject(role, 0);
                }
            }
        }


        public static void RemoveObject(this IMetaObjectInstance arObj, IMetaRI role, Int32 index)
        {
            var method = arObj.GetType().GetMethod($"Remove{role.Name}");
            if (method != null)
            {
                method.Invoke(arObj, new object[] { index });
            }
        }

        public static void RemoveObject(this IMetaObjectInstance arObj, IMetaRI role, object obj)
        {
            var collection = arObj.GetCollectionValueRaw(role.Name);
            if (collection is IMetaCollectionInstance metas2)
            {
                Int32 index = 0;
                foreach (var meta in metas2)
                {
                    if (meta.Equals(obj))
                    {
                        arObj.RemoveObject(role, index);
                        return;
                    }
                    index++;
                }
                throw new Exception($"No {role.Name} member {obj} in {arObj}");
            }
        }

        public static void SetObjectSpecified(this IMetaObjectInstance arObj, IMetaRI role, bool isSpecifed)
        {
            var method = arObj.GetType().GetMethod($"{role.Name}Specified");
            if (method != null)
            {
                method.Invoke(arObj, new object[] { isSpecifed });
            }
        }
    }
}
