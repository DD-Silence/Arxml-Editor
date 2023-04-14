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

using GenTool_CsDataServerDomAsr4.Iface;
using Meta.Helper;
using Meta.Iface;
using System.Data;
using System.Reflection;

namespace ArxmlEditor.Model
{
    public enum ArCommonType
    {
        None = 0,
        Reference,
        References,
        Meta,
        Metas,
        Enum,
        Enums,
        Bool,
        Integer,
        Other,
        Others,
    };

    public delegate void ChangedEventHandler();

    public class ArCommon
    {
        public ArCommonType Type { get; }
        private IARRef? Reference { get; }
        private IEnumerable<IARRef>? References { get; }
        private IMetaObjectInstance? Meta { get; }
        private IEnumerable<IMetaObjectInstance>? Metas { get; }
        private Enum? Enum { get; set; }
        private IMetaCollectionInstance? Enums { get; }
        private bool? Bool { get; set; }
        private AsrInt? Integer { get; }
        private object? Obj { get; }
        private IEnumerable<object>? Objs { get; }
        public IMetaRI? Role { get; }
        public ArCommon  Parent { get; }
        public event ChangedEventHandler? Changed;
        public static readonly Dictionary<string, Dictionary<string, string[]>> arFilter = new()
        //{
        //    {
        //        "ArPackage", new()
        //        {
        //            {"Include", new string[] { "ArPackage", "Element" } }
        //        }
        //    },
        //    {
        //        "Element", new()
        //        {
        //            {"Include", new string[] { "EcucModuleDef" } }
        //        }
        //    }
        //}
        ;


        public ArCommon(object? obj, IMetaRI? role, ArCommon? parent)
        {
            if (obj == null)
            {
                if (role != null)
                {
                    if (role.IsRefernce())
                    {
                        Type = ArCommonType.Reference;
                    }
                    else if (role.IsMeta())
                    {
                        Type = ArCommonType.Meta;
                    }
                    else if (role.IsEnum())
                    {
                        Type = ArCommonType.Enum;
                    }
                    else if (role.IsBool())
                    {
                        Type = ArCommonType.Bool;
                    }
                    else if (role.IsInteger())
                    {
                        Type = ArCommonType.Integer;
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
                if (obj is IARRef r)
                {
                    if (role == null)
                    {
                        Reference = r;
                        Type = ArCommonType.Reference;
                    }
                    else if (role.IsRefernce())
                    {
                        Reference = r;
                        Type = ArCommonType.Reference;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not IARRef");
                    }
                }
                else if (obj is IEnumerable<IARRef> rs)
                {
                    if (role == null)
                    {
                        References = rs;
                        Type = ArCommonType.References;
                    }
                    else if (role.IsRefernce())
                    {
                        References = rs;
                        Type = ArCommonType.References;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj and role not match");
                    }
                }
                else if (obj is IMetaObjectInstance meta)
                {
                    if (role == null)
                    {
                        Meta = meta;
                        Type = ArCommonType.Meta;
                    }
                    else if (role.IsMeta())
                    {
                        Meta = meta;
                        Type = ArCommonType.Meta;
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
                        Type = ArCommonType.Metas;
                    }
                    else if (role.IsMeta())
                    {
                        Metas = metas;
                        Type = ArCommonType.Metas;
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
                else if (obj is bool b)
                {
                    if (role == null)
                    {
                        Bool = b;
                        Type = ArCommonType.Bool;
                    }
                    else if (role.IsBool())
                    {
                        Bool = b;
                        Type = ArCommonType.Bool ;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not IMetaCollectionInstance");
                    }
                }
                else if (obj is AsrInt v)
                {
                    if (role == null)
                    {
                        Integer = v;
                        Type = ArCommonType.Integer;
                    }
                    else if (role.IsInteger())
                    {
                        Integer = v;
                        Type = ArCommonType.Integer;
                    }
                    else
                    {
                        throw new ArgumentException($"ArCommon initialization fail, obj is not AsrInt");
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

        public IMetaObjectRoot Root()
        {
            IMetaObjectRoot result;

            if ((Type == ArCommonType.Reference) && (Reference != null))
            {
                result = Reference.Root;
            }
            else if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                result = Meta.Root;
            }
            else
            {
                result = Parent.Root();
            }
            return result;
        }

        public IEnumerable<IMetaObjectInstance> AllObjects()
        {
            return Root().AllObjects;
        }

        public IARRef? TryGetReference()
        {
            if (Type == ArCommonType.Reference)
            {
                return Reference;
            }
            return null;
        }

        public IARRef GetReference()
        {
            if ((Type == ArCommonType.Reference) && (Reference != null))
            {
                return Reference;
            }
            throw new Exception("Type is not Reference");
        }

        public IEnumerable<IARRef>? TryGetReferences()
        {
            if (Type == ArCommonType.References)
            {
                return References;
            }
            return null;
        }

        public IEnumerable<IARRef> GetReferences()
        {
            if ((Type == ArCommonType.References) && (References != null))
            {
                return References;
            }
            throw new Exception("Type is not References");
        }

        public ArCommonList GetCommonReferences()
        {
            ArCommonList result = new();

            if ((Type == ArCommonType.References) && (References != null))
            {
                foreach (var r in References)
                {
                    result.Add(new ArCommon(r, Role, this));
                }
                return result;
            }
            throw new Exception("Type is not References");
        }

        public IMetaObjectInstance? TryGetMeta()
        {
            if (Type == ArCommonType.Meta)
            {
                return Meta;
            }
            return null;
        }

        public IMetaObjectInstance GetMeta()
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                return Meta;
            }
            throw new Exception("Type is not MetaObject");
        }

        public IEnumerable<IMetaObjectInstance>? TryGetMetas()
        {
            if (Type == ArCommonType.Metas)
            {
                return Metas;
            }
            return null;
        }

        public IEnumerable<IMetaObjectInstance> GetMetas()
        {
            if ((Type == ArCommonType.Metas) && (Metas != null))
            {
                return Metas;
            }
            throw new Exception("Type is not MetaObjects");
        }

        public ArCommonList GetCommonMetas()
        {
            ArCommonList result = new();

            if ((Type == ArCommonType.Metas) && (Metas != null))
            {
                Dictionary<string, string[]>? filter = null;
                if (Role != null)
                {
                    if (arFilter.ContainsKey(Role.Name))
                    {
                        filter = arFilter[Role.Name];
                    }
                }
                foreach (var m in Metas)
                {
                    if ((filter != null) && (Role != null))
                    {
                        if (Role.MultipleInterfaceTypes)
                        {
                            if (filter.ContainsKey("Include"))
                            {
                                if (!filter["Include"].Contains(m.InterfaceType.Name[1..]))
                                {
                                    continue;
                                }
                            }
                            else if (filter.ContainsKey("Exclude"))
                            {
                                if (filter["Exclude"].Contains(Role.Name))
                                {
                                    continue;
                                }
                            }
                        }
                    }
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

        public string? GetEnumName()
        {
            if ((Type == ArCommonType.Enum) && (Enum != null))
            {
                if (Role != null)
                {
                    var result =  Enum.GetName(Role.InterfaceType, Enum);
                    if (result != null)
                    {
                        return result[1..];
                    }
                }
                return null;
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

        public ArCommonList GetCommonEnums()
        {
            ArCommonList result = new();

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

        public string? GetEnumsName(int index)
        {
            if ((Type == ArCommonType.Enums) && (Enums != null))
            {
                if ((Role != null) && (index < Enums.Count))
                {
                    int count = 0;
                    foreach (var e in Enums)
                    {
                        if (count == index)
                        {
                            var result = Enum.GetName(Role.InterfaceType, e);
                            if (result != null)
                            {
                                return result[1..];
                            }
                        }
                        count++;
                    }
                }
                return null;
            }
            throw new Exception("Type is not Enums");
        }

        public bool? TryGetBool()
        {
            if (Type == ArCommonType.Bool)
            {
                return Bool;
            }
            return null;
        }

        public bool GetBool()
        {
            if (Type == ArCommonType.Bool)
            {
                if (Bool != null)
                {
                    return (bool)Bool;
                }
                else
                {
                    return false;
                }
            }
            throw new Exception("Type is not Bool");
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

        public ArCommonList GetCommonObjs()
        {
            ArCommonList result = new();

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

        public ArCommonList GetCommons()
        {
            ArCommonList result;

            if ((Type == ArCommonType.Metas) && (Metas != null))
            {
                result = GetCommonMetas();
            }
            else if ((Type == ArCommonType.Enums) && (Enums != null))
            {
                result = GetCommonEnums();
            }
            else if((Type == ArCommonType.Others) && (Objs != null))
            {
                result = GetCommonObjs();
            }
            else if ((Type == ArCommonType.References) && (References != null))
            {
                result = GetCommonReferences();
            }
            else
            {
                result = new();
            }
            return result;
        }

        public ArCommonList GetExistingMember()
        {
            ArCommonList result = new();
            Dictionary<string, string[]>? filter = null;
            if (Role != null)
            {
                if (arFilter.ContainsKey(Role.Name))
                {
                    filter = arFilter[Role.Name];
                }
            }

            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                foreach (var o in Meta.MetaAllRoles)
                {
                    if (o.RoleType == RoleTypeEnum.Reference)
                    {
                        continue;
                    }

                    if (filter != null)
                    {
                        if (filter.ContainsKey("Include"))
                        {
                            if (!filter["Include"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                        else if (filter.ContainsKey("Exclude"))
                        {
                            if (filter["Exclude"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                    }

                    if (o.Single())
                    {
                        if (o.Option())
                        {
                            if (Meta.IsSpecified(o.Name))
                            {
                                result.Add(new ArCommon(Meta.GetValue(o.Name), o, this));
                            }
                        }
                        else
                        {
                            result.Add(new ArCommon(Meta.GetValue(o.Name), o, this));
                        }
                    }
                    else if (o.Multiply())
                    {
                        var childObjs = Meta.GetCollectionValueRaw(o.Name);

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

        public List<IReferrable> GetReferencedFrom()
        {
            List<IReferrable> result = new();
            if (Meta.GetCollectionValueRaw("ReferencedFrom") is IMetaCollectionInstance collection)
            {
                foreach (var c in collection)
                {
                    if (c is IMetaObjectInstance meta)
                    {
                        if (meta.Owner is IReferrable referrable)
                        {
                            result.Add(referrable);
                        }
                    }
                }
            }
            return result;
        }

        public List<IMetaRI> GetCandidateMember()
        {
            List<IMetaRI> result = new();
            Dictionary<string, string[]>? filter = null;
            if (Role != null)
            {
                if (arFilter.ContainsKey(Role.Name))
                {
                    filter = arFilter[Role.Name];
                }
            }

            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                foreach (var o in Meta.MetaAllRoles)
                {
                    if (o.RoleType == RoleTypeEnum.Reference)
                    {
                        continue;
                    }

                    if (filter != null)
                    {
                        if (filter.ContainsKey("Include"))
                        {
                            if (!filter["Include"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                        else if (filter.ContainsKey("Exclude"))
                        {
                            if (filter["Exclude"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                    }

                    if (o.Single())
                    {
                        if (o.Option())
                        {
                            if (!Meta.IsSpecified(o.Name))
                            {
                                result.Add(o);
                            }
                        }
                    }
                    else if (o.Multiply())
                    {
                        var childObjs = Meta.GetCollectionValueRaw(o.Name);
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

        public ArCommonList GetAllMember()
        {
            ArCommonList result = new();
            IMetaObjectInstance? meta = null;

            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                meta = Meta;
            }
            else if ((Type == ArCommonType.Reference) && (Reference != null))
            {
                meta = Reference;
            }

            Dictionary<string, string[]>? filter = null;
            if (Role != null)
            {
                if (arFilter.ContainsKey(Role.Name))
                {
                    filter = arFilter[Role.Name];
                }
            }

            if (meta != null)
            {
                foreach (var o in meta.MetaAllRoles)
                {
                    if (o.RoleType == RoleTypeEnum.Reference)
                    {
                        continue;
                    }

                    if (filter != null)
                    {
                        if (filter.ContainsKey("Include"))
                        {
                            if (!filter["Include"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                        else if (filter.ContainsKey("Exclude"))
                        {
                            if (filter["Exclude"].Contains(o.Name))
                            {
                                continue;
                            }
                        }
                    }

                    if (o.Single())
                    {
                        if (o.Option())
                        {
                            if (meta.IsSpecified(o.Name))
                            {
                                result.Add(new ArCommon(meta.GetValue(o.Name), o, this));
                            }
                            else
                            {
                                result.Add(new ArCommon(null, o, this));
                            }
                        }
                        else
                        {
                            result.Add(new ArCommon(meta.GetValue(o.Name), o, this));
                        }
                    }
                    else if (o.Multiply())
                    {
                        var childObjs = meta.GetCollectionValueRaw(o.Name);

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
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                foreach (var o in Meta.MetaAllRoles)
                {
                    if (o.RoleType == RoleTypeEnum.Reference)
                    {
                        continue;
                    }

                    if (o.Single())
                    {
                        if (o.Required())
                        {
                            var v = Meta.GetValue(o.Name);
                            if (v == null)
                            {
                                if (o.InterfaceType.Name == "String")
                                {
                                    Meta.SetValue(o.Name, "");
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

        private object? AddObject(IMetaRI role, object? obj=null, bool notify = true)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                object? objAdd;
                var method = Meta.GetType().GetMethod($"Add{role.Name}");
                if (obj == null)
                {
                    if (role.InterfaceType.Name == "String")
                    {
                        objAdd = $"{role.Name}";
                    }
                    else
                    {
                        objAdd = Activator.CreateInstance(role.InterfaceType);
                    }
                }
                else
                {
                    objAdd = obj;
                }
                if ((method != null) && (objAdd != null))
                {
                    method.Invoke(Meta, new object[] { objAdd });
                    if (notify)
                    {
                        Changed?.Invoke();
                    }
                    return objAdd;
                }
            }
            return null;
        }

        public void RemoveAllObject(IMetaRI role, bool notify=true)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                var collection = Meta.GetCollectionValueRaw(role.Name);
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
                        RemoveObject(role, 0, false);
                    }
                }
                if (notify)
                {
                    Changed?.Invoke();
                }
            }
        }

        public void RemoveObject(IMetaRI role, Int32 index, bool notify=true)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                var collection = Meta.GetCollectionValueRaw(role.Name);
                if (collection is IMetaCollectionInstance metas)
                {
                    if (metas.Count > index)
                    {
                        var method = Meta.GetType().GetMethod($"Remove{role.Name}");
                        method?.Invoke(Meta, new object[] { index });
                        if (notify)
                        {
                            Changed?.Invoke();
                        }
                    }
                }
            }
        }

        public void RemoveObject(IMetaRI role, ArCommon c)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                var collection = Meta.GetCollectionValueRaw(role.Name);
                if (collection is IMetaCollectionInstance metas)
                {
                    Int32 index = 0;
                    foreach (var meta in metas)
                    {
                        if (meta.Equals(c.Obj))
                        {
                            RemoveObject(role, index);
                            Changed?.Invoke();
                            return;
                        }
                        index++;
                    }
                    throw new Exception($"No {role.Name} member {c.Obj} in {Meta}");
                }
            }
        }

        public void SetSpecified(IMetaRI role, bool isSpecifed)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                Meta.SetSpecified(role.Name, isSpecifed);
                Changed?.Invoke();
            }
        }

        public bool CanDelete()
        {
            if (Role != null)
            {
                if ((Type == ArCommonType.Meta) && (Meta != null))
                {
                    if ((uint)Role.MaxOccurs > 1)
                    {
                        var brothers = Meta.Owner.GetCollectionValue(Role.Name);

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

        public ArCommon? Add(IMetaRI role)
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                if (role.Single())
                {
                    if (role.Option())
                    {
                        Meta.SetSpecified(role.Name, true);
                        var newObj = Meta.GetValue(role.Name);
                        if (newObj is IReferrable referrable)
                        {
                            referrable.ShortName = role.Name;
                        }
                        var newCommon = new ArCommon(newObj, role, this);
                        newCommon.Check();
                        Changed?.Invoke();
                        return newCommon;
                    }
                }
                else if (role.Multiply())
                {
                    if (role.IsMeta())
                    {
                        var newObj = Meta.AddNew(role.Name, role.InterfaceType);
                        if (newObj is IReferrable referrable)
                        {
                            referrable.ShortName = role.Name;
                        }
                        var newCommon = new ArCommon(newObj, role, this);
                        newCommon.Check();
                        Changed?.Invoke();
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

        public ArCommon? AddMultipleInterface(IMetaRI role, Type type)
        {
            if (role.Multiply())
            {
                if (role.MultipleInterfaceTypes == true)
                {
                    var newObj = Meta.AddNew(role.Name, type);
                    if (newObj is IReferrable referrable)
                    {
                        if (type != null)
                        {
                            referrable.ShortName = type.Name[1..];
                        }
                    }
                    var newCommon = new ArCommon(newObj, role, this);
                    newCommon.Check();
                    Changed?.Invoke();
                    return newCommon;
                }
            }
            return null;
        }

        public List<Type> RoleTypesFor(string roleName)
        {
            List<Type> result = new();
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                Dictionary<string, string[]>? filter = null;
                if (Role != null)
                {
                    if (arFilter.ContainsKey(roleName))
                    {
                        filter = arFilter[roleName];
                    }
                }
                foreach (var t in Meta.RoleTypesFor(roleName))
                {
                    if (filter != null)
                    {
                        if (filter.ContainsKey("Include"))
                        {
                            if (!filter["Include"].Contains(t.Name[1..]))
                            {
                                continue;
                            }
                        }
                        else if (filter.ContainsKey("Exclude"))
                        {
                            if (filter["Include"].Contains(t.Name[1..]))
                            {
                                continue;
                            }
                        }
                    }
                    result.Add(t);
                }
            }
            return result;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case ArCommonType.None:
                    return "None";

                case ArCommonType.Meta:
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

                case ArCommonType.Metas:
                    if (Role != null)
                    {
                        return $"{Role.Name}(s)";
                    }
                    return "";

                case ArCommonType.Enum:
                    if (Enum != null)
                    {
                        return Enum.ToString()[1..];
                    }
                    return "";

                case ArCommonType.Enums:
                    if (Role != null)
                    {
                        return $"{Role.Name}(s)";
                    }
                    return "";

                case ArCommonType.Integer:
                    if (Integer != null)
                    {
                        if (Integer.Signed)
                        {
                            return Integer.SignedValue.ToString();
                        }
                        else
                        {
                            return Integer.UnsignedValue.ToString();
                        }
                    }
                    return "";

                case ArCommonType.Reference:
                    if (Reference != null)
                    {
                        return $"{Reference.Value.Split("/").Last()}(R)";
                    }
                    return "";

                case ArCommonType.References:
                    if (Role != null)
                    {
                        return $"{Role.Name}(Rs)";
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
            return ((Reference == null) && (References == null) && (Meta == null) && (Metas == null) &&
                    (Enum == null) && (Enums == null) && (Integer == null) && (Bool == null) &&
                    (Obj == null) && (Objs == null));
        }

        public bool IsReferrable()
        {
            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                if (Meta is IReferrable)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> EnumCanditate()
        {
            List<string> result = new();

            if (((Type == ArCommonType.Enum) || (Type == ArCommonType.Enums)) && (Role != null))
            {
                foreach (var name in Enum.GetNames(Role.InterfaceType))
                {
                    result.Add(name[1..]);
                }
            }
            return result;
        }

        public List<Type> ReferenceCanditate()
        {
            List<Type> result = new();

            if (((Type == ArCommonType.Reference) || (Type == ArCommonType.References)) && (Role != null))
            {
                var assembly = Assembly.GetAssembly(Role.InterfaceType);

                if (assembly != null )
                {
                    var types = assembly.GetTypes();

                    if (types != null )
                    {
                        var nameFound = Role.InterfaceType.Name[..^4];
                        Type? typeFound = null;

                        foreach (var type in types)
                        {
                            if (type.Name == nameFound)
                            {
                                typeFound = type;
                                break;
                            }
                        }

                        foreach (var type in types)
                        {
                            if ((type.GetInterfaces().Contains(typeFound)) && (type.Namespace == "GenTool_CsDataServerDomAsr4.InternalParser"))
                            {
                                result.Add(type);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public Dictionary<Type, List<IReferrable>> ReferenceExisting()
        {
            Dictionary<Type, List<IReferrable>> result = new();
            foreach (var reference in ReferenceCanditate())
            {
                var resultPartial = new List<IReferrable>();
                foreach (var obj in AllObjects())
                {
                    if ((obj.InterfaceType.Name[1..] == reference.Name[1..]) && (obj is IReferrable referrable))
                    {
                        resultPartial.Add(referrable);
                    }
                }
                result[reference] = resultPartial;
            }
            return result;
        }

        public void SetEnum(string name)
        {
            if ((Type == ArCommonType.Enum) && (Role != null))
            {
                try
                {
                    Enum = Enum.Parse(Role.InterfaceType, $"e{name}", true) as Enum;
                    if (Parent.Type == ArCommonType.Meta)
                    {
                        Parent.GetMeta().SetValue(Role.Name, Enum);
                    }
                }
                catch
                {
                    Enum = Enum.Parse(Role.InterfaceType, $"t{name}", true) as Enum;
                    if (Parent.Type == ArCommonType.Meta)
                    {
                        Parent.GetMeta().SetValue(Role.Name, Enum);
                    }
                }
            }
        }

        public void SetEnums(int index, string name)
        {
            if ((Type == ArCommonType.Enums) && (Role != null) && (Enums != null))
            {
                if ((index >= 0) && (index < Enums.Count))
                {
                    List<Enum> results = new();
                    int count = 0;
                    foreach (var e in Enums)
                    {
                        if (e is Enum ee)
                        {
                            if (count == index)
                            {
                                var newE = Enum.Parse(Role.InterfaceType, $"e{name}", true);
                                if (newE is Enum newEE)
                                {
                                    results.Add(newEE);
                                }
                                else
                                {
                                    results.Add(ee);
                                }
                            }
                            else
                            {
                                results.Add(ee);
                            }
                        }
                        count++;
                    }
                    Parent.RemoveAllObject(Role, false);
                    foreach (var e in results)
                    {
                        Parent.AddObject(Role, e, false);
                    }
                    Changed?.Invoke();
                }
            }
        }

        public void SetBool(bool newValue)
        {
            if ((Type == ArCommonType.Bool) && (Role != null))
            {
                if (Parent.Type == ArCommonType.Meta)
                {
                    Parent.GetMeta().SetValue(Role.Name, newValue);
                    Changed?.Invoke();
                }
            }
        }

        public void SetOther(object newValue)
        {
            if ((Type == ArCommonType.Other) && (Role != null))
            {
                if ((Parent.Type == ArCommonType.Meta) && (Parent.Meta != null))
                {
                    Parent.Meta.SetValue(Role.Name, newValue);
                    Changed?.Invoke();
                }
                else if ((Parent.Type == ArCommonType.Reference) && (Parent.Reference != null))
                {
                    Parent.Reference.SetValue(Role.Name, newValue);
                    Changed?.Invoke();
                }
            }
            else if ((Type == ArCommonType.Integer) && (Role != null) && (Integer != null))
            {
                if (newValue is string s)
                {
                    if (UInt64.TryParse(s, out ulong v))
                    {
                        Integer.Signed = false;
                        Integer.UnsignedValue = v;
                    }
                    else if (Int64.TryParse(s, out long sv))
                    {
                        Integer.Signed = true;
                        Integer.SignedValue = sv;
                    }
                }
            }
        }

        public void SetOthers(int index, object newValue)
        {
            if ((Type == ArCommonType.Others) && (Role != null) && (Objs != null))
            {
                if ((index >= 0) && (index < Objs.Count()))
                {
                    List<object> results = new();
                    int count = 0;
                    foreach (var o in Objs)
                    {
                        if (count == index)
                        {
                            results.Add(newValue);
                        }
                        else
                        {
                            results.Add(o);
                        }
                        count++;
                    }
                    Parent.RemoveAllObject(Role, false);
                    foreach (var e in results)
                    {
                        Parent.AddObject(Role, e, false);
                    }
                    Changed?.Invoke();
                }
            }
        }

        public IReferrable[] GetReferenceRaw()
        {
            List<IReferrable> result = new();

            if ((Type == ArCommonType.Reference) && (Reference != null))
            {
                var property = Reference.GetType().GetProperty($"ObjectList");
                if (property != null)
                {
                    var value = property.GetValue(Reference);

                    if (value is IMetaCollectionInstance metas)
                    {
                        foreach (var meta in metas)
                        {
                            if (meta is IReferrable referrable)
                            {
                                result.Add(referrable);
                            }
                        }
                    }
                }
                return result.ToArray();
            }
            return Array.Empty<IReferrable>();
        }

        public string GetPrimitiveConstraints()
        {
            string result = "";

            if (Role != null)
            {
                if ((Parent.Type == ArCommonType.Meta) && (Parent.Meta != null))
                {
                    foreach (var i in Parent.Meta.InterfaceType.GetTypeInfo().ImplementedInterfaces)
                    {
                        var members = i.GetMember(Role.Name);
                        if (members.Length > 0)
                        {
                            foreach (var m in members)
                            {
                                foreach (var d in m.GetCustomAttributesData())
                                {
                                    if (d.AttributeType.Name == "PrimitiveConstraintsAttribute")
                                    {
                                        foreach (var a in d.ConstructorArguments)
                                        {
                                            result += $"^{a.ToString()[1..^1]}";
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private string GetRoleTypeArDocument()
        {
            string result = "";

            if (Role != null)
            {
                foreach (var d in Role.InterfaceType.GetCustomAttributesData())
                {
                    if (d.AttributeType.Name == "AutosarDocumentationAttribute")
                    {
                        if (d.ConstructorArguments.Count > 0)
                        {
                            result += $"{d.ConstructorArguments[0]}{Environment.NewLine}";
                        }
                    }
                }
            }

            if (result == "")
            {
                result = Environment.NewLine;
            }
            return result;
        }

        private string GetMetaArDocument()
        {
            string result = "";

            if ((Type == ArCommonType.Meta) && (Meta != null))
            {
                result += $"Internal Type: {Meta.InterfaceType.Name[1..]}{Environment.NewLine}";
                foreach (var d in Meta.InterfaceType.GetCustomAttributesData())
                {
                    if (d.AttributeType.Name == "AutosarDocumentationAttribute")
                    {
                        foreach (var a in d.ConstructorArguments)
                        {
                            result += $"Desc: {a}{Environment.NewLine}";
                        }
                    }
                }

                foreach (var d in Meta.InterfaceType.GetCustomAttributesData())
                {
                    if (d.AttributeType.Name == "PrimitiveConstraintsAttribute")
                    {
                        foreach (var a in d.ConstructorArguments)
                        {
                            result += $"Constraint: {a}{Environment.NewLine}";
                        }
                    }
                }
            }

            if (result == "")
            {
                result = Environment.NewLine;
            }
            return result;
        }

        private string GetRoleArDocument()
        {
            var result = "";

            if (Role != null)
            {
                if ((Parent.Type == ArCommonType.Meta) && (Parent.Meta != null))
                {
                    foreach (var i in Parent.Meta.InterfaceType.GetTypeInfo().ImplementedInterfaces)
                    {
                        var members = i.GetMember(Role.Name);
                        if (members.Length > 0)
                        {
                            foreach (var m in members)
                            {
                                foreach (var d in m.GetCustomAttributesData())
                                {
                                    if (d.AttributeType.Name == "AutosarDocumentationAttribute")
                                    {
                                        foreach (var a in d.ConstructorArguments)
                                        {
                                            result += $"RoleDesc: {a}{Environment.NewLine}";
                                        }
                                    }
                                }

                                foreach (var d in m.GetCustomAttributesData())
                                {
                                    if (d.AttributeType.Name == "PrimitiveConstraintsAttribute")
                                    {
                                        foreach (var a in d.ConstructorArguments)
                                        {
                                            result += $"Constraint: {a}{Environment.NewLine}";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public string GetDesc()
        {
            string result = "";

            if (Role != null)
            {
                result += $"Type: {Role.Name}{Environment.NewLine}";
                result += $"Min: {Role.Min()}{Environment.NewLine}";
                result += $"Max: {Role.Max()}{Environment.NewLine}";
                result += $"Desc: {GetRoleTypeArDocument()}{Environment.NewLine}";

                if (Role.MultipleInterfaceTypes)
                {
                    result += GetMetaArDocument();
                }
                result += GetRoleArDocument();
            }
            return result;
        }

        public bool Empty()
        {
            bool result = true;

            if (Type == ArCommonType.Meta)
            {
                if (GetCandidateMember().Count != 0)
                {
                    result = false;
                }
            }
            else if ((Type == ArCommonType.Metas) && (Metas != null))
            {
                if (Metas.Any())
                {
                    result = false;
                }
            }
            return result;
        }

        public ArCommonList this[string name]
        {
            get
            {
                ArCommonList result = new();

                if (Type == ArCommonType.Meta)
                {
                    Dictionary<string, string[]>? filter = null;

                    if (Role != null)
                    {
                        if (arFilter.ContainsKey(Role.Name))
                        {
                            filter = arFilter[Role.Name];
                        }
                    }

                    if (Meta != null)
                    {
                        if ((Role != null) && (Role.MultipleInterfaceTypes))
                        {
                            if (Meta.InterfaceType.Name[1..] == name)
                            {
                                result.Add(this);
                            }
                        }
                        else
                        {
                            foreach (var o in Meta.MetaAllRoles)
                            {
                                if (o.RoleType == RoleTypeEnum.Reference)
                                {
                                    continue;
                                }

                                if (o.Name != name)
                                {
                                    continue;
                                }

                                if (filter != null)
                                {
                                    if (filter.ContainsKey("Include"))
                                    {
                                        if (!filter["Include"].Contains(o.Name))
                                        {
                                            continue;
                                        }
                                    }
                                    else if (filter.ContainsKey("Exclude"))
                                    {
                                        if (filter["Exclude"].Contains(o.Name))
                                        {
                                            continue;
                                        }
                                    }
                                }

                                if (o.Single())
                                {
                                    if (o.Option())
                                    {
                                        if (Meta.IsSpecified(o.Name))
                                        {
                                            result.Add(new ArCommon(Meta.GetValue(o.Name), o, this));
                                        }
                                    }
                                    else
                                    {
                                        result.Add(new ArCommon(Meta.GetValue(o.Name), o, this));
                                    }
                                }
                                else if (o.Multiply())
                                {
                                    var childObjs = Meta.GetCollectionValueRaw(o.Name);

                                    if (childObjs is IMetaCollectionInstance collection)
                                    {
                                        result.AddRange(new ArCommon(collection, o, this).GetCommons());
                                    }
                                }
                            }
                        }
                    }
                }
                else if((Type == ArCommonType.Metas) || (Type == ArCommonType.References))
                {
                    foreach (var c in GetCommons())
                    {
                        result.AddRange(c[name]);
                    }
                }
                return result;
            }
        }

        public delegate bool FilterFunc(ArCommon common);
        public ArCommonList this[string name, FilterFunc func]
        {
            get
            {
                ArCommonList result = new();

                foreach (var c in this[name])
                {
                    if (func(c))
                    {
                        result.Add(c);
                    }
                }
                return result;
            }
        }
    }

    public class ArCommonList : List<ArCommon>
    {
        public ArCommonList this[string name]
        {
            get
            {
                ArCommonList result = new();

                foreach (var c in this)
                {
                    result.AddRange(c[name]);
                }
                return result;
            }
        }

        public delegate bool FilterFunc(ArCommon common);
        public ArCommonList this[string name, FilterFunc func]
        {
            get
            {
                ArCommonList result = new();

                foreach (var c in this[name])
                {
                    if (func(c))
                    {
                        result.Add(c);
                    }
                }
                return result;
            }
        }
    }
}
