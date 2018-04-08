using System;
using System.Collections.Generic;
using System.Data.Entity;
using MefBase.IBaseEF;

namespace MefBase.BaseEF
{
    public abstract class UnitOfWorkContextBase:IUnitOfWorkContext
    {

        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        protected abstract DbContext Context { get; }

        /// <summary>
        ///获取 当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        /// <summary>
        ///提交当前单元操作的结果
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                var result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        /// <summary>
        ///把当前单元操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            IsCommitted = false;
        }

        /// <summary>
        /// 释放内存资源
        /// </summary>
        public void Dispose()
        {
            //释放托管和非托管资源
            Dispose(true);

            //手动调用了Dispose释放资源
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 传入bool值disposing以确定是否释放托管资源
        /// </summary>
        /// <param name="disposing">是否释放资源</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;

            //未提交的事务进行提交
            if (!IsCommitted)
            {
                Commit();
            }

            //清理"托管资源"的代码
            Context.Dispose();
        }

        //供GC调用的析构函数
        ~UnitOfWorkContextBase()
        {
            Dispose(false); //释放非托管资源
        }

        /// <summary>
        ///为指定的类型返回 System.Data.Entity.DbSet，允许对上下文中的给定实体执行 CRUD 
        /// </summary>
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : Entity.BaseEntity.BaseEntity
        {
            return Context.Set<TEntity>();
        }

        /// <summary>
        ///注册一个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterNew<TEntity>(TEntity entity) where TEntity : Entity.BaseEntity.BaseEntity
        {
            var state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
                
            }
            IsCommitted = false;
        }

        /// <summary>
        ///批量注册多个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterNew<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity.BaseEntity.BaseEntity
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var entity in entities)
                {
                    RegisterNew(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>
        ///注册一个更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterModified<TEntity>(TEntity entity) where TEntity : Entity.BaseEntity.BaseEntity
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            IsCommitted = false;
        }

        /// <summary>
        ///注册一个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterDeleted<TEntity>(TEntity entity) where TEntity : Entity.BaseEntity.BaseEntity
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>
        ///批量注册多个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity.BaseEntity.BaseEntity
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var entity in entities)
                {
                    RegisterDeleted(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="parameters">参数</param>
        public void ExecuteSqlCommand(string sql, params object[] parameters)
        {
            Context.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
