using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Domain.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entity.ConfigurationsBase
{
    public static class BaseModelGenericConfiguration
    {
        public static void configureBaseModelGeneric<T>(this EntityTypeBuilder<T> builder)
        where T : BaseModelGeneric
        {
            builder.ConfigureBaseModel();

            builder.Property(x => x.name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.description)
                .HasMaxLength(250);
        }
    }
}
