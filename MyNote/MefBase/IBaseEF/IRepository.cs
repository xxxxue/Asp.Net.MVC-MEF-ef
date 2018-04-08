using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MefBase.IBaseEF
{
    public interface IRepository<TEntity> where TEntity: Entity.BaseEntity.BaseEntity
    {
        #region 属性

        /// <summary>
        /// 获取 当前实体的查询数据集(通过读上下文进行读取)
        /// </summary>
        IQueryable<TEntity> Entities { get;}

        /// <summary>
        /// 实现存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parametersName">存储过程参数加@,多参数用逗号分隔</param>
        /// <param name="parameters">参数的付值</param>
        /// <returns></returns>
        List<TEntity> GetEntitiesByPro(string procName, string parametersName, params object[] parameters);

        #endregion

        #region 公共方法

        /// <summary>
        /// 插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(TEntity entity, bool isSave = true);

        /// <summary>
        /// 批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Insert(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        /// 删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(object id, bool isSave = true);

        /// <summary>
        /// 删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(TEntity entity, bool isSave = true);

        /// <summary>
        /// 删除实体记录集合（单元操作，批量处理建议使用重载方法 )
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(IEnumerable<TEntity> entities, bool isSave = true);

        /// <summary>
        /// 删除所有符合特定表达式的数据（非单元操作，适合批量操作，推荐使用：直接执行SQL，不需要查询实体）
        /// </summary>
        /// <param name="filterExpression"> 查询条件谓语表达式 </param>
        /// <returns> 操作影响的行数 </returns>
        int Delete(Expression<Func<TEntity, bool>> filterExpression);

        /// <summary>
        /// 更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        int Update(TEntity entity, bool isSave = true);

        /// <summary>
        /// 查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <param name="isReadOnly">是否使用只读上下文</param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        TEntity GetByKey(object key, bool isReadOnly = true);

        /// <summary>
        /// 批量修改实体记录（效率高，无需查询至内存中处理）
        /// </summary>
        /// <param name="filterExpression">筛选符合目标条件的记录表达式</param>
        /// <param name="updateExpression">更新实体表达式</param>
        /// <returns></returns>
        int Update(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TEntity>> updateExpression);

        #endregion
    }
}
