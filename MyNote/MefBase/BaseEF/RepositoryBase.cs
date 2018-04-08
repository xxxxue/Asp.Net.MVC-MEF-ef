using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EntityFramework.Extensions;
using MefBase.IBaseEF;

namespace MefBase.BaseEF
{
    public abstract  class RepositoryBase<TEntity>:IRepository<TEntity> where TEntity:Entity.BaseEntity.BaseEntity
    {
        #region 属性

        /// <summary>
        /// 获取只读仓储上下文的实例
        /// </summary>
        public abstract IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 获取或设置 EntityFramework的 只读 数据仓储上下文
        /// </summary>
        protected IUnitOfWorkContext Context
        {
            get
            {
                if (UnitOfWork is IUnitOfWorkContext)
                {
                    return UnitOfWork as IUnitOfWorkContext;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取当前实体的查询数据集(通过读上下文进行读取，只读专用，返回的实体数据不会被上下文跟踪)
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get
            {
                return Context.Set<TEntity>().AsNoTracking();
            }
        }

        /// <summary>
        /// 使用存储过程调查询所需实体集合
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parametersName">传递参数名称以@开头，多参数中间用逗号分隔</param>
        /// <param name="parameters">传递参数值</param>
        /// <returns></returns>
        public virtual List<TEntity> GetEntitiesByPro(string procName, string parametersName, params object[] parameters)
        {
            return Context.Set<TEntity>().SqlQuery(procName + " " + parametersName, parameters).AsNoTracking().ToList();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(TEntity entity, bool isSave = true)
        {
            Context.RegisterNew(entity);
            return isSave ? Context.Commit() : 0;
        }

        /// <summary>
        /// 批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            Context.RegisterNew(entities);
            return isSave ? Context.Commit() : 0;
        }

        /// <summary>
        /// 删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(object id, bool isSave = true)
        {
            var entity = Context.Set<TEntity>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        /// <summary>
        /// 删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(TEntity entity, bool isSave = true)
        {
            Context.RegisterDeleted(entity);
            return isSave ? Context.Commit() : 0;
        }

        /// <summary>
        /// 删除实体记录集合（单元操作，批量处理建议使用重载方法)
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool isSave = true)
        {
            Context.RegisterDeleted(entities);
            return isSave ? Context.Commit() : 0;
        }

        /// <summary>
        /// 删除所有符合特定表达式的数据（非单元操作，适合批量操作）
        /// </summary>
        /// <param name="filterExpression"> 查询条件谓语表达式 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Context.Set<TEntity>().Delete(filterExpression);
        }

        /// <summary>
        /// 更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Update(TEntity entity, bool isSave = true)
        {
            Context.RegisterModified(entity);
            return isSave ? Context.Commit() : 0;
        }

        /// <summary>
        /// 批量修改实体记录（推荐：高效率，无需查询至内存中处理！）
        /// </summary>
        /// <param name="filterExpression">筛选符合目标条件的记录表达式</param>
        /// <param name="updateExpression">更新实体表达式</param>
        /// <returns></returns>
        public virtual int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression)
        {
            return Context.Set<TEntity>().Where(filterExpression).Update(updateExpression);
        }

        /// <summary>
        /// 查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <param name="isReadOnly">是否使用只读上下文</param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        public virtual TEntity GetByKey(object key, bool isReadOnly = true)
        {
            return Context.Set<TEntity>().Find(key);
        }

        /// <summary>
        /// 执行存储过程（非单元操作，不能使用EF事务提交）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        public virtual void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            Context.ExecuteSqlCommand(sql, parameters);
            
        }

        #endregion
    }
}
