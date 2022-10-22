﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WPPostFromVideoConsole.Context;

#nullable disable

namespace WPPostFromVideoConsole.Migrations
{
    [DbContext(typeof(VideoContext))]
    partial class VideoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11");

            modelBuilder.Entity("WPPostFromVideoConsole.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PostName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Status")
                        .HasColumnType("INTEGER")
                        .HasComment("Publish= 0, Future=1, Private=2 .Draft=3, Pending=4,Trash=5");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VideoIdx")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WordpressId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PostId");

                    b.HasIndex("VideoIdx");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WPPostFromVideoConsole.Models.Video", b =>
                {
                    b.Property<int>("Idx")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("INTEGER")
                        .HasComment("Wordpress Publication Status; Whether future or now = true. If video post exist in WP");

                    b.Property<DateTime?>("PublishedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Thumbnail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Idx");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("WPPostFromVideoConsole.Models.Post", b =>
                {
                    b.HasOne("WPPostFromVideoConsole.Models.Video", "Video")
                        .WithMany("PostParams")
                        .HasForeignKey("VideoIdx")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Video");
                });

            modelBuilder.Entity("WPPostFromVideoConsole.Models.Video", b =>
                {
                    b.Navigation("PostParams");
                });
#pragma warning restore 612, 618
        }
    }
}
