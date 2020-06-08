using System;

namespace Data
{
    /// <summary>
    /// Класс контактов
    /// </summary>
    public class Contact
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int Member1_ID { get; set; }
        public int Member2_ID { get; set; }
    }
}
