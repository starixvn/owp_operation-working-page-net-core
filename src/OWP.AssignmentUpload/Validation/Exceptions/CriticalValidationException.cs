using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

public class CriticalValidationException(string message) : Exception(message)
{
}