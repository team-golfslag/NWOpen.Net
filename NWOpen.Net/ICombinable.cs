// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

namespace NWOpen.Net;

/// <summary>
/// Interface for combining two objects of the same type.
/// </summary>
/// <typeparam name="T">The type of object to combine.</typeparam>
public interface ICombinable<T>
{
    /// <summary>
    /// Combine two objects of the same type.
    /// </summary>
    /// <param name="other">The other object to combine with this one.</param>
    /// <returns>The combined object.</returns>
    public T Combine(T other);
}
