using Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DAL.DBContext
{
    public class DbContextBase : DbContext
    {
        #region /// 构造函数

        public DbContextBase() : base("MyNoteDBEntities")
        {
            //修改模型后~运行~会创建新的数据库
            Database.SetInitializer<DbContextBase>(new DropCreateDatabaseIfModelChanges<DbContextBase>());
        }

        public DbContextBase(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //修改模型后~运行~会创建新的数据库
            Database.SetInitializer<DbContextBase>(new DropCreateDatabaseIfModelChanges<DbContextBase>());
        }

        #endregion /// 构造函数

        #region /// 属性

        // 例子: public DbSet<Entity.Activitys.ActivityType> ActivityType { get; set; }

        /// <summary>
        /// 日记表
        /// </summary>
        public DbSet<NoteContentSet> NoteContentSet { get; set; }

        /// <summary>
        /// 用户表
        /// </summary>
        public DbSet<UserInfoSet> UserInfoSet { get; set; }

        #endregion /// 属性

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //移除自动建表时自动加上s的复数形式
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //移除一对多的级联删除约定
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //多对多启用级联删除约定
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }
    }
}