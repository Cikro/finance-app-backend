using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.Repositories
{
    public class DatabaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }

        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        [Column("date_last_edited")]
        public DateTime DateLastEdited { get; set; }
    }
}
