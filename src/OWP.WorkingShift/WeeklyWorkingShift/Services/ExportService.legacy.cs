

namespace OWP.WorkingShift.WeeklyWorkingShift.Services;

using Constants;
using Enumerations;
using Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OWP.Services;
using OWP.WorkingShift.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;
using WorkingShiftModel = Common.Models.WorkingShiftModel;

public interface ExportService
{
    Task Export(Stream stream, ExportModel viewModel);
}

public class ExportServiceImp : ExportService
{
    private readonly INapSession currentSession;
    private readonly RefResourceService resourceRepository;
    private readonly WorkingShiftTimezoneSettingRepository timezoneSettingRepository;
    private readonly UserRepository userRepository;
    private readonly Repositories.Repository repository;
    private readonly WorkingShiftRepository workingShiftRepository;
    private readonly WorkingShiftColorRepository workingShiftColorRepository;
    private readonly ConfigurationService configurationService;

    public ExportServiceImp(
        INapSession currentSession,
        RefResourceService resourceRepository,
        WorkingShiftTimezoneSettingRepository timezoneSettingRepository,
        UserRepository userRepository,
        Repositories.Repository repository,
        WorkingShiftRepository workingShiftRepository,
        WorkingShiftColorRepository workingShiftColorRepository, ConfigurationService configurationService)
    {
        this.currentSession = currentSession;
        this.resourceRepository = resourceRepository;
        this.timezoneSettingRepository = timezoneSettingRepository;
        this.userRepository = userRepository;
        this.repository = repository;
        this.workingShiftRepository = workingShiftRepository;
        this.workingShiftColorRepository = workingShiftColorRepository;
        this.configurationService = configurationService;
    }

    public async Task Export(Stream stream, ExportModel viewModel)
    {
        var exportModel = await BuildWorkingShiftExportModel(viewModel);

        var templatePath = @$"App_Data{Path.DirectorySeparatorChar}Templates{Path.DirectorySeparatorChar}ExportWorkingShift.xlsx";
        var absoluteTemplatePath = $"{configurationService.CurrentDirectory}{Path.DirectorySeparatorChar}{templatePath}";

        using (var fileStream = new FileStream(absoluteTemplatePath, FileMode.Open, FileAccess.Read))
        {
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                await Export(worksheet, viewModel, exportModel);
                var excelPassword = PasswordUtils.BuildPassword(viewModel.IsFrame);
                await package.SaveAsAsync(stream, excelPassword);
            }
        }
    }

    public async Task Export(ExcelWorksheet worksheet, ExportModel viewModel, WorkingShiftExportModel exportModel)
    {
        worksheet.Name = viewModel.GetSheetName();
        var row = 1;
        var col = 1;
        const int totalColumn = 13;
        const int firstDayColumn = 4;

        var listDates = new List<DateTime>();
        var fromDate = viewModel.FromDate;
        var toDate = viewModel.ToDate;
        for (var day = fromDate; day.Date <= toDate; day = day.AddDays(1))
        {
            listDates.Add(day);
        }

        // region Header
        worksheet.Cells[row, col].Value = worksheet.Cells[row, col].Value.ToString()
            .Replace("{from_date}", viewModel.FromDate.ToString("dd-MMM-yy"))
            .Replace("{to_date}", viewModel.ToDate.ToString("dd-MMM-yy"))
            .Replace("{gmt}", exportModel.TimezoneSetting.Name);

        row++;
        var exportText = viewModel.ExportType == ExportType.Filter
            ? await GetSectionForExport(viewModel.Section)
            : viewModel.GetExportText();
        worksheet.Cells[row, col].Value = worksheet.Cells[row, col].Value.ToString()
            .Replace("{export_type}", exportText);

        row++;
        var exportedTime = DateTimeOffset.UtcNow.AddMinutes(8 * 60 + exportModel.TimezoneSetting.Offset);
        worksheet.Cells[row, col].Value = worksheet.Cells[row, col].Value.ToString()
            .Replace("{print_date}", exportedTime.ToString("MMM dd, yyyy HH:mm:ss"))
            .Replace("{nick_name}", exportModel.User?.NickName)
            .Replace("{full_name}", exportModel.User?.FullName)
            .Replace("{total_record}", exportModel.WorkingShiftUsers.Count.ToString());

        // region Shift
        row++;
        var workingShiftColorExports = GetWorkingShiftGroupByDate(exportModel);
        ProcessShiftColors(worksheet, ref row, ref col, totalColumn, firstDayColumn, listDates, workingShiftColorExports);

        // region Section
        row++;
        var sectionHeaderRow = row;
        const int dayColumn = 4;
        col = dayColumn;
        foreach (var day in listDates)
        {
            worksheet.Cells[row, col].Value = $"{day.ToString("ddd").ToUpperInvariant()}\n{day.ToString("MMM dd").ToUpperInvariant()}";
            col++;
        }

        row++;
        ProcessWorkingShiftUsers(worksheet, exportModel, ref row, ref col, totalColumn, listDates);

        worksheet.PrinterSettings.RepeatRows = new ExcelAddress($"${sectionHeaderRow}:${sectionHeaderRow}");
    }

    private static void ProcessWorkingShiftUsers(ExcelWorksheet worksheet, WorkingShiftExportModel exportModel, ref int row, ref int col, int totalColumn, List<DateTime> listDates)
    {
        if (exportModel.WorkingShiftUsers.Any())
        {
            CloneRow(worksheet, row, exportModel.WorkingShiftUsers.Count, totalColumn);
            foreach (var item in exportModel.WorkingShiftUsers)
            {
                col = 1;
                worksheet.Cells[row, col].Value = item.SectionName ?? string.Empty;

                col++;
                worksheet.Cells[row, col].Value = item.EmployeeId;

                var isAddressTooLong = item.Address != null && item.Address?.Length > 25;
                var numberOfTimesTextLonger = item.Address?.Length / 25 + 1;
                var isPhoneTooLong = item.Phone != null && item.Phone?.Length > 14;
                var isDisplayNameTooLong = item.DisplayName != null && item.DisplayName?.Length > 12;

                if (isAddressTooLong || isPhoneTooLong || isDisplayNameTooLong)
                {
                    worksheet.Row(row).Height = numberOfTimesTextLonger.HasValue && numberOfTimesTextLonger.Value > 1 ?
                        numberOfTimesTextLonger.Value * 15 : 30;
                    worksheet.Row(row).Style.WrapText = true;
                }

                col++;
                worksheet.Cells[row, col].Value = item.DisplayName;

                col++;
                for (var i = 0; i < listDates.Count; i++)
                {
                    var workingShift = item.WorkingShifts[i];
                    if (workingShift != null)
                    {
                        var workingShiftItem = exportModel.WorkingShiftColors.FirstOrDefault(x => x.ShiftId == workingShift.ShiftId);
                        if (workingShiftItem != null)
                        {
                            if (workingShiftItem.ShiftCode?.Length > 7 && worksheet.Row(row).Height < 30)
                            {
                                worksheet.Row(row).Height = 30;
                            }
                            UpdateCellWithShift(worksheet.Cells[row, col], workingShiftItem);
                        }
                    }

                    col++;
                }

                worksheet.Cells[row, totalColumn - 1].Value = item.Address;
                worksheet.Cells[row, totalColumn].Value = item.Phone;

                row++;
            }
        }
    }

    private static void ProcessShiftColors(
        ExcelWorksheet worksheet,
        ref int row,
        ref int col,
        int totalColumn,
        int firstDayColumn,
        List<DateTime> listDates,
        List<WorkingShiftGroupByDateModel> workingShiftColorExports)
    {
        if (workingShiftColorExports.Any())
        {
            var shiftHeaderRow = row;
            row++;
            for (var i = 0; i < workingShiftColorExports.Count; i++)
            {
                worksheet.InsertRow(row + 1, 1);
                if (i != 0)
                {
                    worksheet.Cells[row, 1, row, totalColumn].Copy(worksheet.Cells[row + 1, 1, row + 1, totalColumn]);
                }
            }

            foreach (var item in workingShiftColorExports)
            {
                worksheet.Cells[row, 1].Value = item.WorkingShiftColor.ShiftCode;
                col = firstDayColumn;
                foreach (var date in listDates)
                {
                    if (item.WorkingShiftDates.ContainsKey(date))
                    {
                        worksheet.Cells[row, col].Value = item.WorkingShiftDates[date].Count;
                    }

                    col++;
                }

                row++;
            }

            for (var i = 3; i <= totalColumn - 2; i++)
            {
                var fromCol = i;
                var toCol = i;
                var totalEmployeeAddress = ExcelCellBase.GetAddress(shiftHeaderRow + 1, fromCol, row - 1, toCol);
                worksheet.Cells[shiftHeaderRow, i].Formula = $"SUM({totalEmployeeAddress})";
                worksheet.Cells[shiftHeaderRow, i].Calculate();
            }
        }
        else
        {
            row++;
        }
    }

    private static void CloneRow(ExcelWorksheet worksheet, int row, int totalRow, int totalColumn)
    {
        for (var i = 0; i < totalRow; i++)
        {
            worksheet.InsertRow(row + 1, 1);
            if (i != 0)
            {
                worksheet.Cells[row, 1, row, totalColumn].Copy(worksheet.Cells[row + 1, 1, row + 1, totalColumn]);
            }
        }
    }

    private static void UpdateCellWithShift(ExcelRange cell, WorkingShiftColorModel workingShiftItem)
    {
        cell.Value = workingShiftItem.ShiftCode;
        var backgroundColor = ColorTranslator.FromHtml(workingShiftItem.Background);
        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
        cell.Style.Fill.BackgroundColor.SetColor(backgroundColor);

        var foreColor = ColorTranslator.FromHtml(workingShiftItem.Color);
        cell.Style.Font.Color.SetColor(foreColor);
    }

    private async Task<WorkingShiftExportModel> BuildWorkingShiftExportModel(ExportModel model)
    {
        var searchUsers = await repository.SearchWorkingShiftUser(model);
        searchUsers = searchUsers.Where(x => x.Pickup).ToList();
        IList<WorkingShiftModel> workingShifts = new List<WorkingShiftModel>();
        IList<WorkingShiftColorModel> workingShiftColors = new List<WorkingShiftColorModel>();
        if (searchUsers.Any())
        {
            var userIds = string.Join(",", searchUsers.Select(x => x.UserId));
            workingShifts = await workingShiftRepository.QueryByUser(new WorkingShiftQueryByUserModel
            {
                UserIds = userIds,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            });

            workingShiftColors = await workingShiftColorRepository.GetAllByTimeZone(model.TimeZoneOffset);
            var shiftIds = workingShifts.Select(x => x.ShiftId).Distinct();
            workingShiftColors = workingShiftColors.Where(x => shiftIds.Contains(x.ShiftId)).ToList();
        }

        searchUsers = searchUsers.Order().AddWorkingShifts(model.FromDate, model.ToDate, workingShifts);
        return new WorkingShiftExportModel
        {
            User = await userRepository.GetUserInfoById(currentSession.UserId),
            TimezoneSetting = await timezoneSettingRepository.GetTimezoneSettingByOffset(model.TimeZoneOffset),
            WorkingShiftUsers = searchUsers,
            WorkingShiftColors = workingShiftColors,
            WorkingShifts = workingShifts
        };
    }

    private List<WorkingShiftGroupByDateModel> GetWorkingShiftGroupByDate(WorkingShiftExportModel exportModel)
    {
        return exportModel.WorkingShiftColors
            .Where(c => c.IsTimeShift)
            .Select(c =>
            {
                var workingShiftDates = exportModel.WorkingShifts
                    .Where(ws => ws.ShiftId == c.ShiftId)
                    .GroupBy(ws => ws.ShiftDate)
                    .ToDictionary(g => g.Key, g => g.ToList());
                return new WorkingShiftGroupByDateModel
                {
                    WorkingShiftColor = c,
                    WorkingShiftDates = workingShiftDates
                };
            })
            .ToList();
    }

    private async Task<string> GetSectionForExport(string sectionIds)
    {
        var resources = await resourceRepository.GetRefResourcesByName(ReferenceResourceNames.Section);
        var ids = sectionIds.Split(',');
        if (ids.Length - 1 == resources.Count())
        {
            return "All Sections";
        }

        var sectionNames = ids.Select(x =>
        {
            var sectionId = int.Parse(x);
            return sectionId == 0 ? "None" : resources.First(r => r.ResourceId == sectionId).Value;
        });

        return string.Join(", ", sectionNames);
    }
}