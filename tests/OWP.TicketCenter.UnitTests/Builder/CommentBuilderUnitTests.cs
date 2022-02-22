using OWP.TicketCenter.Builder;

using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.TicketCenter.UnitTests.Builder
{
    public class CommentBuilderUnitTests
    {
        [Fact]
        public void GetTwoLastComment_List_Comment_Return_TwoLastComment()
        {
            // Arrange
            var createdDateMessage1 = DateTime.Now.AddMinutes(5);

            var createdDateMessage2 = DateTime.Now.AddMinutes(10);

            var createdDateMessage3 = DateTime.Now.AddMinutes(15);

            var messageModel = new List<MessageResponseModel>
            {
                new()
                {
                    CreatedDate = createdDateMessage1,
                    Message = "Message 1",
                    MessageId = 1,
                    UserId = 8,
                    Username = "sa"
                },
                new()
                {
                    CreatedDate = createdDateMessage2,
                    Message = "Message 1",
                    MessageId = 1,
                    UserId = 8,
                    Username = "sa"
                },
                new()
                {
                    CreatedDate = createdDateMessage3,
                    Message = "Message 1",
                    MessageId = 1,
                    UserId = 8,
                    Username = "sa"
                }
            };

            // Act
            var actual = TwoLastCommentBuilder.GetTwoLastComment(messageModel);

            // Assert
            var expected = new List<MessageDetailViewModel>
            {
                new()
                {
                    CreatedDate = createdDateMessage3,
                    Message = "Message 1",
                    MessageId = 1,
                    UserId = 8,
                    Username = "sa"
                },
                new()
                {
                    CreatedDate = createdDateMessage2,
                    Message = "Message 1",
                    MessageId = 1,
                    UserId = 8,
                    Username = "sa"
                },
            };

            DeepAssert.Equal(expected, actual.ToList());
        }
    }
}