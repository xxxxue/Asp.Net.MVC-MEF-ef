using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

using MefBase.BaseEF;
using MefBase.IBaseEF;

namespace DAL.DBContext
{
    public abstract class  Base<TEntity>:RepositoryBase<TEntity> where  TEntity : Entity.BaseEntity.BaseEntity
    {
        [Import(typeof(IUnitOfWork))]

        public override IUnitOfWork UnitOfWork { get; set; }

    }
}
