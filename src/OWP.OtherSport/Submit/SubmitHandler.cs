namespace OWP.OtherSport.Submit
{
    using MediatR;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SubmitRequest: IRequest<string>
    {
    }

    public class SubmitHandler: IRequestHandler<SubmitRequest, string>
    {
        private readonly AssignmentMaxbetMapper mapper;
        private readonly CacheManipulator cacheManipulator;
        private readonly CommandRepository commandRepository;

        public SubmitHandler(
            AssignmentMaxbetMapper mapper,
            CacheManipulator cacheManipulator,
            CommandRepository commandRepository)
        {
            this.mapper = mapper;
            this.cacheManipulator = cacheManipulator;
            this.commandRepository = commandRepository;
        }

        public async Task<string> Handle(SubmitRequest request, CancellationToken cancellationToken)
        {
            var newAssignments = await cacheManipulator.GetNewAssignments();
            var eventDateInString = string.Join(',', newAssignments.Select(a => a.EventDate));

            await SaveOldAssignmentsToSnapshot(eventDateInString);
            await SaveAssignmentsToDatabase(newAssignments, eventDateInString);
            return string.Empty;
        }

        public async Task SaveOldAssignmentsToSnapshot(string eventDateInString)
        {
            var oldSnapshot = await commandRepository.Query(eventDateInString);
            await cacheManipulator.SaveOldAssignmentSnapshot(oldSnapshot);
        }

        public async Task SaveAssignmentsToDatabase(List<AssignmentModel> newAssignments, string eventDateInString)
        {
            var oldMatchIds = await commandRepository.GetOldMatchIds(eventDateInString);
            await commandRepository.Update(oldMatchIds, newAssignments);
        }
    }
}
