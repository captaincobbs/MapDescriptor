using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDescriptorTest.Entity
{
    /// <summary>
    /// A class that uses this interface will have an entity
    /// </summary>
    public interface IHasEntity
    {
        /// <summary>
        /// Returns <see cref="EntityProperties"/>
        /// </summary>
        /// <returns>EntityProperty</returns>
        EntityProperties GetEntityProperties();
    }
}
