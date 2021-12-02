using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Services
{
    /// <summary>
    /// Результат отправки Email сообщения
    /// </summary>
    public class ResultEmailService
    {
        public ResultEmailService(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// True - если успешно отправлено Email сообщение
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Информация об ошибке
        /// </summary>
        public Exception Exception { get; set; }
    }
}