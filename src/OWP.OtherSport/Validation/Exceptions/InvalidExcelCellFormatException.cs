
namespace OWP.OtherSport.Validation.Exceptions;


public class InvalidExcelCellFormatException(string message) : CriticalValidationException(message)
{
}