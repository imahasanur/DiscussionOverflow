using Autofac.Core;
using Autofac.Extras.Moq;
using DiscussionOverflow.Application.Features.Services;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Domain.Repositories;
using Moq;
using Shouldly;

namespace DiscussionOverflow.Application.Tests
{
    public class QuestionManagementServiceTests
    {
        private AutoMock _mock;
        private Mock<IQuestionRepository> _questionRepositoryMock;
        private Mock<IVoteRepository> _voteRepositoryMock;
        private Mock<IApplicationUnitOfWork> _unitOfWorkMock;
        private QuestionManagementService _questionManagementService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [SetUp]
        public void Setup()
        {
            _questionRepositoryMock = _mock.Mock<IQuestionRepository>();
            _voteRepositoryMock = _mock.Mock<IVoteRepository>();
            _unitOfWorkMock = _mock.Mock<IApplicationUnitOfWork>();
            _questionManagementService = _mock.Create<QuestionManagementService>();
        }

        [TearDown]
        public void TearDown()
        {

            _questionRepositoryMock?.Reset();
            _voteRepositoryMock?.Reset();
            _unitOfWorkMock?.Reset();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _mock?.Dispose();
        }

        [Test]
        public async Task UpdateVoteCountAsync_UpdatesQuestionVote_WhenItemTypeIsQuestion()
        {
            // Arrange
            //id -> this Question/Answer id already should be on database
            //newVoter -> this user is not a voter of this own Question/Answer
            var id = new Guid("e580cf4d-fa7a-421d-8d0f-ab406d0a2e23");
            var type = "upvote";
            var itemType = "Question";
            var newVoter = "test1@gmail.com";

            var vote = new Vote { Id = new Guid("6559b465-83ae-4005-a415-2e24d2728cc4"), UpVote = 1, DownVote = 1, Voter = "skill3@gmail.com", TimeStamp = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) };
            _unitOfWorkMock.SetupGet(x => x.VoteRepository).Returns(_voteRepositoryMock.Object).Verifiable();
            _voteRepositoryMock.Setup(r => r.GetQuestionVoteAsync(id)).ReturnsAsync(new[] { vote });
            _voteRepositoryMock.Setup(r => r.GetByIdAsync(vote.Id)).ReturnsAsync(vote).Verifiable();
            
            // Act
            await _questionManagementService.UpdateVoteCountAsync(id, type, itemType, newVoter);

            // Assert
            vote.UpVote.ShouldBe(2);
            vote.Voter.ShouldBe("skill3@gmail.com,test1@gmail.com", StringCompareShould.IgnoreCase);
            _unitOfWorkMock.Verify(u => u.UpdateEntityAsync(vote), Times.Once);
            _unitOfWorkMock.Verify();
        }

        [Test]
        public async Task UpdateVoteCountAsync_UpdatesAnswerVote_WhenItemTypeIsAnswer()
        {
            // Arrange
            //id -> this Question/Answer id already should be on database
            //newVoter -> this user is not a voter of this own Question/Answer

            var id = new Guid("68503176-0314-4937-92e4-400a6f4f4472");
            var type = "upvote";
            var itemType = "Answer";
            var newVoter = "test2@gmail.com";

            var vote = new Vote { Id = new Guid("6aaf6885-7b65-44a0-9e65-9b89c28f7673"), UpVote = 2, DownVote = 1, Voter = "skill3@gmail.com,skill4@gmail.com", TimeStamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) };
            _unitOfWorkMock.SetupGet(x => x.VoteRepository).Returns(_voteRepositoryMock.Object).Verifiable();
            _voteRepositoryMock.Setup(r => r.GetAnswerVoteAsync(id)).ReturnsAsync(new[] { vote });
            _voteRepositoryMock.Setup(r => r.GetByIdAsync(vote.Id)).ReturnsAsync(vote).Verifiable();

            // Act
            await _questionManagementService.UpdateVoteCountAsync(id, type, itemType, newVoter);

            // Assert
            vote.UpVote.ShouldBe(3);
            vote.Voter.ShouldBe("skill3@gmail.com,skill4@gmail.com,test2@gmail.com", StringCompareShould.IgnoreCase);
            _unitOfWorkMock.Verify(u => u.UpdateEntityAsync(vote), Times.Once);
            _unitOfWorkMock.Verify();
        }
    }
}