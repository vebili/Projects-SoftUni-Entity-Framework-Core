using Quiz.Services.Models;

namespace Quiz.Services
{
    public interface IQuizService
    {
        void Add(string title);

        QuizViewModel GetQuizById(int quizId);
    }
}
