using System.Drawing;
namespace Entity.Clases
{
    public class EventPhoto
    {
        /// <summary>
        /// Код события
        /// </summary>
        public int idEvent { get; set; }
        /// <summary>
        /// Код считывателя
        /// </summary>
        public int idResource { get; set; }
        /// <summary>
        /// Код персоны
        /// </summary>
        public int idStaff { get; set; }
        /// <summary>
        /// Код нарушения
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// Фотография
        /// </summary>

        public Bitmap Bitmap { get; set; }
    }
}

