using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Criteria.PublicApi;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.Models;
using OWP.Utils;
using System.Xml.Linq;
using Olympus.Xml;

namespace OWP.AutoAssignment
{
    public interface Repository
    {
        Task<IEnumerable<SchedulingRequest>> GetRequestsInternal();

        Task<IEnumerable<RequestModel>> GetPublicRequests();

        Task CreateRequest(CreateRequestCriteria criteria);

        Task<IEnumerable<RealMatchCommonInfo>> GetRealMatchCommonInfoByIds(IList<int> ids);

        Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchCommonInfoByIds(IList<int> ids);

        Task<IEnumerable<MatchQueried>> GetMatchByDate(DateTime eventDate);

        Task<ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>>> GetLAInfoByDate(DateTime eventDate);

        Task<ValueTuple<IEnumerable<SchedulingTraderInfo>, IEnumerable<UserWorkingShift>>> GetTraderInfoByDate(DateTime eventDate);

        Task<ResultDataObject> SaveRequest(SaveRequestModel model);

        Task UpdateStatusRequest(StatusRequestModel statusRequestModel);

        Task<ValueTuple<IEnumerable<SchedulingTraderInfo>, IEnumerable<UserWorkingShift>>> GetTraders(DateTime eventDate);

        Task<IEnumerable<int>> GetUserInfoFromNAPUserControl();

        Task<IEnumerable<int>> GetCustInfoFromDB02();

        Task<string> GetRequestIsWaiting(DateTime eventDate);

        Task<IEnumerable<MaxBetOfFTHDP>> GetMaxMaxbetFTHDP(IList<int> matchIds);

        Task<IEnumerable<MaxBetCorner>> GetMaxBetCorner(IList<MaxBetOfFTHDP> events);

        Task<IEnumerable<MaxBetFTHDPInfo>> GetMaxMaxBetFTHDPInfo(IList<int> matchIds);

        Task<int> GetRequestStatus(string requestId);

        Task RemoveRequest(string requestId);

        Task<IEnumerable<LAExportInfo>> GetLAExportInfo(GetLAExportInfoCriteria criteria);

        Task<IEnumerable<TraderExportInfo>> GetTraderExportInfo(GetTraderExportInfoCriteria criteria);

        Task<IEnumerable<AutoAssignmentResultModel>> GetAutoAssignmentResultByRequestId(string requestId);

        Task<IEnumerable<InfoSiteMatchModel>> GetInfoSiteMatchInfo(IList<MatchQueryInfo> events);

        Task<IEnumerable<ProviderInfo>> GetProviderInfo(IList<int> matchIds, string requestId);

        Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(IList<int> matchIds, string requestId);

        Task SaveEventSender(IEnumerable<AutoAssignmentHistory> events);

        Task<string> GetLatestRequestIdByDate(DateTime eventDate);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly INapSettings napSettings;

        public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings)
        {
            this.dynamicRepository = dynamicRepository;
            this.napSettings = napSettings;
        }

        public async Task<IEnumerable<SchedulingRequest>> GetRequestsInternal()
        {
            var criteria = new GetRequestsCriteria();
            var allRequest = await dynamicRepository.FetchAsync<SchedulingRequest>(criteria);
            return allRequest.Any()
                ? allRequest.OrderByDescending(r => r.CreatedDate).ToList()
                : new List<SchedulingRequest>();
        }

        public async Task<IEnumerable<RealMatchCommonInfo>> GetRealMatchCommonInfoByIds(IList<int> ids)
        {
            var criteria = new GetCommonRealMatchInfoByIdsCriteria
            {
                Ids = string.Join(",", ids)
            };

            return await dynamicRepository.FetchAsync<RealMatchCommonInfo>(criteria);
        }

        public async Task<IEnumerable<SabaMatchCommonInfo>> GetSabaMatchCommonInfoByIds(IList<int> ids)
        {
            var criteria = new GetCommonSabaMatchInfoByIdsCriteria
            {
                Ids = string.Join(",", ids)
            };

            return await dynamicRepository.FetchAsync<SabaMatchCommonInfo>(criteria);
        }

        public async Task<IEnumerable<RequestModel>> GetPublicRequests()
        {
            var criteria = new NewRequestCriteria();
            return await dynamicRepository.FetchAsync<RequestModel>(criteria);
        }

        public async Task<(IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>)> GetLAInfoByDate(DateTime eventDate)
        {
            var criteria = new GetLAInfoCriteria
            {
                SiteId = napSettings.SiteId,
                EventDate = eventDate
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchMultipleAsync<LAUserInfo, UserWorkingShift>(criteria);
        }

        public async Task<(IEnumerable<SchedulingTraderInfo>, IEnumerable<UserWorkingShift>)> GetTraderInfoByDate(DateTime eventDate)
        {
            var criteria = new GetTradersByDateCriteria
            {
                SiteId = napSettings.SiteId,
                EventDate = eventDate
            };

            return await dynamicRepository.FetchMultipleAsync<SchedulingTraderInfo, UserWorkingShift>(criteria);
        }

        public async Task<IEnumerable<MatchQueried>> GetMatchByDate(DateTime eventDate)
        {
            var criteria = new GetInfoSiteMatchByDateCriteria
            {
                EventDate = eventDate
            };
            return await dynamicRepository.FetchAsync<MatchQueried>(criteria);
        }

        public async Task UpdateStatusRequest(StatusRequestModel statusRequestModel)
        {
            var criteria = new UpdateStatusRequestCriteria
            {
                RequestId = statusRequestModel.RequestId,
                Status = statusRequestModel.Status
            };
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<(IEnumerable<SchedulingTraderInfo>, IEnumerable<UserWorkingShift>)> GetTraders(DateTime eventDate)
        {
            {
                var criteria = new GetTradersByDateCriteria
                {
                    SiteId = napSettings.SiteId,
                    EventDate = eventDate
                };

                return await dynamicRepository.FetchMultipleAsync<SchedulingTraderInfo, UserWorkingShift>(criteria);
            }
        }

        public async Task<IEnumerable<int>> GetCustInfoFromDB02()
            => await dynamicRepository.FetchAsync<int>(new GetCustInfoFromDB02Criteria());

        public async Task<IEnumerable<int>> GetUserInfoFromNAPUserControl()
            => await dynamicRepository.FetchAsync<int>(new GetUserInfoFromNAPUserControlCriteria { SiteId = napSettings.SiteId });

        public async Task<string> GetRequestIsWaiting(DateTime eventDate)
        {
            var criteria = new GetRequestIsWaitingCriteria
            {
                EventDate = eventDate
            };
            var result = await dynamicRepository.FetchAsync<RequestIsWaitingModel>(criteria);
            return result.Any()
                ? string.Join(",", result.Select(req => req.RequestId))
                : string.Empty;
        }

        public async Task CreateRequest(CreateRequestCriteria criteria)
            => await dynamicRepository.ExecuteAsync(criteria);

        public async Task<IEnumerable<MaxBetOfFTHDP>> GetMaxMaxbetFTHDP(IList<int> matchIds)
            => await dynamicRepository.FetchAsync<MaxBetOfFTHDP>(
                new GetMaxMaxbetFTHDPCriteria { MatchIds = string.Join(',', matchIds) });

        public async Task<IEnumerable<MaxBetCorner>> GetMaxBetCorner(IList<MaxBetOfFTHDP> events)
        {
            var xml = XmlConverter.Serialize(events);
            var criteria = new GetMaxBetCornerCriteria
            {
                Xml = xml
            };
            return await dynamicRepository.FetchAsync<MaxBetCorner>(criteria);
        }

        public async Task<IEnumerable<MaxBetFTHDPInfo>> GetMaxMaxBetFTHDPInfo(IList<int> matchIds)
            => await dynamicRepository.FetchAsync<MaxBetFTHDPInfo>(
                new GetMaxMaxBetFTHDPInfoCriteria { MatchIds = string.Join(',', matchIds) });

        public async Task<int> GetRequestStatus(string requestId)
        {
            var criteria = new RequestStatusCriteria
            {
                RequestId = requestId
            };
            var requestStatus = await dynamicRepository.FetchAsync<int>(criteria);
            return requestStatus.FirstOrDefault();
        }

        public async Task RemoveRequest(string requestId)
        {
            var criteria = new RemoveRequestCriteria
            {
                RequestId = requestId
            };
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<ResultDataObject> SaveRequest(SaveRequestModel model)
        {
            var result = new List<ResultDataObject>();
            var criteria = new SaveRequestCriteria
            {
                RequestId = model.RequestId,
                Status = model.Code,
                AutoAssignmentResultDoc = XmlConverter.Serialize(model.Data)
            };
            criteria.ThrowExceptionIfInvalid();
            await dynamicRepository.ExecuteAsync(criteria);
            result.Add(new ResultDataObject
            {
                Data = model.RequestId,
                Code = criteria.ErrCode
            });

            return new ResultDataObject
            {
                Code = result.Any(x => x.Code == 0) ? 0 : 1,
                Data = result
            };
        }

        public async Task<IEnumerable<LAExportInfo>> GetLAExportInfo(GetLAExportInfoCriteria criteria)
        => await dynamicRepository.FetchAsync<LAExportInfo>(criteria);

        public async Task<IEnumerable<TraderExportInfo>> GetTraderExportInfo(GetTraderExportInfoCriteria criteria)
        => await dynamicRepository.FetchAsync<TraderExportInfo>(criteria);

        public async Task<IEnumerable<AutoAssignmentResultModel>> GetAutoAssignmentResultByRequestId(string requestId)
        {
            var criteria = new GetAutoAssignmentResultCriteria
            {
                RequestId = requestId
            };
            return await dynamicRepository.FetchAsync<AutoAssignmentResultModel>(criteria);
        }

        public async Task<IEnumerable<InfoSiteMatchModel>> GetInfoSiteMatchInfo(IList<MatchQueryInfo> events)
        {
            var xml = XmlConverter.Serialize(events);
            var criteria = new GetInfoSiteMatchByIdAndBetTypeCriteria
            {
                Xml = xml
            };
            return await dynamicRepository.FetchAsync<InfoSiteMatchModel>(criteria);
        }

        public async Task<IEnumerable<ProviderInfo>> GetProviderInfo(IList<int> matchIds, string requestId)
            => await dynamicRepository.FetchAsync<ProviderInfo>(
                new GetProviderInfoCriteria
                {
                    MatchIds = string.Join(',', matchIds),
                    RequestId = requestId
                });

        public async Task<IEnumerable<LeagueSettingInfo>> GetLeagueSettingInfo(IList<int> matchIds, string requestId)
            => await dynamicRepository.FetchAsync<LeagueSettingInfo>(
                new GetLeagueSettingInfoCriteria
                {
                    MatchIds = string.Join(',', matchIds),
                    RequestId = requestId
                });

        public async Task SaveEventSender(IEnumerable<AutoAssignmentHistory> events)
        {
            var xml = XmlConverter.Serialize(events);
            var criteria = new SaveEventSenderCriteria
            {
                Xml = xml
            };
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<string> GetLatestRequestIdByDate(DateTime eventDate)
        {
            var criteria = new GetLatestRequestByDateCriteria
            {
                EventDate = eventDate
            };
            return (await dynamicRepository.FetchAsync<string>(criteria))
                .FirstOrDefault();
        }
    }
}