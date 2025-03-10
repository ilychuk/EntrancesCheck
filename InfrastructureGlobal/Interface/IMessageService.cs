using System.Windows.Forms;

namespace InfrastructureGlobal.Interface
{
    public interface IMessageService
    {
        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        void ShowMessage(string message);
        /// <summary>
        /// Вывод предупреждения
        /// </summary>
        /// <param name="exclamation">Текст сообщения</param>
        void ShowExclamation(string exclamation);
        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="error">Текст сообщения</param>
        void ShowError(string error);
        /// <summary>
        /// Вопрос
        /// </summary>
        /// <param name="question">Текст вопроса</param>
        /// <returns>Результат выбора DiallogResult</returns>
        DialogResult ShowQuestion(string question);
    }
}
