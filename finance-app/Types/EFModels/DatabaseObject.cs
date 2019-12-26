using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace finance_app.Types.EFModels
{
    public class DatabaseObject
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateLastEdited { get; set; }
    }
}
