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

        void Update();
    }
}
