using System.Runtime.Serialization;

namespace OWP.OtherSport.Validation.Exceptions;


public class CriticalValidationException(string message) : Exception(message)
{
}