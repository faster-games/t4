using System;
using Object = UnityEngine.Object;

namespace FasterGames.T4.Runtime
{
    /// <summary>
    /// Represents a Template parsing host that contains user data
    /// </summary>
    /// <typeparam name="TData">user data type</typeparam>
    public interface IDataHost<out TData> where TData : Object
    {   
        /// <summary>
        /// The user data
        /// </summary>
        public TData Data { get; }
    }
}