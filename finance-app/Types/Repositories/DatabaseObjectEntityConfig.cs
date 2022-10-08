
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_app.Types.Repositories {

    public class DatabaseObjectEntityConfig<T> : IEntityTypeConfiguration<T> where T : DatabaseObject 
    {
        protected string TableName { get ;set; }

        public DatabaseObjectEntityConfig(string tableName) {
            TableName = tableName;
        }

        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.ToTable(TableName);
            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.DateCreated)
                .ValueGeneratedOnAdd();
            builder.Property(e => e.DateLastEdited)
                .ValueGeneratedOnUpdate();
        }
    }  
}