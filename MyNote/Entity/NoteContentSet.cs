using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("NoteContentSet")]
    public partial class NoteContentSet : BaseEntity.BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string SendDate { get; set; }
    }
}