﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.World
{
    public interface ITileObject
    {
        TileObjectType TileType { get; }
    }
}
