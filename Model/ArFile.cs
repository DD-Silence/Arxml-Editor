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

using GenTool_CsDataServerDomAsr4;
using GenTool_CsDataServerDomAsr4.Iface;

namespace ArxmlEditor.Model
{
    internal class ArFile
    {
        private readonly List<string> paths = new();
        public readonly IAUTOSAR? root;

        public ArFile(List<string> filePaths)
        {
            paths = filePaths;

            var domain = DomainFactory.Instance.Create();
            if (domain != null)
            {
                domain.Load(paths.ToArray(), true);
                root = domain.Model;
            }
            else
            {
                throw new Exception("Fail to create domain.");
            }
        }

        public void Save()
        {
            root?.Domain.Save();
        }
    }
}
