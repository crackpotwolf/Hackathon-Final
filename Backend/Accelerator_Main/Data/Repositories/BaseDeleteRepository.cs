using Data.Interfaces._BaseEntities;
using Data.Interfaces.Repositories;
using Data.Models.DB.Files;
using Data.Models.DB.Project;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public partial class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : class, IBaseEntity
    {
        /// <summary>
        /// Удаление объекта c зависимостями
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool RemoveCascade(T model)
        {
            if (model == null) return true;

            try
            {
                // Получить имя
                var name = typeof(T).Name;

                // Выбрать удаление
                switch (name)
                {
                    // Ниокр
                    case nameof(Project): RemoveCascadeNiokr(model); break;

                    // Информация о документе
                    case nameof(DocumentInfo): RemoveCascadeDocumentInfo(model.Guid); break;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");

                throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }
        }

        /// <summary>
        /// Удаление ниокра
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected bool RemoveCascadeNiokr(T model)
        {
            try
            {
                model.IsDelete = true;
                _db.Update(model);

                // DocumentInfo
                var modelsDocumentInfo = _db.DocumentInfos.Where(p => p.ProjectGuid == model.Guid).ToList();

                foreach (var modelDocumentInfo in modelsDocumentInfo)
                    RemoveCascadeDocumentInfo(modelDocumentInfo.Guid);

                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");

                throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }
        }


        /// <summary>
        /// Удаление информации о документе
        /// </summary>
        /// <param name="modelGuid">Guid</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected bool RemoveCascadeDocumentInfo(Guid modelGuid)
        {
            try
            {
                var model = _db.DocumentInfos.FirstOrDefault(p => p.Guid == modelGuid);

                model.IsDelete = true;

                // FileVersion
                var modelsFileVersion = _db.FileVersions.Where(p => p.DocumentInfoGuid == model.Guid)
                    .ToList();
                foreach (var modelFileVersion in modelsFileVersion) modelFileVersion.IsDelete = true;

                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");

                throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }
        }
    }
}