using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.ConfigurationsBase
{
    public static class BaseModelConfiguration
    {
        public static void ConfigureBaseModel<T>(this EntityTypeBuilder<T> builder)where T : BaseModel
        {
            builder.HasKey(x => x.id);
            builder.Property(x => x.active).IsRequired();
            builder.Property(x => x.is_deleted).IsRequired();
            //builder.Property(x => x.created_date).IsRequired();
        }
    }
}
