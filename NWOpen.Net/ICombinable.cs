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
