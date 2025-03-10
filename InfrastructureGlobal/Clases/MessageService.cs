using System.Windows.Forms;
using InfrastructureGlobal.Interface;

namespace InfrastructureGlobal.Clases
{
    
    public class MessageService : IMessageService
    {
        /// <summary>
        /// Вывод сообщения
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, @"Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Вывод предупреждения
        /// </summary>
        /// <param name="exclamation">Текст сообщения</param>
        public void ShowExclamation(string exclamation)
        {
            MessageBox.Show(exclamation, @"Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        /// <summary>
        /// Ошибка
        /// </summary>
        /// <param name="error">Текст сообщения</param>
        public void ShowError(string error)
        {
            MessageBox.Show(error, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Вопрос
        /// </summary>
        /// <param name="question">Текст вопроса</param>
        /// <returns></returns>
        public DialogResult ShowQuestion(string question)
        {
            return MessageBox.Show(question, @"Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
