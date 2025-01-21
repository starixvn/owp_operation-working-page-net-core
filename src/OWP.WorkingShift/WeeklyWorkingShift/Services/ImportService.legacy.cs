
using Fanex.NAP.Client;

namespace OWP.WorkingShift.WeeklyWorkingShift.Services;

using Criteria;
using Fanex.Data.Repository;
using Models;
using OfficeOpenXml;
using OWP.Models;
using OWP.WorkingShift.Common.Repositories;
using OWP.WorkingShift.Common.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

public interface ImportService
{
    Task<ValidateImportResult> Validate(Stream stream);

    Task Import(IList<WorkingShiftImportEntity> shifts);
}

public class ImportServiceImp(
    WorkingShiftColorRepository workingShiftColorRepository,
    IDynamicRepository dynamicRepository,
    INapSession currentSession,
    UserRepository userRepository) : ImportService
{
    public async Task Import(IList<WorkingShiftImportEntity> shifts)
    {
        var uploadData = new WorkingShiftUploadData
        {
            WorkingShifts = shifts.ToArray()
        };
        var criteria = new EmployeeWorkingShiftUploadCriteria
        {
            ActorId = currentSession.UserId,
            Data = uploadData.ToXmlString()
        };

        await dynamicRepository.ExecuteAsync(criteria);
    }

    public async Task<ValidateImportResult> Validate(Stream stream)
    {
        try
        {
            using (var package = new ExcelPackage(stream))
            {
                if (package.Workbook == null || !package.Workbook.Worksheets.Any())
                {
                    throw new IOException("Workbook is empty or has no sheet");
                }

                var sheet = package.Workbook.Worksheets.First();
                return await Validate(sheet);
            }
        }
        catch (Exception ex)
        {
            return new ValidateImportResult
            {
                InvalidCells = new List<ImportInvalidCell> { new("", "Error", ex.Message) }
            };
        }
        finally
        {
            await stream.FlushAsync();
            await stream.DisposeAsync();
        }
    }

    private async Task<ValidateImportResult> Validate(ExcelWorksheet sheet)
    {
        var invalidCells = ValidateHeaderFormat(sheet).ToList();
        if (invalidCells.Any())
        {
            return new ValidateImportResult
            {
                InvalidCells = invalidCells
            };
        }

        var importCells = await ValidateData(sheet);
        invalidCells = importCells.OfType<ImportInvalidCell>().ToList();
        if (invalidCells.Any())
        {
            return new ValidateImportResult
            {
                InvalidCells = invalidCells
            };
        }

        var validCells = importCells.OfType<ImportValidCell>();
        var shifts = validCells.Select(x => new WorkingShiftImportEntity
        {
            EmployeeId = x.EmployeeId,
            ShiftDate = x.ShiftDate,
            ShiftId = x.ShiftId
        }).ToList();
        return new ValidateImportResult
        {
            Shifts = shifts
        };
    }

    private static DateTime? GetDateTime(ExcelWorksheet sheet, int row, int column)
    {
        try
        {
            if (sheet.Cells[row, column].Value == null)
            {
                return null;
            }

            var stringValue = sheet.Cells[row, column].Value.ToString();
            var dateStringFormat = "d-MMM";
            if (DateTime.TryParseExact(stringValue, dateStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                return result;
            }

            dateStringFormat = "dd-MMM";
            if (DateTime.TryParseExact(stringValue, dateStringFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))

            {
                return result;
            }
            var dateNum = long.Parse(stringValue);
            result = DateTime.FromOADate(dateNum);

            return result;
        }
        catch
        {
            return null;
        }
    }

    private static IEnumerable<ImportInvalidCell> ValidateHeaderFormat(ExcelWorksheet sheet)
    {
        const int firstRow = 1;
        var maxColumn = sheet.Dimension.End.Column;
        const int employeeIdColumn = 1;
        const int nickNameColumn = 2;
        const int dateColumn = 3;
        for (var col = firstRow; col <= maxColumn; col++)
        {
            var cell = sheet.Cells[firstRow, col];
            if (col == employeeIdColumn && cell.Value == null)
            {
                yield return new ImportInvalidCell(cell.Address, "EmployeeId", "EmployeeId column does not exist");
            }
            if (col == nickNameColumn && (cell.Value == null || cell.Value.ToString().ToLowerInvariant() != "nickname"))
            {
                yield return new ImportInvalidCell(cell.Address, "NickName", "Nickname column is incorrect format");
            }
            if (col >= dateColumn && cell.Value != null)
            {
                var date = GetDateTime(sheet, firstRow, col);
                if (date == null)
                {
                    yield return new ImportInvalidCell(cell.Address, "Date", $"[{cell.Value}] The date is an incorrect format");
                }
            }
        }
    }

#pragma warning disable S1541 // Methods and properties should not be too complex
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    private async Task<IList<ImportCell>> ValidateData(ExcelWorksheet sheet)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
#pragma warning restore S1541 // Methods and properties should not be too complex
    {
        var minDateOfWorkingShift = GetMinDateOfWorkingShift();
        var result = new List<ImportCell>();
        var workingShiftColors = await workingShiftColorRepository.GetAll();
        var users = await userRepository.GetAllUsers();
        users = users.Where(x => !x.IsSecondAccount && !string.IsNullOrEmpty(x.EmployeeId)).ToList();
        const int firstRow = 1;
        const int dataRow = 2;
        const int employeeIdColumn = 1;
        const int dateColumn = 3;
        var totalRow = sheet.Dimension.End.Row;
        const int totalColumn = 9;
        var employeeIdsExists = new List<string>();

        for (var col = dateColumn; col <= totalColumn; col++)
        {
            var shiftDate = GetDateTime(sheet, firstRow, col);
            if (shiftDate < minDateOfWorkingShift)
            {
                result.Add(new ImportInvalidCell(new ExcelAddress(firstRow, col, firstRow, col).Address, "Shift Date", $"Shift Date must be after {minDateOfWorkingShift.ToShortDateString()}"));
            }
        }

        for (var row = dataRow; row <= totalRow; row++)
        {
            var cell = sheet.Cells[row, employeeIdColumn];
            var employeeId = cell.Value;

            if (employeeIdsExists.TrueForAll(x => x != employeeId.ToString()))
            {
                employeeIdsExists.Add(employeeId.ToString());
            }
            else
            {
                result.Add(new ImportInvalidCell(cell.Address, "EmployeeId", $"EmployeeId [{cell.Value}] duplicated"));
            }

            var validateResultUserIds = ValidateEmployeeIdAndGetUserIds(cell, users);
            if (validateResultUserIds.Select(x => x.Invalid).Any())
            {
                result.AddRange(validateResultUserIds.Select(x => x.Invalid));
            }

            var userIds = validateResultUserIds.Select(x => x.Value).ToList();

            for (var col = dateColumn; col <= totalColumn; col++)
            {
                cell = sheet.Cells[row, col];
                var validateResultShiftCode = ValidateShiftCode(cell, workingShiftColors);
                if (validateResultShiftCode?.Invalid != null)
                {
                    result.Add(validateResultShiftCode.Invalid);
                }

                if (validateResultShiftCode != null && validateResultShiftCode.Value > 0)
                {
                    var shiftId = validateResultShiftCode.Value;
                    var shiftDate = GetDateTime(sheet, firstRow, col);
                    var importCells = GetImportValidCell(cell, shiftDate.Value, userIds, shiftId);
                    result.AddRange(importCells);
                }
            }
        }

        return result;
    }

    private static List<ImportCell> GetImportValidCell(ExcelRange cell, DateTime shiftDate, List<int> userIds, int shiftId)
    {
        var result = new List<ImportCell>();

        foreach (var userId in userIds)
        {
            result.Add(new ImportValidCell(cell.Address, userId, shiftDate, shiftId));
        }

        return result;
    }

    private static List<ValidateCellResult> ValidateEmployeeIdAndGetUserIds(ExcelRange cell, IList<WeeklyWorkingShiftUser> users)
    {
        var result = new List<ValidateCellResult>();
        if (cell.Value == null)
        {
            result.Add(new ValidateCellResult { Invalid = new ImportInvalidCell(cell.Address, "Name", "EmployeeId cannot be empty") });

            return result;
        }

        var employeeId = cell.Value.ToString();

        var listUsers = users.Where(x => x.EmployeeId.Equals(employeeId)).ToList();
        if (listUsers.Any())
        {
            foreach (var user in listUsers)
            {
                result.Add(new ValidateCellResult { Value = user.UserId });
            }
        }
        else
        {
            result.Add(new ValidateCellResult { Invalid = new ImportInvalidCell(cell.Address, "Name", $"EmployeeId [{employeeId}] does not exist") });
        }
        return result;
    }

#pragma warning disable S1541 // Methods and properties should not be too complex
    private static ValidateCellResult ValidateShiftCode(ExcelRange cell, IList<WorkingShiftColorModel> workingShiftColors)
#pragma warning restore S1541 // Methods and properties should not be too complex
    {
        var shiftCode = string.Empty;
        if (cell.Value != null)
        {
            shiftCode = cell.Value.ToString().Trim();
        }

        if (string.IsNullOrEmpty(shiftCode))
        {
            return null;
        }

        if (shiftCode.Contains('<') || shiftCode.Contains(','))
        {
            shiftCode = WebUtility.HtmlEncode(shiftCode);
            return new ValidateCellResult
            {
                Invalid = new ImportInvalidCell(cell.Address, "Shift", $"Shift [{shiftCode}] should not contain specific characters like < or ,")
            };
        }

        shiftCode = shiftCode.ToUpperInvariant();
        var shiftCodeWithExtension = shiftCode.ToShiftCodeWithExtension();
        shiftCode = shiftCodeWithExtension.Item1;
        var extension = shiftCodeWithExtension.Item2;

        var shiftTime = HelperService.ParseShiftTimeSpan(shiftCode);
        var searchShiftColor = workingShiftColors.FirstOrDefault(x =>
        {
            if (shiftTime != null)
            {
                return x.IsTimeShift && x.StartTime == shiftTime.StartTime && x.EndTime == shiftTime.EndTime && x.Extension == extension;
            }

            return !x.IsTimeShift && string.Equals(x.ShiftCode.Trim(), shiftCode.Trim(), StringComparison.InvariantCultureIgnoreCase);
        });

        return searchShiftColor == null
            ? new ValidateCellResult
            {
                Invalid = new ImportInvalidCell(cell.Address, "ShiftNotExists", $"Shift [{shiftCode.ToShiftCodeWithExtension(extension)}] does not exist")
            }
            : new ValidateCellResult
            {
                Value = searchShiftColor.ShiftId
            };
    }

    private static DateTime GetMinDateOfWorkingShift()
    {
        var today = DateTimeOffset.Now.Date;
        var FOUR_WEEK = 28;
        return StartOfWeek(today).AddDays(-FOUR_WEEK);
    }

    public static DateTime StartOfWeek(DateTime value)
    {
        var diff = (7 + (value.DayOfWeek - DayOfWeek.Monday)) % 7;
        return value.AddDays(-1 * diff).Date;
    }
}