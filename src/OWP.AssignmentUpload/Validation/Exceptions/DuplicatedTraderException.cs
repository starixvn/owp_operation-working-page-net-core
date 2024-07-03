using System.Runtime.Serialization;

namespace OWP.AssignmentUpload.Validation.Exceptions;

public class DuplicatedTraderException(string traders) : CriticalValidationException($"{traders} has already been used by someone else. Please check again!")
{
}