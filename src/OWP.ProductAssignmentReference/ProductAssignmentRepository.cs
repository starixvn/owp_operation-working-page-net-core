using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;
using OWP.ProductAssignmentReference.Criteria;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference
{
    public interface ProductAssignmentRepository
    {
        Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

        Task UpdateUserInfo(UserInfoEditor info);

        Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);
    }

    public class ProductAssignmentRepositoryImp : ProductAssignmentRepository
    {
        private readonly IDynamicRepository dynamicRepository;

        public ProductAssignmentRepositoryImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
        {
            var criteria = new UserInfoCriteria
            {
                RoleId = queryInfo.RoleId,
                User = queryInfo.User,
                Section = queryInfo.Section
            };
            return (await dynamicRepository.FetchAsync<UserInfo>(criteria)).ToList();
        }

        public async Task UpdateUserInfo(UserInfoEditor info)
        {
            var userInfoEditorCriteria = new UserInfoEditorCriteria
            {
                UserId = info.UserId,
                Label = info.Label,
                Value = info.Value
            };
            await dynamicRepository.ExecuteAsync(userInfoEditorCriteria);
        }

        public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
            => await dynamicRepository.FetchAsync<ReferenceResource>(
                new GetRefResourceByNameCriteria
                {
                    RefName = name
                });
    }
}
