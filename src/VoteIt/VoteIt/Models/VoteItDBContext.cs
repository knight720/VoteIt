using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VoteIt.Models
{
    public partial class VoteItDBContext : DbContext
    {
        public VoteItDBContext()
        {
        }

        public VoteItDBContext(DbContextOptions<VoteItDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Feed> Feed { get; set; }
        public virtual DbSet<FeedLike> FeedLike { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=VoteItDB;User ID=sa;Password=P@ssw0rd;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feed>(entity =>
            {
                entity.Property(e => e.FeedId).HasColumnName("Feed_Id");

                entity.Property(e => e.FeedCreatedDateTime)
                    .HasColumnName("Feed_CreatedDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeedCreatedUser)
                    .IsRequired()
                    .HasColumnName("Feed_CreatedUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FeedLike).HasColumnName("Feed_Like");

                entity.Property(e => e.FeedTitle)
                    .IsRequired()
                    .HasColumnName("Feed_Title")
                    .HasMaxLength(160);

                entity.Property(e => e.FeedValidFlag).HasColumnName("Feed_ValidFlag");
            });

            modelBuilder.Entity<FeedLike>(entity =>
            {
                entity.HasIndex(e => new { e.FeedLikeFeedId, e.FeedLikeCreatedUser })
                    .HasName("IDX_FeedId_CreatedUser");

                entity.Property(e => e.FeedLikeId).HasColumnName("FeedLike_Id");

                entity.Property(e => e.FeedLikeCreatedDateTime)
                    .HasColumnName("FeedLike_CreatedDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeedLikeCreatedUser)
                    .IsRequired()
                    .HasColumnName("FeedLike_CreatedUser")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FeedLikeFeedId).HasColumnName("FeedLike_FeedId");

                entity.Property(e => e.FeedLikeValidFlag).HasColumnName("FeedLike_ValidFlag");
            });
        }
    }
}
