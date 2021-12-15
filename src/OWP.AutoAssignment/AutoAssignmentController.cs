using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.AutoAssignment.Services.InternalApi;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OWP.AutoAssignment
{
    [Route("api/auto-assignment")]
    [ApiController]
    [Authorize]
    [EnableCors("AllowOrigin")]
    public class AutoAssignmentController : ControllerBase
    {
        private readonly CreateRequestService createRequestService;

        private readonly ExportExcelService exportExcelService;



        public AutoAssignmentController(
            CreateRequestService createRequestService,
            ExportExcelService exportExcelService)
        {
            this.createRequestService = createRequestService;
            this.exportExcelService = exportExcelService;
        }

        [Route("remove-request")]
        [HttpPost]
        public async Task RemoveRequest(CheckRequestModel model)
          => await createRequestService.RemoveRequest(model.RequestId);

        [Route("get-request")]
        [HttpGet]
        public async Task<IEnumerable<SchedulingRequest>> GetRequests()
            => await createRequestService.GetRequests();

        [Route("check-request")]
        [HttpPost]
        public async Task<int> CheckRequest(CheckRequestModel model)
            => await createRequestService.GetRequestStatus(model.RequestId);

        [Route("create-request")]
        [HttpPost]
        public async Task<IActionResult> CreateRequest(CreateRequestModel model)
        {
            var existsMatchValid = await createRequestService.ExistMatchValid(model.EventDate);
            var existsTraderValid = await createRequestService.ExistsSuspenderValid(model.EventDate);
            var requestIdIsWaiting = await createRequestService.GetRequestIsWaiting(model.EventDate);
            if (!string.IsNullOrWhiteSpace(requestIdIsWaiting))
            {
                var message = string.Format(AutoConstant.ExistsRequestIdIsWaiting, requestIdIsWaiting);
                return BadRequest(message);
            }
            if (!existsMatchValid)
            {
                return BadRequest(AutoConstant.NoFindAnyMatchValidOnSelectedDate);
            }
            if (!existsTraderValid)
            {
                return BadRequest(AutoConstant.NoFindAnyTraderValidOnSelectedDate);
            }
            await createRequestService.CreateRequest(model);
            return Ok();
        }

        [Route("export-excel")]
        [HttpGet]
        public async Task<HttpResponseMessage> ExportExcel(DateTime eventDate)
        {
            var exportExcelModel = new ExportModel
            {
                // TODO
            };
            var absolutedTemplatePath = @"App_Data\Templates\AutoAssignment.xlsx";
            var excelPackage = exportExcelService.CreateExcelPackage(absolutedTemplatePath, exportExcelModel);

            var bin = excelPackage.GetAsByteArray();

            return BuildExcelPackageResponse(bin, eventDate);

        }

        private HttpResponseMessage BuildExcelPackageResponse(byte[] bin, DateTime eventDate)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(bin)
            };
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = $"Auto_Assignment_{eventDate.ToString("yyyyMMdd")}.xlsx"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xlsx");
            return result;
        }
    }
}