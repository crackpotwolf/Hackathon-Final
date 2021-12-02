using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enum.Status
{
    /// <summary>
    /// Статус готовности
    /// </summary>
    public enum CompleteStatus
    {
        /// <summary>
        /// Не завершено
        /// </summary>
        [Display(Name = "Не завершено")]
        Incomplete,

        /// <summary>
        /// Завершено успешно
        /// </summary>
        [Display(Name = "Завершено успешно")]
        CompleteSuccess,

        /// <summary>
        /// Завершено с ошибкой 
        /// </summary> 
        [Display(Name = "Завершено с ошибкой")]
        CompleteError
    }
}