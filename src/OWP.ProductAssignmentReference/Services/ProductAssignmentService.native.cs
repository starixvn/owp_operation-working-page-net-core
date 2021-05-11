using System.Collections.Generic;
using System.Threading.Tasks;
using OWP.ProductAssignmentReference.Models;

namespace OWP.ProductAssignmentReference.Services
{
    public interface ProductAssignmentService
    {
        Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo);

        Task UpdateUserInfo(UserInfoEditor info);

        Task<IEnumerable<ReferenceResource>> GetResourceByName(string name);
    }

    public class ProductAssignmentServiceImp : ProductAssignmentService
    {
        private readonly ProductAssignmentRepository repository;

        public ProductAssignmentServiceImp(ProductAssignmentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<ReferenceResource>> GetResourceByName(string name)
            => await repository.GetResourceByName(name);

        public async Task<IList<UserInfo>> QueryUser(QueryUserInfo queryInfo)
            => await repository.QueryUser(queryInfo);

        public async Task UpdateUserInfo(UserInfoEditor info)
            => await repository.UpdateUserInfo(info);
    }
}
