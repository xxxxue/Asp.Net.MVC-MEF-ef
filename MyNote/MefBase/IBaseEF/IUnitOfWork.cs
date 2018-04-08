namespace MefBase.IBaseEF
{
    public interface IUnitOfWork
    {
        #region 属性

        /// <summary>
        /// 获取 当前单元操作是否已被提交
        /// </summary>
        bool IsCommitted { get; }

        #endregion

        #region 方法

        /// <summary>
        /// 提交当前单元操作的结果
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// 把当前单元操作回滚成未提交状态
        /// </summary>
        void Rollback();

        /// <summary>
        /// 释放单元操作中的DBContext等对象
        /// </summary>
        void Dispose();

        #endregion
    }
}
