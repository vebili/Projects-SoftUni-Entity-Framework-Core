using Quiz.Data;
using Quiz.Models;
using Quiz.Services.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserAnswerService(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void AddUserAnswer(string userId, int quizId, int questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                IdentityUserId = userId,
                QuizId = quizId,
                QuestionId = questionId,
                AnswerId = answerId
            };

            this.applicationDbContext.UserAnswers.Add(userAnswer);
            this.applicationDbContext.SaveChanges();
        }

        public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        {
            var userAnswers = new List<UserAnswer>();

            foreach (var item in quizInputModel.Questions)
            {
                var userAnswer = new UserAnswer
                {
                    IdentityUserId = quizInputModel.UserId,
                    QuizId = quizInputModel.QuizId,
                    AnswerId = item.AnswerId,
                    QuestionId = item.QuestionId
                };

                userAnswers.Add(userAnswer);
            }

            this.applicationDbContext.AddRange(userAnswers);
            this.applicationDbContext.SaveChanges();
        }

        public int GetUserResult(string userId, int quizId)
        {
            var totalPoints = this.applicationDbContext.Quizes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .ThenInclude(x => x.UserAnswers)
                .Where(x => x.Id == quizId && 
                    x.UserAnswers.Any(x => x.IdentityUserId == userId))
                .SelectMany(x => x.UserAnswers)
                .Where(x => x.Answer.IsCorrect)
                .Sum(x => x.Answer.Points);

            return totalPoints;
        }
    }
}
