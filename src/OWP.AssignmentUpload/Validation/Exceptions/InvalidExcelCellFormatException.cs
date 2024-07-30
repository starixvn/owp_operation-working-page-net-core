
namespace OWP.AssignmentUpload.Validation.Exceptions;


public class InvalidExcelCellFormatException(string message) : CriticalValidationException(message)
{
}