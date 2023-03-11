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
using System.Collections;

namespace ArxmlEditor.Model
{
    public static class ObjectExtend
    {
        public static bool CanDelete(this object obj, IMetaRI role)
        {
            if (obj is IMetaObjectInstance mObj)
            {
                if ((uint)role.MaxOccurs > 1)
                {
                    var brothers = mObj.Owner.GetCollectionValue(role.Name);

                    if (brothers.Count() > role.MinOccurs)
                    {
                        return true;
                    }
                }
                else
                {
                    if (role.MinOccurs == 0)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if ((uint)role.MinOccurs == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CanEdit(this object obj)
        {
            if ((obj is not IEnumerable) || (obj is string))
            {
                return true;
            }
            if (obj is IReferrable)
            {
                return true;
            }
            return false;
        }
    }
}
