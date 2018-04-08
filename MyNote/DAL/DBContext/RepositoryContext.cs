using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Data.Entity;
using MefBase.BaseEF;
using MefBase.IBaseEF;


namespace DAL.DBContext
{
    [Export(typeof(IUnitOfWork))]
   public class RepositoryContext : UnitOfWorkContextBase
    {
        /// <summary>
        /// 获取或设置 当前使用的只读数据访问上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get { return DbContext; }
        }

        private DbContext _dbContext;

        /// <summary>
        /// 获取或设置只读数据访问上下文对象
        /// </summary>
        public DbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = new DbContextBase("MyNoteDBEntities")); }
            set { _dbContext = value; }
        }
    }
}
