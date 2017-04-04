using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Vivelin.Home.Data;

namespace Vivelin.Home.Migrations
{
    [DbContext(typeof(HomeContext))]
    partial class HomeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Vivelin.Home.Data.Quote", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Citation");

                    b.Property<string>("Text");

                    b.HasKey("ID");

                    b.ToTable("Quotes");
                });
        }
    }
}
