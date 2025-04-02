// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

namespace NWOpen.Net.Exceptions;

public class NWOpenException(string message, Exception e) : Exception(message, e)
{
    // This exception is thrown when there is an error with the NWOpen API.
    // It contains the message and the inner exception that caused the error.
}
