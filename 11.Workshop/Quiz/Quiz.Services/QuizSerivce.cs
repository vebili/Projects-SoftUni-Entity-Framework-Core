using System.Linq;
using Quiz.Data;
using Quiz.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Services
{
    public class QuizSerivce : IQuizService
    {
        private readonly ApplicationDbContext applicationDbContext;

        public QuizSerivce(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(string title)
        {
            var quiz = new Quiz.Models.Quiz
            {
                Title = title
            };

            this.applicationDbContext.Quizes.Add(quiz);
            this.applicationDbContext.SaveChanges();
        }

        public QuizViewModel GetQuizById(int quizId)
        {
            var quiz = this.applicationDbContext.Quizes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var quizViewModel = new QuizViewModel
            {
                Id = quizId,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(x => new QuestionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Answers = x.Answers.Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                    .ToList()
                })
                .ToList()
            };

            return quizViewModel;
        }
    }
}
