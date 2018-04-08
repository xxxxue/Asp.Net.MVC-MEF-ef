using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("UserInfoSet")]
    public partial class UserInfoSet : BaseEntity.BaseEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string RegisterDate { get; set; }
        public string LoginDate { get; set; }
    }
}