﻿/*  
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

using GenTool_CsDataServerDomAsr4.Iface;
using Meta.Helper;
using Meta.Iface;
using System.Data;

namespace ArxmlEditor.Model
{
    public enum ArCommonType
    {
        None = 0,
        MetaObject,
        MetaObjects,
        Enum,
        Enums,
        Other,
        Others,
    };
    public class ArCommon
    {
        public ArCommonType Type { get; }
        public IMetaObjectInstance? Meta { get; }
        public IEnumerable<IMetaObjectInstance>? Metas { get; }
        public Enum? Enum { get; }
        public IMetaCollectionInstance? Enums { get; }
        public object? Obj { get; }
        public IEnumerable<object>? Objs { get; }
        public IMetaRI? Role { get; }
        public ArCommon  Parent { get; }

        public ArCommon(object? obj, IMetaRI? role, ArCommon? parent)
        {
            if (obj == null)
            {
                if (role != null)
                {
                    if (role.IsMeta())
                    {
                        Type = ArCommonType.MetaObject;
                    }
                    else if (role.IsEnum())
                    {
                        Type = ArCommonType.Enum;
                    }
                    else
                    {
                        Type = ArCommonType.Other;
                    }
                }
                else
                {
                    throw new ArgumentException($"ArCommon initialization fail, obj and role are all null");
                }
            }
            else
            {
                if (obj is IMetaObjectInstance meta)
                {
                    if (role == null)
                    {
                        Meta = meta;
                        Type = ArCommonType.MetaObject;
                    }
                    else if (role.IsMeta())
                    {
                        Meta = meta;
                        Type = ArCommonType.MetaObject;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj and role not match");
                    }
                }
                else if (obj is IEnumerable<IMetaObjectInstance> metas)
                {
                    if (role == null)
                    {
                        Metas = metas;
                        Type = ArCommonType.MetaObjects;
                    }
                    else if (role.IsMeta())
                    {
                        Metas = metas;
                        Type = ArCommonType.MetaObjects;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj and role not match");
                    }
                }
                else if (obj is Enum en)
                {
                    if (role == null)
                    {
                        Enum = en;
                        Type = ArCommonType.Enum;
                    }
                    else if (role.IsEnum())
                    {
                        Enum = en;
                        Type = ArCommonType.Enum;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj and role not match");
                    }
                }
                else if (obj.GetType().Name.EndsWith("EnumList"))
                {
                    if (obj is IMetaCollectionInstance ens)
                    {
                        if (role == null)
                        {
                            Enums = ens;
                            Type = ArCommonType.Enums;
                        }
                        else if (role.IsEnum())
                        {
                            Enums = ens;
                            Type = ArCommonType.Enums;
                        }
                        else
                        {
                            throw new ArgumentException($"ArCommon initialization fail, obj and role not match");
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not IMetaCollectionInstance");
                    }
                }
                else if (obj is IEnumerable<object> objs)
                {
                    if (role == null)
                    {
                        Objs = objs;
                        Type = ArCommonType.Others;
                    }
                    else if (role.IsOther())
                    {
                        Objs = objs;
                        Type = ArCommonType.Others;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not IMetaCollectionInstance");
                    }
                }
                else
                {
                    if (role == null)
                    {
                        Obj = obj;
                        Type = ArCommonType.Other;
                    }
                    else if (role.IsOther())
                    {
                        Obj = obj;
                        Type = ArCommonType.Other;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not IMetaCollectionInstance");
                    }
                }
            }

            Role = role;

            if (parent != null)
            {
                Parent = parent;
            }
            else
            {
                Parent = this;
            }
        }

        public IMetaObjectInstance? TryGetMeta()
        {
            if (Type == ArCommonType.MetaObject)
            {
                return Meta;
            }
            return null;
        }

        public IMetaObjectInstance GetMeta()
        {
            if ((Type == ArCommonType.MetaObject) && (Meta != null))
            {
                return Meta;
            }
            throw new Exception("Type is not MetaObject");
        }

        public IEnumerable<IMetaObjectInstance>? TryGetMetas()
        {
            if (Type == ArCommonType.MetaObjects)
            {
                return Metas;
            }
            return null;
        }

        public IEnumerable<IMetaObjectInstance> GetMetas()
        {
            if ((Type == ArCommonType.MetaObjects) && (Metas != null))
            {
                return Metas;
            }
            throw new Exception("Type is not MetaObjects");
        }

        public List<ArCommon> GetCommonMetas()
        {
            var result = new List<ArCommon>();

            if ((Type == ArCommonType.MetaObjects) && (Metas != null))
            {
                foreach (var m in Metas)
                {
                    result.Add(new ArCommon(m, Role, this));
                }
                return result;
            }
            throw new Exception("Type is not MetaObjects");
        }

        public Enum? TryGetEnum()
        {
            if (Type == ArCommonType.Enum)
            {
                return Enum;
            }
            return null;
        }

        public Enum GetEnum()
        {
            if ((Type == ArCommonType.Enum) && (Enum != null))
            {
                return Enum;
            }
            throw new Exception("Type is not Enum");
        }

        public IMetaCollectionInstance? TryGetEnums()
        {
            if (Type == ArCommonType.Enums)
            {
                return Enums;
            }
            return null;
        }

        public IMetaCollectionInstance GetEnums()
        {
            if ((Type == ArCommonType.Enums) && (Enums != null))
            {
                return Enums;
            }
            throw new Exception("Type is not Enums");
        }

        public List<ArCommon> GetCommonEnums()
        {
            var result = new List<ArCommon>();

            if ((Type == ArCommonType.Enums) && (Enums != null))
            {
                foreach (var e in Enums)
                {
                    result.Add(new ArCommon(e, Role, this));
                }
                return result;
            }
            throw new Exception("Type is not Enums");
        }

        public object? TryGetObj()
        {
            if (Type == ArCommonType.Other)
            {
                return Obj;
            }
            return null;
        }

        public object GetObj()
        {
            if ((Type == ArCommonType.Other) && (Obj != null))
            {
                return Obj;
            }
            throw new Exception("Type is not Other");
        }

        public IEnumerable<object>? TryGetObjs()
        {
            if (Type == ArCommonType.Others)
            {
                return Objs;
            }
            return null;
        }

        public IEnumerable<object> GetObjs()
        {
            if ((Type == ArCommonType.Others) && (Objs != null))
            {
                return Objs;
            }
            throw new Exception("Type is not Others");
        }

        public List<ArCommon> GetCommonObjs()
        {
            var result = new List<ArCommon>();

            if ((Type == ArCommonType.Others) && (Objs != null))
            {
                foreach (var o in Objs)
                {
                    result.Add(new ArCommon(o, Role, this));
                }
                return result;
            }
            throw new Exception("Type is not Others");
        }

        public List<ArCommon> GetExistingMember()
        {
            List<ArCommon> result = new();

            if (Type == ArCommonType.MetaObject)
            {
                var arObj = GetMeta();
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
                                result.Add(new ArCommon(arObj.GetValue(o.Name), o, this));
                            }
                        }
                        else
                        {
                            result.Add(new ArCommon(arObj.GetValue(o.Name), o, this));
                        }
                    }
                    else if (o.Multiply())
                    {
                        var childObjs = arObj.GetCollectionValueRaw(o.Name);

                        if (childObjs is IMetaCollectionInstance collection)
                        {
                            if (collection.Count > 0)
                            {
                                result.Add(new ArCommon(collection, o, this));
                            }
                        }
                    }
                }
            }
            return result;
        }

        public List<IMetaRI> GetCandidateMember()
        {
            List<IMetaRI> result = new();

            if (Type == ArCommonType.MetaObject)
            {
                var arObj = GetMeta();
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
            }
            return result;
        }

        public List<ArCommon> GetAllMember()
        {
            List<ArCommon> result = new();

            if (Type == ArCommonType.MetaObject)
            {
                var arObj = GetMeta();
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
                                result.Add(new ArCommon(arObj.GetValue(o.Name), o, this));
                            }
                            else
                            {
                                result.Add(new ArCommon(null, o, this));
                            }
                        }
                        else
                        {
                            result.Add(new ArCommon(arObj.GetValue(o.Name), o, this));
                        }
                    }
                    else if (o.Multiply())
                    {
                        var childObjs = arObj.GetCollectionValueRaw(o.Name);

                        if (childObjs is IMetaCollectionInstance collection)
                        {
                            result.Add(new ArCommon(collection, o, this));
                        }
                    }
                }
            }
            return result;
        }

        public void Check()
        {
            if (Type == ArCommonType.MetaObject)
            {
                var arObj = GetMeta();
                foreach (var o in arObj.MetaAllRoles)
                {
                    if (o.RoleType == RoleTypeEnum.Reference)
                    {
                        continue;
                    }

                    if (o.Single())
                    {
                        if (o.Required())
                        {
                            var v = arObj.GetValue(o.Name);
                            if (v == null)
                            {
                                if (o.InterfaceType.Name == "String")
                                {
                                    arObj.SetValue(o.Name, "");
                                }
                                else
                                {
                                    throw new ArgumentNullException();
                                }
                            }
                        }
                    }
                }
            }
        }

        private object? AddObject(IMetaRI role)
        {
            var arObj = TryGetMeta();

            if (arObj != null)
            {
                var method = arObj.GetType().GetMethod($"Add{role.Name}");
                var newObj = Activator.CreateInstance(role.InterfaceType);
                if ((method != null) && (newObj != null))
                {
                    method.Invoke(arObj, new object[] { newObj });
                    return newObj;
                }
            }
            return null;
        }

        public void RemoveAllObject(IMetaRI role)
        {
            var arObj = TryGetMeta();

            if (arObj != null)
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
                        RemoveObject(role, 0);
                    }
                }
            }
        }

        public void RemoveObject(IMetaRI role, Int32 index)
        {
            var arObj = TryGetMeta();

            if (arObj != null)
            {
                var method = arObj.GetType().GetMethod($"Remove{role.Name}");
                if (method != null)
                {
                    method.Invoke(arObj, new object[] { index });
                }
            }
        }

        public void RemoveObject(IMetaRI role, ArCommon c)
        {
            var arObj = TryGetMeta();

            if (arObj != null)
            {
                var collection = arObj.GetCollectionValueRaw(role.Name);
                if (collection is IMetaCollectionInstance metas2)
                {
                    Int32 index = 0;
                    foreach (var meta in metas2)
                    {
                        if (meta.Equals(c.Obj))
                        {
                            RemoveObject(role, index);
                            return;
                        }
                        index++;
                    }
                    throw new Exception($"No {role.Name} member {c.Obj} in {arObj}");
                }
            }
        }

        public void SetSpecified(IMetaRI role, bool isSpecifed)
        {
            if (Type == ArCommonType.MetaObject)
            {
                var mObj = GetMeta();
                mObj.SetSpecified(role.Name, isSpecifed);
            }
        }

        public void SetObjectSpecified(IMetaRI role, bool isSpecifed)
        {
            var arObj = TryGetMeta();

            if (arObj != null)
            {
                var method = arObj.GetType().GetMethod($"{role.Name}Specified");
                if (method != null)
                {
                    method.Invoke(arObj, new object[] { isSpecifed });
                }
            }
        }

        public bool CanDelete()
        {
            if (Role != null)
            {
                if (Type == ArCommonType.MetaObject)
                {
                    var mObj = GetMeta();

                    if ((uint)Role.MaxOccurs > 1)
                    {
                        var brothers = mObj.Owner.GetCollectionValue(Role.Name);

                        if (brothers.Count() > Role.MinOccurs)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Role.MinOccurs == 0)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if ((uint)Role.MinOccurs == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CanEdit()
        {
            if ((Type == ArCommonType.Enum) || (Type == ArCommonType.Other))
            {
                return true;
            }
            return false;
        }

        public ArCommon? Add(IMetaRI role, Type? type = null)
        {
            if (Type != ArCommonType.MetaObject)
            {
                return null;
            }

            var mObj = GetMeta();

            if(role.Single())
            {
                if (role.Option())
                {
                    mObj.SetSpecified(role.Name, true);
                    var newCommon = new ArCommon(mObj.GetValue(role.Name), role, this);
                    newCommon.Check();
                    return newCommon;
                }
            }
            else if (role.Multiply())
            {
                if (role.MultipleInterfaceTypes == true)
                {
                    var newObj = mObj.AddNew(role.Name, type);
                    var newCommon = new ArCommon(newObj, role, this);
                    newCommon.Check();
                    return newCommon;
                }
                else
                {
                    if (role.IsMeta())
                    {
                        var newObj = mObj.AddNew(role.Name, role.InterfaceType);
                        var newCommon = new ArCommon(newObj, role, this);
                        newCommon.Check();
                        return newCommon;
                    }
                    else
                    {
                        var newObj = AddObject(role);
                        if (newObj != null)
                        {
                            var newCommon = new ArCommon(newObj, role, this);
                            newCommon.Check();
                            return newCommon;
                        }
                    }
                }
            }
            return null;
        }

        public Type[] RoleTypesFor(string roleName)
        {
            if (Type == ArCommonType.MetaObject)
            {
                var meta = GetMeta();
                return meta.RoleTypesFor(roleName);
            }
            return Array.Empty<Type>();
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ArCommonType.None:
                    return "None";

                case ArCommonType.MetaObject:
                    if (Role != null)
                    {
                        if (Meta is IReferrable refMeta)
                        {
                            return refMeta.ShortName;
                        }
                        else
                        {
                            return Role.Name;
                        }
                    }
                    return "";

                case ArCommonType.MetaObjects:
                    if (Role != null)
                    {
                        return $"{Role.Name}(s)";
                    }
                    return "";

                case ArCommonType.Enum:
                    if (Enum != null)
                    {
                        return Enum.ToString();
                    }
                    return "";

                case ArCommonType.Enums:
                    if (Role != null)
                    {
                        return $"{Role.Name}(s)";
                    }
                    return "";

                case ArCommonType.Other:
                    if (Obj != null)
                    {
                        var result = Obj.ToString();
                        if (result != null)
                        {
                            return result;
                        }
                    }
                    return "";

                case ArCommonType.Others:
                    if (Role != null)
                    {
                        return $"{Role.Name}(s)";
                    }
                    return "";

                default:
                    return "";
            }
        }

        public bool IsNull()
        {
            return ((Meta == null) && (Metas == null) && (Enum == null) && (Enums == null) && (Obj == null) && (Objs == null));
        }

        public List<string> EnumCanditate()
        {
            List<string> result = new();
            if (((Type == ArCommonType.Enum) || (Type == ArCommonType.Enums)) && (Role != null))
            {
                foreach (var v in Enum.GetNames(Role.InterfaceType))
                {
                    result.Add(v);
                }
            }
            return result;
        }
    }
}