using Data.Models.DB._BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.DB
{
    /// <summary>
    /// Заявка на инновацию
    /// </summary>
    public class InnovationRequest : BaseEntity
    {
        /// <summary>
        /// Опишите проблему
        /// </summary>
        public string Problem { get; set; }

        /// <summary>
        /// Как проявляется проблема?
        /// </summary>
        public string ProblemDescription { get; set; }

        /// <summary>
        /// Каковы последствия проблемы?
        /// </summary>
        public string ProblemEffects { get; set; }

        /// <summary>
        /// Почему возникла проблема
        /// </summary>
        public string ProblemReason { get; set; }

        /// <summary>
        /// Кого затрагивает проблема
        /// </summary>
        public string ProblemArea { get; set; }

        /// <summary>
        /// Когда желательно решить проблему
        /// </summary>
        public string ProblemTiming { get; set; }

        /// <summary>
        /// Как пытались решить проблему
        /// </summary>
        public string SolvingAttempts { get; set; }

        /// <summary>
        /// Как с вами связаться
        /// </summary>
        public string Contacts { get; set; }
    }
}
