using Data.Interfaces._BaseEntities;
using Data.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    /// <summary>
    /// Базовый репозиторий
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BaseEntityRepository<T> : IBaseEntityRepository<T> where T : class, IBaseEntity
    {
        private readonly AcceleratorContext _db;
        private readonly ILogger<IBaseEntityRepository<T>> _logger;

        public BaseEntityRepository(AcceleratorContext db,
            ILogger<IBaseEntityRepository<T>> logger)
        {
            _db = db;
            _logger = logger;
        }

        /// <summary>
        /// Обновление модели перед записью в БД
        /// </summary>
        /// <param name="model">Модель</param>
        /// <param name="nowUtc">Текущее время</param>
        /// <returns>Обновленная запись</returns>
        private T UpdateEntityBeforeSave(T model, DateTime nowUtc)
        {
            model.UpdateBeforeSave(nowUtc);
            return model;
        }
        ///<inheritdoc cref="UpdateEntityBeforeSave(T, DateTime)"/>
        private T UpdateEntityBeforeSave(T model)
        {
            return UpdateEntityBeforeSave(model, DateTime.UtcNow);
        }

        ///<inheritdoc cref="UpdateEntityBeforeSave(T)"/>
        ///<param name="models">Модели</param>
        private IEnumerable<T> UpdateEntityBeforeSave(IEnumerable<T> models)
        {
            var now = DateTime.UtcNow;
            return models.Select(p => UpdateEntityBeforeSave(p, now));
        }

        /// <summary>
        /// Добавляет объект
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual T Add(T model)
        {
            try
            {
                _db.Add(model);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении сущности ({GetNameEntity()}): {ex}");
                throw new Exception($"Ошибка при добавлении записи: {ex}");
            }
            return model;
        }

        /// <summary>
        /// Добавляет элементы указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> AddRange(IEnumerable<T> models)
        {
            try
            {
                _db.AddRange(models);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при добавлении сущностей ({GetNameEntity()}): {ex}");
                throw new Exception($"Ошибка при добавлении записей: {ex}");
            }
            return models;
        }

        /// <summary>
        /// Обновление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Update(T model)
        {
            try
            {
                _db.Update(UpdateEntityBeforeSave(model));
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обновлении сущности ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при обновлении файла: {ex}");
            }
            return false;
        }

        /// <summary>
        /// Обновление элементов указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual bool UpdateRange(IEnumerable<T> models)
        {
            try
            {
                _db.UpdateRange(UpdateEntityBeforeSave(models));
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при обновлении сущностей ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при обновлении файлов: {ex}");
            }

            return false;
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Remove(T model)
        {
            if (model == null) return true;

            try
            {
                model.IsDelete = true;

                _db.Update(UpdateEntityBeforeSave(model));
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }

            return false;
        }

        /// <summary>
        /// Удаление объекта по Guid
        /// </summary>
        /// <param name="Guid"></param>
        /// <returns></returns>
        public virtual bool Remove(Guid Guid)
        {
            try
            {
                var model = _db.Set<T>().AsNoTracking().FirstOrDefault(p => p.Guid == Guid);
                return Remove(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }
            return false;
        }

        /// <summary>
        /// Удаляет элементы указанной коллекции
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public virtual bool RemoveRange(IEnumerable<T> models)
        {
            try
            {
                foreach (var model in models)
                    model.IsDelete = true;

                _db.UpdateRange(UpdateEntityBeforeSave(models));
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущностей ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при удалении сущностей ({GetNameEntity()}): {ex}");
            }
            return false;
        }

        /// <summary>
        /// Удаляет элементы с идентификаторами в указанной коллекции
        /// </summary>
        /// <param name="guids">Идентификаторы</param>
        /// <returns></returns>
        public bool RemoveRange(IEnumerable<Guid> guids)
        {
            if (guids == null || guids.Count() == 0) return true;
            return RemoveRange(_db.Set<T>().Where(p => guids.Contains(p.Guid)));
        }

        /// <summary>
        /// Удаление объекта
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool Delete(T model)
        {
            try
            {
                _db.Remove(model);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
                //throw new Exception($"Ошибка при удалении сущности ({GetNameEntity()}): {ex}");
            }

            return false;
        }

        /// <summary>
        /// Получить коллекцию
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetListQuery()
        {
            return _db.Set<T>().AsNoTracking().Where(p => !p.IsDelete).AsQueryable();
        }

        /// <summary>
        /// Получить коллекцию с удаленными объектами
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetListQueryWithDeleted()
        {
            return _db.Set<T>().AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Получить коллекцию
        /// </summary>
        /// <returns></returns>
        public virtual List<T> GetList()
        {
            return _db.Set<T>().AsNoTracking().Where(p => !p.IsDelete).ToList();
        }

        /// <summary>
        /// Получить коллекцию с удаленными объектами
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<T> GetListWithDeleted()
        {
            return _db.Set<T>().AsNoTracking().AsQueryable();
        }

        /// <summary>
        /// Проверяет существование хотя бы одного элемента в последовательности 
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<T, bool>> func)
        {
            return GetListQuery().Any(func);
        }

        /// <summary>
        /// Возвращает первый элемент последовательности или значение по умолчанию, если ни одного элемента не найдено
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public virtual T FirstOrDefault(Expression<Func<T, bool>> func)
        {
            return GetListQuery().FirstOrDefault(func);
        }

        /// <summary>
        /// Получение записи по идентификатору
        /// </summary>
        /// <param name="guid">Guid</param>
        /// <returns></returns>
        public T Get(Guid guid)
        {
            return GetListQuery().FirstOrDefault(p => p.Guid == guid);
        }

        /// <summary>
        /// Возращает название сущности указанном в атрибуте: DisplayAttribute
        /// </summary>
        /// <returns></returns>
        private string GetNameEntity()
        {
            return typeof(T).CustomAttributes
                .FirstOrDefault(p => p.AttributeType.Name == "DisplayAttribute")
                .NamedArguments
                .FirstOrDefault(p => p.MemberName == "Name")
                .TypedValue
                .Value.ToString();
        }

        /// <summary>
        /// Получение записей с указанными Guid
        /// </summary>
        /// <param name="guids">Список Guid</param>
        /// <returns></returns>
        public IEnumerable<T> GetByGuids(IEnumerable<Guid> guids)
        {
            if (guids.Count() == 0) return new List<T>();
            return GetListQuery().Where(p => guids.Contains(p.Guid)).ToList();
        }
    }
}