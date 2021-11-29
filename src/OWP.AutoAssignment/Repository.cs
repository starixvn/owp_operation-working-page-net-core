using Fanex.Data.Repository;
using OWP.AutoAssignment.Criteria.PublicApi;
using OWP.AutoAssignment.Models.PublicApi;
using OWP.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OWP.AutoAssignment
{
    public interface Repository
    {
        Task<IEnumerable<RequestModel>> GetRequests();

        Task<IEnumerable<RealMatchCommonInfo>> GetRealMatchCommonInfoByIds(IList<int> ids);

        Task<IEnumerable<MatchQueried>> GetMatchByDate(DateTime eventDate);

        Task<ValueTuple<IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>>> GetLAInfoByDate(DateTime eventDate);

        Task<ValueTuple<IEnumerable<TraderInfo>, IEnumerable<UserWorkingShift>>> GetTraderInfoByDate(DateTime eventDate);

        Task UpdateStatusRequest(StatusRequestModel statusRequestModel);

        Task<ValueTuple<IEnumerable<TraderInfo>, IEnumerable<UserWorkingShift>>> GetTraders(DateTime eventDate);

        Task<IEnumerable<int>> GetUserInfoFromNAPUserControl();

        Task<IEnumerable<int>> GetCustInfoFromDB02();
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        public RepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<RealMatchCommonInfo>> GetRealMatchCommonInfoByIds(IList<int> ids)
        {
            var criteria = new GetCommonRealMatchInfoByIdsCriteria
            {
                Ids = string.Join(",", ids)
            };

            return await dynamicRepository.FetchAsync<RealMatchCommonInfo>(criteria);
        }

        public async Task<IEnumerable<RequestModel>> GetRequests()
        {
            var criteria = new NewRequestCriteria();
            return await dynamicRepository.FetchAsync<RequestModel>(criteria);
        }

        public async Task<(IEnumerable<LAUserInfo>, IEnumerable<UserWorkingShift>)> GetLAInfoByDate(DateTime eventDate)
        {
            var criteria = new GetLAInfoCriteria
            {
                // TODO
                SiteId = 99,
                EventDate = eventDate
            };
            criteria.ThrowExceptionIfInvalid();
            return await dynamicRepository.FetchMultipleAsync<LAUserInfo, UserWorkingShift>(criteria);
        }

        public async Task<(IEnumerable<TraderInfo>, IEnumerable<UserWorkingShift>)> GetTraderInfoByDate(DateTime eventDate)
        {
            var criteria = new GetTradersByDateCriteria
            {
                SiteId = 99,
                EventDate = eventDate
            };

            return await dynamicRepository.FetchMultipleAsync<TraderInfo, UserWorkingShift>(criteria);
        }

        public Task<IEnumerable<MatchQueried>> GetMatchByDate(DateTime eventDate)
        {

            throw new NotImplementedException();
        }

        public async Task UpdateStatusRequest(StatusRequestModel model)
        {
            var criteria = new UpdateStatusRequestCriteria
            {
                RequestId = model.RequestId,
                Status = model.Status
            };
            await dynamicRepository.ExecuteAsync(criteria);
        }

        public async Task<(IEnumerable<TraderInfo>, IEnumerable<UserWorkingShift>)> GetTraders(DateTime eventDate)
        {
            {
                var criteria = new GetTradersByDateCriteria
                {
                    SiteId = 99,
                    EventDate = eventDate
                };

                return await dynamicRepository.FetchMultipleAsync<TraderInfo, UserWorkingShift>(criteria);
            }
        }

        public async Task<IEnumerable<int>> GetCustInfoFromDB02()
            => await dynamicRepository.FetchAsync<int>(new GetCustInfoFromDB02Criteria());

        public async Task<IEnumerable<int>> GetUserInfoFromNAPUserControl()
            => await dynamicRepository.FetchAsync<int>(new GetUserInfoFromNAPUserControlCriteria { SiteId = 99 });
}
}