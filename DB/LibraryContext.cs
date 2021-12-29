using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DB.Models;

namespace DB
{
    public partial class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attributes> Attributes { get; set; } = null!;
        public virtual DbSet<AttributesNames> AttributesNames { get; set; } = null!;
        public virtual DbSet<Authors> Authors { get; set; } = null!;
        public virtual DbSet<Books> Books { get; set; } = null!;
        public virtual DbSet<Categories> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attributes>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.AttributeId })
                    .HasName("PK__Attribut__81F850992FAC5550");

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("value");

                entity.HasOne(d => d.AttributeName)
                    .WithMany(p => p.Values)
                    .HasForeignKey(d => d.AttributeId)
                    .HasConstraintName("FK__Attribute__Attri__75C27486");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Attributes)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("FK__Attribute__BookI__74CE504D");
            });

            modelBuilder.Entity<AttributesNames>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Authors>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("firstName");

                entity.Property(e => e.FullName)
                    .HasMaxLength(101)
                    .IsUnicode(false)
                    .HasColumnName("fullName")
                    .HasComputedColumnSql("(concat([firstName],' ',[LastName]))", false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("lastName");
            });

            modelBuilder.Entity<Books>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Cover)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cover");

                entity.Property(e => e.Descr)
                    .HasColumnType("text")
                    .HasColumnName("descr");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.HasMany(d => d.Author)
                    .WithMany(p => p.Book)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksAuthors",
                        l => l.HasOne<Authors>().WithMany().HasForeignKey("AuthorId").HasConstraintName("FK__BooksAuth__Autho__70099B30"),
                        r => r.HasOne<Books>().WithMany().HasForeignKey("BookId").HasConstraintName("FK__BooksAuth__BookI__6F1576F7"),
                        j =>
                        {
                            j.HasKey("BookId", "AuthorId").HasName("PK__BooksAut__6AED6DC4B0DBC00F");

                            j.ToTable("BooksAuthors");
                        });

                entity.HasMany(d => d.Category)
                    .WithMany(p => p.Book)
                    .UsingEntity<Dictionary<string, object>>(
                        "BooksCategories",
                        l => l.HasOne<Categories>().WithMany().HasForeignKey("CategoryId").HasConstraintName("FK__BooksCate__Categ__7B7B4DDC"),
                        r => r.HasOne<Books>().WithMany().HasForeignKey("BookId").HasConstraintName("FK__BooksCate__BookI__7A8729A3"),
                        j =>
                        {
                            j.HasKey("BookId", "CategoryId").HasName("PK__BooksCat__9C7051A7639E01F6");

                            j.ToTable("BooksCategories");
                        });
            });

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
